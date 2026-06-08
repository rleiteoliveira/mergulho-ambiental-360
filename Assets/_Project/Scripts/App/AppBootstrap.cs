using System.Collections;
using MergulhoAmbiental360.Config;
using MergulhoAmbiental360.Utils;
using UnityEngine;

namespace MergulhoAmbiental360.App
{
    public class AppBootstrap : MonoBehaviour
    {
        public static AppBootstrap Instance { get; private set; }

        public AppSettings settings;
        public bool loadMainMenuOnStart = true;

        public VideoCatalog Catalog { get; private set; }
        public AppSettings Settings => settings;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            StartCoroutine(InitializeRoutine());
        }

        private IEnumerator InitializeRoutine()
        {
            yield return LoadCatalogRoutine();

            if (loadMainMenuOnStart)
            {
                string sceneName = settings != null ? settings.mainMenuSceneName : "MainMenu";
                SceneNavigationService.LoadScene(sceneName);
            }
        }

        public void LoadCatalog()
        {
            VideoCatalogLoader loader = GetComponent<VideoCatalogLoader>();
            if (loader == null)
            {
                loader = gameObject.AddComponent<VideoCatalogLoader>();
            }

            Catalog = loader.LoadCatalog(settings);
            Debug.Log($"Loaded video catalog with {Catalog.items.Count} items.");
        }

        private IEnumerator LoadCatalogRoutine()
        {
            VideoCatalogLoader loader = GetComponent<VideoCatalogLoader>();
            if (loader == null)
            {
                loader = gameObject.AddComponent<VideoCatalogLoader>();
            }

            bool isLoaded = false;
            yield return loader.LoadCatalogRoutine(settings, catalog =>
            {
                Catalog = catalog;
                isLoaded = true;
            });

            if (!isLoaded || Catalog == null)
            {
                Catalog = ScriptableObject.CreateInstance<VideoCatalog>();
            }

            Debug.Log($"Loaded video catalog with {Catalog.items.Count} items.");
        }
    }
}
