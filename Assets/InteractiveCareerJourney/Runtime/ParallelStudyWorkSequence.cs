using UnityEngine;

namespace InteractiveCareerJourney
{
    /// <summary>Stages the parallel university and first game-development job vignette.</summary>
    public sealed class ParallelStudyWorkSequence : MonoBehaviour
    {
        private enum SequenceState { Idle, Parallel, OneYearLater, Exiting, Graduated, Dismissed }

        [SerializeField] private CharacterJourneyMotor universityAlfio;
        [SerializeField] private JourneyCameraFollow cameraFollow;
        [SerializeField] private UniversityCourseMenu universityMenu;
        [SerializeField] private GameObject companyBuilding;
        [SerializeField] private JourneyContentCatalog contentCatalog;
        [SerializeField] private Vector3 companyStopPosition = new Vector3(47f, 0.9f, 7.4f);

        private CharacterJourneyMotor workAlfio;
        private SequenceState state;
        private float stateEndsAt;
        private GUIStyle panelStyle;
        private GUIStyle headingStyle;
        private GUIStyle bodyStyle;
        private GUIStyle bubbleStyle;

        public void BeginSequence()
        {
            if (state != SequenceState.Idle || universityAlfio == null)
                return;

            if (companyBuilding != null)
                companyBuilding.SetActive(true);
            if (universityMenu != null)
                universityMenu.enabled = false;
            if (cameraFollow != null)
                cameraFollow.SetElevatedView(true);

            GameObject clone = Instantiate(universityAlfio.gameObject);
            clone.name = "Alfio - Game Programmer (Parallel Year)";
            Vector3 clonePosition = universityAlfio.transform.position;
            clonePosition.z -= 0.15f;
            clone.transform.position = clonePosition;
            workAlfio = clone.GetComponent<CharacterJourneyMotor>();
            if (workAlfio != null)
                workAlfio.SetNextMilestone(companyStopPosition);

            state = SequenceState.Parallel;
            float duration = contentCatalog != null ? contentCatalog.workStoryDuration : 10f;
            stateEndsAt = Time.time + Mathf.Max(0.1f, duration);
        }

        private void Update()
        {
            if (state == SequenceState.Parallel && Time.time >= stateEndsAt)
            {
                state = SequenceState.OneYearLater;
                float duration = contentCatalog != null ? contentCatalog.oneYearCaptionDuration : 1.8f;
                stateEndsAt = Time.time + Mathf.Max(0.1f, duration);
            }
            else if (state == SequenceState.OneYearLater && Time.time >= stateEndsAt)
            {
                state = SequenceState.Exiting;
                float duration = contentCatalog != null ? contentCatalog.workCloneExitDuration : 0.7f;
                stateEndsAt = Time.time + Mathf.Max(0.1f, duration);
            }
            else if (state == SequenceState.Exiting)
            {
                float duration = contentCatalog != null ? contentCatalog.workCloneExitDuration : 0.7f;
                float progress = 1f - Mathf.Clamp01((stateEndsAt - Time.time) / Mathf.Max(0.1f, duration));
                if (workAlfio != null)
                    workAlfio.transform.localScale = Vector3.one * (1f - progress);
                if (Time.time >= stateEndsAt)
                {
                    if (workAlfio != null)
                        Destroy(workAlfio.gameObject);
                    workAlfio = null;
                    state = SequenceState.Graduated;
                    if (universityMenu != null)
                        universityMenu.enabled = true;
                }
            }
        }

        public void ResetSequence()
        {
            if (workAlfio != null)
                Destroy(workAlfio.gameObject);
            workAlfio = null;
            state = SequenceState.Idle;
            stateEndsAt = 0f;
            if (cameraFollow != null)
                cameraFollow.SetElevatedView(false);
            if (universityMenu != null)
                universityMenu.enabled = true;
            if (companyBuilding != null)
                companyBuilding.SetActive(false);
        }

        private void OnGUI()
        {
            if (state == SequenceState.Idle)
                return;
            EnsureStyles();

            if (state == SequenceState.Parallel || state == SequenceState.Exiting)
            {
                DrawWorldBubble(universityAlfio != null ? universityAlfio.transform : null,
                    contentCatalog != null ? contentCatalog.studyBubble : "STUDYING", 0.6f);
                if (state == SequenceState.Parallel && workAlfio != null)
                {
                    string role = contentCatalog != null ? contentCatalog.workRole : "GAME PROGRAMMER";
                    string tools = contentCatalog != null ? contentCatalog.workTools : "Unity  •  Blender";
                    DrawWorldBubble(workAlfio.transform, role + "\n" + tools, 0.65f);
                }
            }

            if (state == SequenceState.OneYearLater)
                DrawCenterCard(contentCatalog != null ? contentCatalog.oneYearLaterCaption : "ONE YEAR LATER", string.Empty);
            else if (state == SequenceState.Graduated)
                DrawCenterCard(
                    contentCatalog != null ? contentCatalog.graduationResult : "GRADUATED  •  110/110",
                    contentCatalog != null ? contentCatalog.graduationThesis : "Thesis: Semantic Segmentation with Deep Learning",
                    true);
        }

        private void DrawWorldBubble(Transform target, string message, float heightOffset)
        {
            if (target == null || Camera.main == null)
                return;
            Vector3 projected = Camera.main.WorldToScreenPoint(target.position + Vector3.up * heightOffset);
            if (projected.z <= 0f)
                return;
            float width = 220f;
            float height = 68f;
            Rect rect = new Rect(projected.x - width * 0.5f,
                Screen.height - projected.y - height - 4f, width, height);
            GUI.Box(rect, message, bubbleStyle);
        }

        private void DrawCenterCard(string heading, string body, bool canClose = false)
        {
            float width = Mathf.Min(780f, Screen.width - 40f);
            float height = string.IsNullOrEmpty(body) ? 112f : 190f;
            Rect rect = new Rect((Screen.width - width) * 0.5f, (Screen.height - height) * 0.5f, width, height);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUI.Label(new Rect(rect.x + 24f, rect.y + 18f, rect.width - 48f, 62f), heading, headingStyle);
            if (!string.IsNullOrEmpty(body))
                GUI.Label(new Rect(rect.x + 30f, rect.y + 88f, rect.width - 60f, 80f), body, bodyStyle);
            if (canClose)
            {
                Rect closeButton = new Rect(rect.xMax - 132f, rect.yMax - 44f, 108f, 30f);
                if (GUI.Button(closeButton, "CLOSE"))
                    state = SequenceState.Dismissed;
            }
        }

        private void EnsureStyles()
        {
            if (panelStyle != null)
                return;
            panelStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = MakeTexture(new Color(0.08f, 0.13f, 0.19f, 0.94f)) }
            };
            headingStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = Mathf.Clamp(Screen.height / 22, 22, 38),
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(1f, 0.84f, 0.48f) }
            };
            bodyStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
                fontSize = Mathf.Clamp(Screen.height / 42, 15, 23),
                normal = { textColor = Color.white }
            };
            bubbleStyle = new GUIStyle(GUI.skin.box)
            {
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
                fontSize = 15,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(0.1f, 0.13f, 0.16f), background = MakeTexture(new Color(1f, 0.98f, 0.9f, 0.97f)) }
            };
        }

        private static Texture2D MakeTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1) { hideFlags = HideFlags.HideAndDontSave };
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
    }
}
