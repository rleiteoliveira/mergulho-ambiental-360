using System;
using UnityEngine;

namespace MergulhoAmbiental360.Config
{
    public enum VideoSourceType
    {
        LocalFile,
        StreamingUrl,
        Placeholder
    }

    [Serializable]
    public class VideoItem
    {
        public string id;
        public string title;
        [TextArea(2, 4)] public string description;
        public string category;
        public VideoSourceType videoSourceType = VideoSourceType.Placeholder;
        public string localFileName;
        public string streamingUrl;
        public string thumbnail;
        public string durationLabel;
        public bool isEnabled = true;

        public bool HasPlayableSource()
        {
            if (!isEnabled)
            {
                return false;
            }

            return videoSourceType switch
            {
                VideoSourceType.LocalFile => !string.IsNullOrWhiteSpace(localFileName),
                VideoSourceType.StreamingUrl => !string.IsNullOrWhiteSpace(streamingUrl),
                VideoSourceType.Placeholder => false,
                _ => false
            };
        }
    }
}
