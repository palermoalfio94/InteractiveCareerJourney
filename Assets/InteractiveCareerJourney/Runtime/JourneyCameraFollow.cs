using UnityEngine;

namespace InteractiveCareerJourney
{
    /// <summary>Keeps a clean side-on orthographic view centered on the moving character.</summary>
    [RequireComponent(typeof(Camera))]
    public sealed class JourneyCameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 4.2f, -16f);
        [SerializeField, Min(0.01f)] private float smoothTime = 0.18f;
        [SerializeField, Range(3f, 12f)] private float orthographicSize = 5.4f;

        private Camera sceneCamera;
        private float velocityX;

        private void Awake()
        {
            sceneCamera = GetComponent<Camera>();
            sceneCamera.orthographic = true;
            sceneCamera.orthographicSize = orthographicSize;
        }

        private void LateUpdate()
        {
            if (target == null)
                return;

            Vector3 position = transform.position;
            position.x = Mathf.SmoothDamp(position.x, target.position.x + offset.x, ref velocityX, smoothTime);
            position.y = offset.y;
            position.z = offset.z;
            transform.position = position;
        }

        public void SetTarget(Transform followTarget)
        {
            target = followTarget;
        }
    }
}
