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
        [SerializeField] private float elevatedHeight = 7.2f;
        [SerializeField, Range(9f, 35f)] private float elevatedPitch = 24f;
        [SerializeField, Range(-45f, 45f)] private float elevatedYaw = 20f;
        [SerializeField, Range(3f, 12f)] private float elevatedOrthographicSize = 8.2f;
        [SerializeField] private float elevatedHorizontalOffset = -0.5f;
        [SerializeField, Min(0.01f)] private float viewBlendSpeed = 2.2f;

        private Camera sceneCamera;
        private float velocityX;
        private float baseHeight;
        private float basePitch;
        private float baseYaw;
        private bool elevatedView;

        private void Awake()
        {
            sceneCamera = GetComponent<Camera>();
            sceneCamera.orthographic = true;
            sceneCamera.orthographicSize = orthographicSize;
            baseHeight = offset.y;
            basePitch = transform.eulerAngles.x;
            baseYaw = transform.eulerAngles.y;
        }

        private void LateUpdate()
        {
            if (target == null)
                return;

            Vector3 position = transform.position;
            float horizontalOffset = elevatedView ? elevatedHorizontalOffset : offset.x;
            position.x = Mathf.SmoothDamp(position.x, target.position.x + horizontalOffset, ref velocityX, smoothTime);
            float targetHeight = elevatedView ? elevatedHeight : baseHeight;
            position.y = Mathf.Lerp(position.y, targetHeight, Time.deltaTime * viewBlendSpeed);
            position.z = offset.z;
            transform.position = position;
            sceneCamera.orthographicSize = Mathf.Lerp(sceneCamera.orthographicSize,
                elevatedView ? elevatedOrthographicSize : orthographicSize, Time.deltaTime * viewBlendSpeed);
            float pitch = elevatedView ? elevatedPitch : basePitch;
            float yaw = elevatedView ? elevatedYaw : baseYaw;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(pitch, yaw, 0f), Time.deltaTime * viewBlendSpeed);
        }

        public void SetTarget(Transform followTarget)
        {
            target = followTarget;
        }

        public void SetElevatedView(bool enabled)
        {
            elevatedView = enabled;
        }
    }
}
