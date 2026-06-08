using MergulhoAmbiental360.App;
using MergulhoAmbiental360.Config;
using MergulhoAmbiental360.Utils;
using UnityEngine;

namespace MergulhoAmbiental360.UI
{
    public class VideoSelectionController : MonoBehaviour
    {
        public Transform cardParent;
        public VideoCardView cardPrefab;
        public GameObject emptyState;

        private void Start()
        {
            BuildCards();
        }

        public void BuildCards()
        {
            if (cardParent == null || cardPrefab == null)
            {
                Debug.LogWarning("VideoSelectionController needs cardParent and cardPrefab.");
                return;
            }

            foreach (Transform child in cardParent)
            {
                if (child.gameObject != cardPrefab.gameObject)
                {
                    Destroy(child.gameObject);
                }
            }

            VideoCatalog catalog = AppBootstrap.Instance != null ? AppBootstrap.Instance.Catalog : null;
            bool hasAnyItem = false;

            if (catalog != null)
            {
                foreach (VideoItem item in catalog.items)
                {
                    if (item == null || !item.isEnabled)
                    {
                        continue;
                    }

                    VideoCardView card = Instantiate(cardPrefab, cardParent);
                    card.gameObject.SetActive(true);
                    card.Configure(item, OnCardSelected);
                    hasAnyItem = true;
                }
            }

            cardPrefab.gameObject.SetActive(false);
            if (emptyState != null)
            {
                emptyState.SetActive(!hasAnyItem);
            }
        }

        public void OpenSettings()
        {
            Debug.Log("Settings button pressed. Teacher mode/settings screen is planned for a later phase.");
        }

        public void QuitApp()
        {
            SceneNavigationService.Quit();
        }

        private void OnCardSelected(VideoItem item)
        {
            SceneNavigationService.SelectVideo(item);
        }
    }
}
