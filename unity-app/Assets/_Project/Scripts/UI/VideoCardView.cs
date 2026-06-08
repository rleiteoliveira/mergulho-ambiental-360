using System;
using MergulhoAmbiental360.Config;
using UnityEngine;
using UnityEngine.UI;

namespace MergulhoAmbiental360.UI
{
    public class VideoCardView : MonoBehaviour
    {
        public Text titleText;
        public Text descriptionText;
        public Text durationText;
        public Text categoryText;
        public Button selectButton;
        public Image thumbnailImage;
        public GameObject placeholderBadge;

        private VideoItem item;
        private Action<VideoItem> onSelected;

        public void Configure(VideoItem videoItem, Action<VideoItem> selectedCallback)
        {
            item = videoItem;
            onSelected = selectedCallback;

            if (titleText != null)
            {
                titleText.text = item.title;
            }

            if (descriptionText != null)
            {
                descriptionText.text = item.description;
            }

            if (durationText != null)
            {
                durationText.text = item.durationLabel;
            }

            if (categoryText != null)
            {
                categoryText.text = item.category;
            }

            if (placeholderBadge != null)
            {
                placeholderBadge.SetActive(item.sourceType == VideoSourceType.Placeholder);
            }

            if (selectButton != null)
            {
                selectButton.onClick.RemoveListener(HandleClick);
                selectButton.onClick.AddListener(HandleClick);
            }
        }

        private void HandleClick()
        {
            onSelected?.Invoke(item);
        }
    }
}
