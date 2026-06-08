using UnityEngine;

namespace MergulhoAmbiental360.XR
{
    public class XRInputAdapter : MonoBehaviour
    {
        public Camera fallbackCamera;
        public LayerMask uiLayerMask = ~0;
        public float maxRayDistance = 12f;

        public Ray GetPointerRay()
        {
#if UNITY_EDITOR
            if (fallbackCamera == null)
            {
                fallbackCamera = Camera.main;
            }

            if (fallbackCamera != null)
            {
                return fallbackCamera.ScreenPointToRay(Input.mousePosition);
            }
#endif
            Transform pointer = transform;
            return new Ray(pointer.position, pointer.forward);
        }

        public bool WasPrimaryActionPressed()
        {
#if UNITY_EDITOR
            return Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return);
#else
            return Input.GetButtonDown("Fire1");
#endif
        }
    }
}
