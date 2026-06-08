using System.Collections.Generic;
using MergulhoAmbiental360.App;
using MergulhoAmbiental360.Config;
using MergulhoAmbiental360.UI;
using MergulhoAmbiental360.Video360;
using MergulhoAmbiental360.XR;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

namespace MergulhoAmbiental360.EditorTools
{
    public static class SceneBootstrapGenerator
    {
        private const string ScenesPath = "Assets/_Project/Scenes";
        private const string ScriptableObjectsPath = "Assets/_Project/ScriptableObjects";
        private const string AppSettingsPath = ScriptableObjectsPath + "/AppSettings.asset";
        private static readonly Color BackgroundColor = new Color(0.02f, 0.06f, 0.09f, 1f);
        private static readonly Color PanelColor = new Color(0.04f, 0.14f, 0.17f, 0.96f);
        private static readonly Color ButtonColor = new Color(0.02f, 0.48f, 0.56f, 1f);
        private static readonly Color AccentColor = new Color(1f, 0.82f, 0.18f, 1f);

        [MenuItem("Tools/Mergulho Ambiental 360/Create or Refresh Base Scenes")]
        public static void CreateOrRefreshBaseScenes()
        {
            EnsureFolders();
            AppSettings settings = EnsureAppSettings();
            CreateAppStartScene(settings);
            CreateMainMenuScene();
            CreateVideoPlayerScene();
            ConfigureBuildSettings();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog(
                "Mergulho Ambiental 360",
                "Base scenes created or refreshed. Open Assets/_Project/Scenes/AppStart.unity to run the PoC.",
                "OK");
        }

        private static void EnsureFolders()
        {
            CreateFolderIfMissing("Assets", "_Project");
            CreateFolderIfMissing("Assets/_Project", "Scenes");
            CreateFolderIfMissing("Assets/_Project", "ScriptableObjects");
        }

        private static void CreateFolderIfMissing(string parent, string folder)
        {
            string path = $"{parent}/{folder}";
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder(parent, folder);
            }
        }

        private static AppSettings EnsureAppSettings()
        {
            AppSettings settings = AssetDatabase.LoadAssetAtPath<AppSettings>(AppSettingsPath);
            if (settings != null)
            {
                return settings;
            }

            settings = ScriptableObject.CreateInstance<AppSettings>();
            AssetDatabase.CreateAsset(settings, AppSettingsPath);
            EditorUtility.SetDirty(settings);
            return settings;
        }

        private static void CreateAppStartScene(AppSettings settings)
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "AppStart";

            GameObject bootstrap = new GameObject("AppBootstrap");
            AppBootstrap appBootstrap = bootstrap.AddComponent<AppBootstrap>();
            bootstrap.AddComponent<VideoCatalogLoader>();
            appBootstrap.settings = settings;
            appBootstrap.loadMainMenuOnStart = true;

