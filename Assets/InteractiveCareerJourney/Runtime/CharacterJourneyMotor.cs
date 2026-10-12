using UnityEngine;

namespace InteractiveCareerJourney
{
    /// <summary>Moves the prototype character along the X axis and gives its limbs a simple walk cycle.</summary>
    public sealed class CharacterJourneyMotor : MonoBehaviour
    {
        [SerializeField] private float walkingSpeed = 2.2f;
        [SerializeField] private float stopAtX = 5.5f;
        [SerializeField] private Transform[] gaitPivots;
        [SerializeField] private float[] gaitPhaseOffsets;
        [SerializeField] private float gaitAngle = 22f;

        private float startAtX;
        private float firstStopAtX;
        private bool isPaused;
        private bool isWalking;

        public bool IsPaused => isPaused;
        public bool IsWalking => isWalking && !isPaused;
        public bool HasReachedMilestone => transform.position.x >= stopAtX - 0.01f;

        public void SetNextMilestone(float positionX)
        {
            stopAtX = positionX;
            isPaused = false;
            isWalking = !HasReachedMilestone;
        }

        private void Start()
        {
            startAtX = transform.position.x;
            firstStopAtX = stopAtX;
            isWalking = true;
        }

        private void Update()
        {
            if (isPaused || !isWalking)
            {
                ResetGait();
                return;
            }

            Vector3 position = transform.position;
            position.x = Mathf.MoveTowards(position.x, stopAtX, walkingSpeed * Time.deltaTime);
            transform.position = position;

            if (Mathf.Approximately(position.x, stopAtX))
            {
                isWalking = false;
                ResetGait();
                return;
            }

            AnimateGait();
        }

        public void Pause()
        {
            isPaused = true;
            ResetGait();
        }

        public void Resume()
        {
            isPaused = false;
            isWalking = !HasReachedMilestone;
        }

        public void RestartJourney()
        {
            Vector3 position = transform.position;
            position.x = startAtX;
            transform.position = position;
            stopAtX = firstStopAtX;
            isPaused = false;
            isWalking = true;
        }

        private void AnimateGait()
        {
            if (gaitPivots == null)
                return;

            float phase = Time.time * walkingSpeed * 2.8f;
            for (int i = 0; i < gaitPivots.Length; i++)
            {
                Transform pivot = gaitPivots[i];
                if (pivot == null)
                    continue;

                float offset = gaitPhaseOffsets != null && i < gaitPhaseOffsets.Length
                    ? gaitPhaseOffsets[i]
                    : 0f;
                float angle = Mathf.Sin(phase + offset) * gaitAngle;
                pivot.localRotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        private void ResetGait()
        {
            if (gaitPivots == null)
                return;

            foreach (Transform pivot in gaitPivots)
            {
                if (pivot != null)
                    pivot.localRotation = Quaternion.identity;
            }
        }
    }
}
