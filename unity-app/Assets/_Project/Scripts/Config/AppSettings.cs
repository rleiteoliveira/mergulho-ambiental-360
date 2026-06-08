using UnityEngine;

namespace MergulhoAmbiental360.Config
{
    [CreateAssetMenu(fileName = "AppSettings", menuName = "Mergulho Ambiental 360/App Settings")]
    public class AppSettings : ScriptableObject
    {
        [Header("Scenes")]
        public string bootstrapSceneName = "AppStart";
        public string mainMenuSceneName = "MainMenu";
        public string videoPlayerSceneName = "Video360Player";

        [Header("Catalog")]
        public string streamingAssetsCatalogFileName = "video_catalog_mock.json";
        public VideoCatalog fallbackCatalog;

        [Header("Playback")]
        public float defaultVolume = 0.85f;
        public float sphereRadius = 24f;
        public bool autoPlayVideo = true;
    }
}