            CreateCamera("BootstrapCamera", new Vector3(0f, 1.6f, -4f), Quaternion.identity);
            SaveScene(scene, "AppStart");
        }

        private static void CreateMainMenuScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "MainMenu";

            Camera camera = CreateCamera("MainCamera", new Vector3(0f, 1.6f, 0f), Quaternion.identity);
            RenderSettings.ambientLight = Color.white;
            CreateEventSystem();

            Canvas canvas = CreateWorldCanvas("MainMenuCanvas", new Vector3(0f, 1.45f, 3.2f), new Vector2(1400f, 900f), camera);
            GameObject panel = CreatePanel("MenuPanel", canvas.transform, new Vector2(1320f, 820f), PanelColor);
            RectTransform panelRect = panel.GetComponent<RectTransform>();

            CreateText("Title", panelRect, "Mergulho Ambiental 360", 64, FontStyle.Bold, new Vector2(0f, 330f), new Vector2(1180f, 90f), Color.white);
            CreateText("Subtitle", panelRect, "Escolha uma experiencia", 34, FontStyle.Normal, new Vector2(0f, 270f), new Vector2(1180f, 60f), AccentColor);

            GameObject grid = new GameObject("CardsGrid", typeof(RectTransform), typeof(GridLayoutGroup));
            grid.transform.SetParent(panelRect, false);
            RectTransform gridRect = grid.GetComponent<RectTransform>();
            gridRect.sizeDelta = new Vector2(1180f, 430f);
            gridRect.anchoredPosition = new Vector2(0f, 20f);
            GridLayoutGroup layout = grid.GetComponent<GridLayoutGroup>();
            layout.cellSize = new Vector2(560f, 190f);
            layout.spacing = new Vector2(32f, 28f);
            layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            layout.constraintCount = 2;

            VideoCardView cardPrefab = CreateVideoCardTemplate(grid.transform);

            GameObject footer = new GameObject("Footer", typeof(RectTransform), typeof(HorizontalLayoutGroup));
            footer.transform.SetParent(panelRect, false);
            RectTransform footerRect = footer.GetComponent<RectTransform>();
            footerRect.sizeDelta = new Vector2(1180f, 90f);
            footerRect.anchoredPosition = new Vector2(0f, -330f);
            HorizontalLayoutGroup footerLayout = footer.GetComponent<HorizontalLayoutGroup>();
            footerLayout.spacing = 28f;
            footerLayout.childAlignment = TextAnchor.MiddleCenter;
            footerLayout.childControlWidth = false;
            footerLayout.childControlHeight = false;

            VideoSelectionController selectionController = panel.AddComponent<VideoSelectionController>();
            selectionController.cardParent = grid.transform;
            selectionController.cardPrefab = cardPrefab;

            Button settingsButton = CreateButton("SettingsButton", footer.transform, "Configuracoes", new Vector2(300f, 72f), ButtonColor);
            Button quitButton = CreateButton("QuitButton", footer.transform, "Sair", new Vector2(220f, 72f), new Color(0.5f, 0.08f, 0.08f, 1f));
            UnityEventTools.AddPersistentListener(settingsButton.onClick, selectionController.OpenSettings);
            UnityEventTools.AddPersistentListener(quitButton.onClick, selectionController.QuitApp);

            GameObject input = new GameObject("XRInputAdapter");
            XRInputAdapter adapter = input.AddComponent<XRInputAdapter>();
            adapter.fallbackCamera = camera;

            SaveScene(scene, "MainMenu");
        }

        private static VideoCardView CreateVideoCardTemplate(Transform parent)
        {
            GameObject card = CreatePanel("VideoCardTemplate", parent, new Vector2(560f, 190f), new Color(0.06f, 0.24f, 0.28f, 1f));
            VideoCardView view = card.AddComponent<VideoCardView>();
            Button button = card.AddComponent<Button>();
            button.targetGraphic = card.GetComponent<Image>();

            RectTransform rect = card.GetComponent<RectTransform>();
            Text title = CreateText("Title", rect, "Titulo", 34, FontStyle.Bold, new Vector2(0f, 52f), new Vector2(490f, 50f), Color.white);
            Text description = CreateText("Description", rect, "Descricao curta", 22, FontStyle.Normal, new Vector2(0f, -10f), new Vector2(490f, 58f), Color.white);
            Text duration = CreateText("Duration", rect, "3 min", 20, FontStyle.Bold, new Vector2(-205f, -72f), new Vector2(130f, 36f), AccentColor);
            Text category = CreateText("Category", rect, "Categoria", 20, FontStyle.Normal, new Vector2(132f, -72f), new Vector2(220f, 36f), AccentColor);

            GameObject badge = CreatePanel("PlaceholderBadge", rect, new Vector2(150f, 34f), new Color(0.74f, 0.36f, 0.04f, 1f));
            badge.GetComponent<RectTransform>().anchoredPosition = new Vector2(185f, 72f);
            CreateText("BadgeText", badge.transform, "Placeholder", 16, FontStyle.Bold, Vector2.zero, new Vector2(140f, 28f), Color.white);

            view.titleText = title;
            view.descriptionText = description;
            view.durationText = duration;
            view.categoryText = category;
            view.selectButton = button;
            view.placeholderBadge = badge;
            card.SetActive(false);
            return view;
        }

        private static void CreateVideoPlayerScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "Video360Player";

            Camera camera = CreateCamera("MainCamera", new Vector3(0f, 1.6f, 0f), Quaternion.identity);
            CreateEventSystem();

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "Video360Sphere";
            Object.DestroyImmediate(sphere.GetComponent<Collider>());
            Video360SphereSetup sphereSetup = sphere.AddComponent<Video360SphereSetup>();
            sphereSetup.radius = 24f;

            GameObject playerRoot = new GameObject("Video360Player");
            VideoPlayer videoPlayer = playerRoot.AddComponent<VideoPlayer>();
            AudioSource audioSource = playerRoot.AddComponent<AudioSource>();
            Video360PlayerController controller = playerRoot.AddComponent<Video360PlayerController>();
            controller.videoPlayer = videoPlayer;
            controller.audioSource = audioSource;
            controller.sphereSetup = sphereSetup;

            Canvas canvas = CreateWorldCanvas("PlayerControlsCanvas", new Vector3(0f, 1.25f, 2.65f), new Vector2(1200f, 560f), camera);
            CanvasGroup group = canvas.gameObject.AddComponent<CanvasGroup>();
            UIStateController uiState = canvas.gameObject.AddComponent<UIStateController>();
            uiState.controlsGroup = group;
            controller.uiStateController = uiState;

            GameObject panel = CreatePanel("ControlsPanel", canvas.transform, new Vector2(1120f, 500f), new Color(0.02f, 0.08f, 0.1f, 0.88f));
            RectTransform panelRect = panel.GetComponent<RectTransform>();

            controller.titleText = CreateText("VideoTitle", panelRect, "Video 360", 44, FontStyle.Bold, new Vector2(0f, 180f), new Vector2(1000f, 64f), Color.white);
            controller.statusText = CreateText("StatusText", panelRect, "Carregando...", 24, FontStyle.Normal, new Vector2(0f, 118f), new Vector2(1000f, 44f), AccentColor);

            GameObject buttons = new GameObject("Buttons", typeof(RectTransform), typeof(HorizontalLayoutGroup));
            buttons.transform.SetParent(panelRect, false);
            RectTransform buttonsRect = buttons.GetComponent<RectTransform>();
            buttonsRect.sizeDelta = new Vector2(1040f, 100f);
            buttonsRect.anchoredPosition = new Vector2(0f, 10f);
            HorizontalLayoutGroup buttonLayout = buttons.GetComponent<HorizontalLayoutGroup>();
            buttonLayout.spacing = 22f;
            buttonLayout.childAlignment = TextAnchor.MiddleCenter;
            buttonLayout.childControlWidth = false;
            buttonLayout.childControlHeight = false;

            Button back = CreateButton("BackButton", buttons.transform, "Menu", new Vector2(160f, 78f), new Color(0.18f, 0.3f, 0.34f, 1f));
            Button playPause = CreateButton("PlayPauseButton", buttons.transform, "Play", new Vector2(180f, 78f), ButtonColor);
            Button restart = CreateButton("RestartButton", buttons.transform, "Reiniciar", new Vector2(210f, 78f), ButtonColor);
            Button next = CreateButton("NextButton", buttons.transform, "Proximo", new Vector2(190f, 78f), ButtonColor);
            controller.playPauseLabel = playPause.GetComponentInChildren<Text>();

            UnityEventTools.AddPersistentListener(back.onClick, controller.BackToMenu);
            UnityEventTools.AddPersistentListener(playPause.onClick, controller.TogglePlayPause);
            UnityEventTools.AddPersistentListener(restart.onClick, controller.RestartVideo);
            UnityEventTools.AddPersistentListener(next.onClick, controller.PlayNextVideo);

            Slider volume = CreateSlider("VolumeSlider", panelRect, new Vector2(0f, -135f), new Vector2(520f, 42f));
            controller.volumeSlider = volume;
            CreateText("VolumeLabel", panelRect, "Volume", 22, FontStyle.Bold, new Vector2(-360f, -135f), new Vector2(160f, 42f), Color.white);

            GameObject input = new GameObject("XRInputAdapter");
            XRInputAdapter adapter = input.AddComponent<XRInputAdapter>();
            adapter.fallbackCamera = camera;

            SaveScene(scene, "Video360Player");
        }

        private static Camera CreateCamera(string name, Vector3 position, Quaternion rotation)
        {
            GameObject cameraObject = new GameObject(name, typeof(Camera), typeof(AudioListener));
            cameraObject.tag = "MainCamera";
            cameraObject.transform.SetPositionAndRotation(position, rotation);
            Camera camera = cameraObject.GetComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = BackgroundColor;
            camera.nearClipPlane = 0.02f;
            return camera;
        }

        private static Canvas CreateWorldCanvas(string name, Vector3 position, Vector2 size, Camera camera)
        {
            GameObject canvasObject = new GameObject(name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasObject.transform.position = position;
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = camera;
            RectTransform rect = canvas.GetComponent<RectTransform>();
            rect.sizeDelta = size;
            rect.localScale = Vector3.one * 0.0025f;
            return canvas;
        }

        private static GameObject CreatePanel(string name, Transform parent, Vector2 size, Color color)
        {
            GameObject panel = new GameObject(name, typeof(RectTransform), typeof(Image));
            panel.transform.SetParent(parent, false);
            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.sizeDelta = size;
            Image image = panel.GetComponent<Image>();
            image.color = color;
            return panel;
        }

        private static Text CreateText(string name, Transform parent, string value, int fontSize, FontStyle style, Vector2 position, Vector2 size, Color color)
        {
            GameObject textObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            textObject.transform.SetParent(parent, false);
            RectTransform rect = textObject.GetComponent<RectTransform>();
            rect.sizeDelta = size;
            rect.anchoredPosition = position;
            Text text = textObject.GetComponent<Text>();
            text.text = value;
            text.alignment = TextAnchor.MiddleCenter;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = fontSize;
            text.fontStyle = style;
            text.color = color;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            return text;
        }

        private static Button CreateButton(string name, Transform parent, string label, Vector2 size, Color color)
        {
            GameObject buttonObject = CreatePanel(name, parent, size, color);
            Button button = buttonObject.AddComponent<Button>();
            button.targetGraphic = buttonObject.GetComponent<Image>();
            Text text = CreateText("Label", buttonObject.transform, label, 28, FontStyle.Bold, Vector2.zero, size, Color.white);
            text.raycastTarget = false;
            return button;
        }

        private static Slider CreateSlider(string name, Transform parent, Vector2 position, Vector2 size)
        {
            GameObject sliderObject = new GameObject(name, typeof(RectTransform), typeof(Slider));
            sliderObject.transform.SetParent(parent, false);
            RectTransform rect = sliderObject.GetComponent<RectTransform>();
            rect.sizeDelta = size;
            rect.anchoredPosition = position;

            GameObject background = CreatePanel("Background", sliderObject.transform, size, new Color(0.14f, 0.22f, 0.24f, 1f));
            GameObject fillArea = new GameObject("Fill Area", typeof(RectTransform));
            fillArea.transform.SetParent(sliderObject.transform, false);
            RectTransform fillAreaRect = fillArea.GetComponent<RectTransform>();
            fillAreaRect.anchorMin = new Vector2(0f, 0.25f);
            fillAreaRect.anchorMax = new Vector2(1f, 0.75f);
            fillAreaRect.offsetMin = new Vector2(12f, 0f);
            fillAreaRect.offsetMax = new Vector2(-12f, 0f);

            GameObject fill = CreatePanel("Fill", fillArea.transform, size, ButtonColor);
            RectTransform fillRect = fill.GetComponent<RectTransform>();
            fillRect.anchorMin = Vector2.zero;
            fillRect.anchorMax = Vector2.one;
            fillRect.offsetMin = Vector2.zero;
            fillRect.offsetMax = Vector2.zero;

            Slider slider = sliderObject.GetComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = 0.85f;
            slider.targetGraphic = background.GetComponent<Image>();
            slider.fillRect = fillRect;
            return slider;
        }

        private static void CreateEventSystem()
        {
            GameObject eventSystem = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            eventSystem.GetComponent<StandaloneInputModule>().forceModuleActive = true;
        }

        private static void SaveScene(Scene scene, string sceneName)
        {
            EditorSceneManager.SaveScene(scene, $"{ScenesPath}/{sceneName}.unity");
        }

        private static void ConfigureBuildSettings()
        {
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>
            {
                new EditorBuildSettingsScene($"{ScenesPath}/AppStart.unity", true),
                new EditorBuildSettingsScene($"{ScenesPath}/MainMenu.unity", true),
                new EditorBuildSettingsScene($"{ScenesPath}/Video360Player.unity", true)
            };

            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}
