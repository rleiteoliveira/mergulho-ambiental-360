using UnityEngine;

namespace MergulhoAmbiental360.UI
{
    public class UIStateController : MonoBehaviour
    {
        public CanvasGroup controlsGroup;
        public float autoHideSeconds = 6f;
        public bool autoHide = true;

        private float lastInteractionTime;

        private void OnEnable()
        {
            ShowControls();
        }

        private void Update()
        {
            if (!autoHide || controlsGroup == null)
            {
                return;
            }

            if (Input.anyKeyDown)
            {
                ShowControls();
            }

            if (Time.unscaledTime - lastInteractionTime >= autoHideSeconds)
            {
                SetVisible(false);
            }
        }

        public void ShowControls()
        {
            lastInteractionTime = Time.unscaledTime;
            SetVisible(true);
        }

        public void ToggleControls()
        {
            if (controlsGroup == null)
            {
                return;
            }

            SetVisible(controlsGroup.alpha < 0.5f);
            lastInteractionTime = Time.unscaledTime;
        }

        private void SetVisible(bool visible)
        {
            if (controlsGroup == null)
            {
                return;
            }

            controlsGroup.alpha = visible ? 1f : 0f;
            controlsGroup.interactable = visible;
            controlsGroup.blocksRaycasts = visible;
        }
    }
}
