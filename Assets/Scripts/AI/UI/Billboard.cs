using UnityEngine;

namespace Game.AI.UI
{
    /// <summary>
    /// Makes the GameObject (typically a UI Canvas) always face the camera.
    /// Useful for world-space health bars.
    /// </summary>
    public class Billboard : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            if (_mainCamera == null) return;

            // Make the object face the camera, maintaining the correct upward orientation.
            transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward,
                             _mainCamera.transform.rotation * Vector3.up);
        }
    }
}
