using System.IO;
using MergulhoAmbiental360.App;
using MergulhoAmbiental360.Config;
using MergulhoAmbiental360.UI;
using MergulhoAmbiental360.Utils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace MergulhoAmbiental360.Video360
{
    public class Video360PlayerController : MonoBehaviour
    {
        public VideoPlayer videoPlayer;
        public AudioSource audioSource;
        public Video360SphereSetup sphereSetup;
        public UIStateController uiStateController;

        [Header("UI")]
        public Text titleText;
        public Text statusText;
        public Text playPauseLabel;
        public Slider volumeSlider;

        private VideoItem currentVideo;

        private void Start()
        {
            EnsureDependencies();
            BindVolume();
            LoadSelectedVideo();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TogglePlayPause();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                BackToMenu();
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                PlayNextVideo();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartVideo();
            }
        }

        public void TogglePlayPause()
        {
            if (videoPlayer == null || currentVideo == null || !currentVideo.HasPlayableSource())
            {
                SetStatus("Video placeholder. Configure um arquivo ou URL para reproduzir.");
                return;
            }

            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
            else
            {
                videoPlayer.Play();
            }

            RefreshPlayPauseLabel();
            uiStateController?.ShowControls();
        }

        public void RestartVideo()
        {
            if (videoPlayer == null || currentVideo == null || !currentVideo.HasPlayableSource())
            {
                return;
            }

            videoPlayer.time = 0;
            videoPlayer.Play();
            RefreshPlayPauseLabel();
            uiStateController?.ShowControls();
        }

        public void PlayNextVideo()
        {
            VideoCatalog catalog = AppBootstrap.Instance != null ? AppBootstrap.Instance.Catalog : null;
            VideoItem next = catalog != null ? catalog.FindNextEnabled(currentVideo?.id) : null;
            if (next == null)
            {
                return;
            }

            SceneNavigationService.SelectVideo(next);
        }

        public void BackToMenu()
        {
            SceneNavigationService.LoadMainMenu();
        }

        public void SetVolume(float volume)
        {
            if (audioSource != null)
            {
                audioSource.volume = Mathf.Clamp01(volume);
            }
        }

        private void LoadSelectedVideo()
        {
            currentVideo = SceneNavigationService.GetSelectedVideo();
            if (currentVideo == null)
            {
                SetStatus("Nenhum video selecionado.");
                return;
            }

            if (titleText != null)
            {
                titleText.text = currentVideo.title;
            }

            if (!currentVideo.HasPlayableSource())
            {
                SetStatus("Placeholder: adicione um video 360 no catalogo para reproduzir.");
                RefreshPlayPauseLabel();
                return;
            }

            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = ResolveVideoUrl(currentVideo);
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.errorReceived += OnVideoError;
            videoPlayer.Prepare();
            SetStatus("Carregando video...");
        }

        private string ResolveVideoUrl(VideoItem item)
        {
            return item.videoSourceType switch
            {
                VideoSourceType.LocalFile => BuildStreamingAssetsVideoPath(item.localFileName),
                VideoSourceType.StreamingUrl => item.streamingUrl,
                _ => string.Empty
            };
        }

        private string BuildStreamingAssetsVideoPath(string fileName)
        {
            string basePath = Application.streamingAssetsPath;
            if (basePath.Contains("://") || basePath.Contains(":///"))
            {
                return $"{basePath.TrimEnd('/')}/Videos/{fileName}";
            }

            return Path.Combine(basePath, "Videos", fileName);
        }

        private void OnVideoPrepared(VideoPlayer preparedPlayer)
        {
            SetStatus(string.Empty);
            bool autoPlay = AppBootstrap.Instance == null
                || AppBootstrap.Instance.Settings == null
                || AppBootstrap.Instance.Settings.autoPlayVideo;

            if (autoPlay)
            {
                preparedPlayer.Play();
            }

            RefreshPlayPauseLabel();
        }

        private void OnVideoError(VideoPlayer source, string message)
        {
            SetStatus($"Erro ao carregar video: {message}");
            RefreshPlayPauseLabel();
        }

        private void EnsureDependencies()
        {
            if (videoPlayer == null)
            {
                videoPlayer = GetComponent<VideoPlayer>();
            }

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }

            if (sphereSetup == null)
            {
                sphereSetup = FindObjectOfType<Video360SphereSetup>();
            }

            if (videoPlayer != null)
            {
                videoPlayer.playOnAwake = false;
                videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
                if (audioSource != null)
                {
                    videoPlayer.SetTargetAudioSource(0, audioSource);
                }

                if (sphereSetup != null)
                {
                    videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
                    videoPlayer.targetMaterialRenderer = sphereSetup.SphereRenderer;
                    videoPlayer.targetMaterialProperty = "_MainTex";
                }
            }
        }

        private void BindVolume()
        {
            float defaultVolume = AppBootstrap.Instance?.Settings != null
                ? AppBootstrap.Instance.Settings.defaultVolume
                : 0.85f;

            if (audioSource != null)
            {
                audioSource.volume = defaultVolume;
            }

            if (volumeSlider != null)
            {
                volumeSlider.value = defaultVolume;
                volumeSlider.onValueChanged.AddListener(SetVolume);
            }
        }

        private void RefreshPlayPauseLabel()
        {
            if (playPauseLabel == null || videoPlayer == null)
            {
                return;
            }

            playPauseLabel.text = videoPlayer.isPlaying ? "Pause" : "Play";
        }

        private void SetStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
        }
    }
}
