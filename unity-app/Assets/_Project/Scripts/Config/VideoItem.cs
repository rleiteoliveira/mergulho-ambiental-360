using System;
using UnityEngine;

namespace MergulhoAmbiental360.Config
{
    [Serializable]
    public class VideoItem
    {
        public string id;
        public string title;
        [TextArea(2, 4)] public string description;
        public string category;
        public VideoSourceType sourceType = VideoSourceType.Placeholder;
        public string localFileName;
        public string streamingUrl;
        public string thumbnailName;
        public string durationLabel;
        public bool isEnabled = true;

        public bool HasPlayableSource()
        {
            if (!isEnabled)
            {
                return false;
            }

            return sourceType switch
            {
                VideoSourceType.LocalFile => !string.IsNullOrWhiteSpace(localFileName),
                VideoSourceType.StreamingUrl => !string.IsNullOrWhiteSpace(streamingUrl),
                VideoSourceType.Placeholder => false,
                _ => false
            };
        }
    }
}
