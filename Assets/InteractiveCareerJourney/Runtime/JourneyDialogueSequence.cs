using UnityEngine;

namespace InteractiveCareerJourney
{
    /// <summary>Runs the school conversations and time jump in Alfio's journey.</summary>
    public sealed class JourneyDialogueSequence : MonoBehaviour
    {
        private enum SequenceState
        {
            WalkingToMiddleSchool,
            Question,
            Answer,
            WalkingToHighSchool,
            TimeJump,
            Reflection,
            WalkingToUniversity,
            AtUniversity
        }

        [SerializeField] private CharacterJourneyMotor alfio;
        [SerializeField] private GameObject mentor;
        [SerializeField] private JourneyContentCatalog contentCatalog;
        [SerializeField] private float highSchoolStopX = 23f;
        [SerializeField] private float universityStopX = 47f;

        private SequenceState state = SequenceState.WalkingToMiddleSchool;
        private float timeJumpEndsAt;
        private GUIStyle bubbleStyle;
        private GUIStyle buttonStyle;
        private GUIStyle timeJumpStyle;

        private void Awake()
        {
            if (mentor != null)
                mentor.SetActive(false);
        }

        private void Update()
        {
            if (state == SequenceState.WalkingToMiddleSchool && !alfio.IsWalking && !alfio.IsPaused)
            {
                state = SequenceState.Question;
                if (mentor != null)
                    mentor.SetActive(true);
            }
            else if (state == SequenceState.WalkingToHighSchool && !alfio.IsWalking && !alfio.IsPaused)
            {
                state = SequenceState.TimeJump;
                float duration = contentCatalog != null ? contentCatalog.yearsLaterDuration : 3f;
                timeJumpEndsAt = Time.time + Mathf.Max(0.1f, duration);
            }
            else if (state == SequenceState.TimeJump && Time.time >= timeJumpEndsAt)
                state = SequenceState.Reflection;
            else if (state == SequenceState.WalkingToUniversity && !alfio.IsWalking && !alfio.IsPaused)
                state = SequenceState.AtUniversity;
        }

        private void OnGUI()
        {
            if (state == SequenceState.TimeJump)
            {
                DrawTimeJump();
                return;
            }

            if (state != SequenceState.Question && state != SequenceState.Answer && state != SequenceState.Reflection)
                return;

            EnsureStyles();
            float width = Mathf.Min(780f, Screen.width - 40f);
            float height = state == SequenceState.Reflection ? 194f : 108f;
            Rect bubble = new Rect((Screen.width - width) * 0.5f, Screen.height - height - 24f, width, height);
            GUI.Box(bubble, GUIContent.none, bubbleStyle);

            string message = state == SequenceState.Question
                ? (contentCatalog != null ? contentCatalog.middleSchoolQuestion : "Hey, you seem a smart kid! Do you wanna learn Computer Science?")
                : state == SequenceState.Answer
                    ? (contentCatalog != null ? contentCatalog.middleSchoolAnswer : "Why not?")
                    : (contentCatalog != null ? contentCatalog.highSchoolReflection : "WOW! I really enjoyed Computer Science! Now I can program in C++, C and C#, and I also know how to design and implement a database! I also have the basics of systems and networking! This is fantastic!");
            float messageHeight = state == SequenceState.Reflection ? height - 62f : 48f;
            GUI.Label(new Rect(bubble.x + 22f, bubble.y + 12f, bubble.width - 44f, messageHeight), message, bubbleStyle);

            string buttonText = state == SequenceState.Question
                ? (contentCatalog != null ? contentCatalog.middleSchoolReplyButton : "Reply")
                : (contentCatalog != null ? contentCatalog.continueButton : "Continue");
            Rect button = new Rect(bubble.xMax - 142f, bubble.yMax - 42f, 120f, 30f);
            if (GUI.Button(button, buttonText, buttonStyle))
                Advance();
        }

        private void Advance()
        {
            if (state == SequenceState.Question)
                state = SequenceState.Answer;
            else if (state == SequenceState.Answer)
            {
                state = SequenceState.WalkingToHighSchool;
                alfio.SetNextMilestone(highSchoolStopX);
            }
            else if (state == SequenceState.Reflection)
            {
                state = SequenceState.WalkingToUniversity;
                alfio.SetNextMilestone(universityStopX);
            }
        }

        public void RestartSequence()
        {
            alfio.RestartJourney();
            if (mentor != null)
                mentor.SetActive(false);
            timeJumpEndsAt = 0f;
            state = SequenceState.WalkingToMiddleSchool;
        }

        private void DrawTimeJump()
        {
            EnsureStyles();
            Rect overlay = new Rect(0f, 0f, Screen.width, Screen.height);
            GUI.color = new Color(0.04f, 0.06f, 0.1f, 0.76f);
            GUI.DrawTexture(overlay, Texture2D.whiteTexture);
            GUI.color = Color.white;
            if (timeJumpStyle == null)
            {
                timeJumpStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = Mathf.Clamp(Screen.height / 12, 28, 72),
                    fontStyle = FontStyle.Bold,
                    normal = { textColor = Color.white }
                };
            }
            GUI.Label(overlay, contentCatalog != null ? contentCatalog.yearsLaterCaption : "5 YEARS LATER", timeJumpStyle);
        }

        private void EnsureStyles()
        {
            if (bubbleStyle != null)
                return;

            bubbleStyle = new GUIStyle(GUI.skin.box)
            {
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
                fontSize = 18,
                normal = { textColor = new Color(0.1f, 0.13f, 0.16f), background = MakeTexture(new Color(1f, 0.98f, 0.9f, 0.97f)) }
            };
            buttonStyle = new GUIStyle(GUI.skin.button) { fontSize = 16, fontStyle = FontStyle.Bold };
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
