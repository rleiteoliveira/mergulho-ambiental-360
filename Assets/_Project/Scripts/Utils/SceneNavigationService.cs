using MergulhoAmbiental360.App;
using MergulhoAmbiental360.Config;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MergulhoAmbiental360.Utils
{
    public static class SceneNavigationService
    {
        public static string SelectedVideoId { get; private set; }

        public static void SelectVideo(VideoItem item)
        {
            SelectedVideoId = item?.id;
            LoadVideoPlayer();
        }

        public static VideoItem GetSelectedVideo()
        {
            VideoCatalog catalog = AppBootstrap.Instance != null ? AppBootstrap.Instance.Catalog : null;
            return catalog != null ? catalog.FindById(SelectedVideoId) : null;
        }

        public static void LoadMainMenu()
        {
            string sceneName = AppBootstrap.Instance?.Settings != null
                ? AppBootstrap.Instance.Settings.mainMenuSceneName
                : "MainMenu";

            LoadScene(sceneName);
        }

        public static void LoadVideoPlayer()
        {
            string sceneName = AppBootstrap.Instance?.Settings != null
                ? AppBootstrap.Instance.Settings.videoPlayerSceneName
                : "Video360Player";

            LoadScene(sceneName);
        }

        public static void LoadScene(string sceneName)
        {
            if (string.IsNullOrWhiteSpace(sceneName))
            {
                Debug.LogError("Cannot load an empty scene name.");
                return;
            }

            SceneManager.LoadScene(sceneName);
        }

        public static void Quit()
        {
#if UNITY_EDITOR
            Debug.Log("Quit requested. In the Unity Editor this only logs the action.");
#else
            Application.Quit();
#endif
        }
    }
}
