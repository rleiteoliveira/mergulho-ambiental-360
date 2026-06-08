using System.Collections.Generic;
using UnityEngine;

namespace MergulhoAmbiental360.Config
{
    [CreateAssetMenu(fileName = "VideoCatalog", menuName = "Mergulho Ambiental 360/Video Catalog")]
    public class VideoCatalog : ScriptableObject
    {
        public List<VideoItem> items = new List<VideoItem>();

        public VideoItem FindById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            return items.Find(item => item != null && item.id == id);
        }

        public VideoItem FindNextEnabled(string currentId)
        {
            List<VideoItem> enabledItems = items.FindAll(item => item != null && item.isEnabled);
            if (enabledItems.Count == 0)
            {
                return null;
            }

            int currentIndex = enabledItems.FindIndex(item => item.id == currentId);
            int nextIndex = currentIndex < 0 ? 0 : (currentIndex + 1) % enabledItems.Count;
            return enabledItems[nextIndex];
        }
    }
}
