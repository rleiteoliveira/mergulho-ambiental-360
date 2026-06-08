using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace MergulhoAmbiental360.Config
{
    public class VideoCatalogLoader : MonoBehaviour
    {
        [Serializable]
        private class CatalogDto
        {
            public List<VideoItemDto> items = new List<VideoItemDto>();
        }

        [Serializable]
        private class VideoItemDto
        {
            public string id;
            public string title;
            public string description;
            public string category;
            public string videoSourceType;
            public string localFileName;
            public string streamingUrl;
            public string thumbnail;
            public string durationLabel;
            public bool isEnabled = true;
        }

        public VideoCatalog LoadCatalog(AppSettings settings)
        {
            if (settings == null)
            {
                Debug.LogWarning("AppSettings not assigned. Loading built-in mock catalog.");
                return CreateCatalogFromJson(ReadStreamingAssetsJson("video_catalog_mock.json"));
            }

            string json = ReadStreamingAssetsJson(settings.streamingAssetsCatalogFileName);
            if (!string.IsNullOrWhiteSpace(json))
            {
                return CreateCatalogFromJson(json);
            }

            if (settings.fallbackCatalog != null)
            {
                return settings.fallbackCatalog;
            }

            Debug.LogWarning("No catalog found. Creating an empty catalog.");
            return ScriptableObject.CreateInstance<VideoCatalog>();
        }

        public IEnumerator LoadCatalogRoutine(AppSettings settings, Action<VideoCatalog> onLoaded)
        {
            string catalogFileName = settings != null ? settings.streamingAssetsCatalogFileName : "video_catalog_mock.json";
            string json = null;
            yield return ReadStreamingAssetsJsonRoutine(catalogFileName, value => json = value);

            if (!string.IsNullOrWhiteSpace(json))
            {
                onLoaded?.Invoke(CreateCatalogFromJson(json));
                yield break;
            }

            if (settings != null && settings.fallbackCatalog != null)
            {
                onLoaded?.Invoke(settings.fallbackCatalog);
                yield break;
            }

            Debug.LogWarning("No catalog found. Creating an empty catalog.");
            onLoaded?.Invoke(ScriptableObject.CreateInstance<VideoCatalog>());
        }

        private string ReadStreamingAssetsJson(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return null;
            }

            string fullPath = BuildStreamingAssetsPath(fileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning($"Catalog file not found: {fullPath}");
                return null;
            }

            return File.ReadAllText(fullPath);
        }

        private IEnumerator ReadStreamingAssetsJsonRoutine(string fileName, Action<string> onLoaded)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                onLoaded?.Invoke(null);
                yield break;
            }

            string fullPath = BuildStreamingAssetsPath(fileName);
            if (fullPath.Contains("://") || fullPath.Contains(":///"))
            {
                using (UnityWebRequest request = UnityWebRequest.Get(fullPath))
                {
                    yield return request.SendWebRequest();

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogWarning($"Catalog file not loaded: {fullPath}. {request.error}");
                        onLoaded?.Invoke(null);
                        yield break;
                    }

                    onLoaded?.Invoke(request.downloadHandler.text);
                }

                yield break;
            }

            if (!File.Exists(fullPath))
            {
                Debug.LogWarning($"Catalog file not found: {fullPath}");
                onLoaded?.Invoke(null);
                yield break;
            }

            onLoaded?.Invoke(File.ReadAllText(fullPath));
        }

        private string BuildStreamingAssetsPath(string fileName)
        {
            string basePath = Application.streamingAssetsPath;
            if (basePath.Contains("://") || basePath.Contains(":///"))
            {
                return $"{basePath.TrimEnd('/')}/{fileName}";
            }

            return Path.Combine(basePath, fileName);
        }

        private VideoCatalog CreateCatalogFromJson(string json)
        {
            VideoCatalog catalog = ScriptableObject.CreateInstance<VideoCatalog>();
            if (string.IsNullOrWhiteSpace(json))
            {
                return catalog;
            }

            CatalogDto dto = JsonUtility.FromJson<CatalogDto>(json);
            if (dto?.items == null)
            {
                return catalog;
            }

            foreach (VideoItemDto source in dto.items)
            {
                catalog.items.Add(ToVideoItem(source));
            }

            return catalog;
        }

        private VideoItem ToVideoItem(VideoItemDto source)
        {
            VideoSourceType sourceType = VideoSourceType.Placeholder;
            if (!string.IsNullOrWhiteSpace(source.videoSourceType))
            {
                Enum.TryParse(source.videoSourceType, true, out sourceType);
            }

            return new VideoItem
            {
                id = source.id,
                title = source.title,
                description = source.description,
                category = source.category,
                videoSourceType = sourceType,
                localFileName = source.localFileName,
                streamingUrl = source.streamingUrl,
                thumbnail = source.thumbnail,
                durationLabel = source.durationLabel,
                isEnabled = source.isEnabled
            };
        }
    }
}
