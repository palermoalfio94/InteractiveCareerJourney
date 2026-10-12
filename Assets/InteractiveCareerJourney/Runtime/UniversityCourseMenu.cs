using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace InteractiveCareerJourney
{
    /// <summary>Click-to-open course and project portfolio panel for the university landmark.</summary>
    public sealed class UniversityCourseMenu : MonoBehaviour
    {
        [SerializeField] private JourneyContentCatalog catalog;
        [SerializeField] private CharacterJourneyMotor journeyMotor;

        private bool isOpen;
        private bool wasAlreadyPaused;
        private int selectedSubjectIndex = -1;
        private Vector2 courseScroll;
        private Vector2 detailScroll;
        private GUIStyle titleStyle;
        private GUIStyle bodyStyle;
        private GUIStyle smallStyle;
        private GUIStyle selectedCourseStyle;

        private void Update()
        {
            if (isOpen)
                return;

            bool pointerPressed = false;
            Vector2 pointerPosition = Vector2.zero;
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
            {
                pointerPressed = Mouse.current.leftButton.wasPressedThisFrame;
                pointerPosition = Mouse.current.position.ReadValue();
            }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            if (!pointerPressed && Input.GetMouseButtonDown(0))
            {
                pointerPressed = true;
                pointerPosition = Input.mousePosition;
            }
#endif
            if (!pointerPressed)
                return;

            Camera sceneCamera = Camera.main;
            if (sceneCamera == null)
                return;

            Ray clickRay = sceneCamera.ScreenPointToRay(pointerPosition);
            if (Physics.Raycast(clickRay, out RaycastHit hit) &&
                (hit.transform == transform || hit.transform.IsChildOf(transform)))
                Open();
        }

        public void Open()
        {
            if (isOpen || catalog == null)
                return;

            isOpen = true;
            wasAlreadyPaused = journeyMotor != null && journeyMotor.IsPaused;
            if (journeyMotor != null)
                journeyMotor.Pause();

            if (catalog.subjects != null && catalog.subjects.Count > 0)
                selectedSubjectIndex = Mathf.Clamp(selectedSubjectIndex, 0, catalog.subjects.Count - 1);
        }

        private void OnGUI()
        {
            if (!isOpen)
                return;

            EnsureStyles();
            float panelWidth = Mathf.Min(1040f, Screen.width - 32f);
            float panelHeight = Mathf.Min(700f, Screen.height - 32f);
            Rect panel = new Rect((Screen.width - panelWidth) * 0.5f, (Screen.height - panelHeight) * 0.5f,
                panelWidth, panelHeight);
            GUI.Box(panel, GUIContent.none, GUI.skin.window);

            GUI.Label(new Rect(panel.x + 24f, panel.y + 18f, panel.width - 100f, 38f),
                catalog.universityTitle, titleStyle);
            if (GUI.Button(new Rect(panel.xMax - 66f, panel.y + 16f, 44f, 34f), "X"))
            {
                Close();
                return;
            }

            GUI.Label(new Rect(panel.x + 24f, panel.y + 58f, panel.width - 48f, 30f),
                catalog.mockContentNotice, smallStyle);

            float contentTop = panel.y + 98f;
            float contentHeight = panel.height - 122f;
            float listWidth = Mathf.Clamp(panel.width * 0.32f, 230f, 310f);
            Rect listRect = new Rect(panel.x + 18f, contentTop, listWidth, contentHeight);
            Rect detailRect = new Rect(listRect.xMax + 16f, contentTop, panel.width - listWidth - 52f, contentHeight);
            GUI.Box(listRect, "COURSES", GUI.skin.window);
            GUI.Box(detailRect, GUIContent.none, GUI.skin.window);

            DrawCourseList(new Rect(listRect.x + 8f, listRect.y + 30f, listRect.width - 16f, listRect.height - 40f));
            DrawCourseDetails(new Rect(detailRect.x + 18f, detailRect.y + 16f, detailRect.width - 36f, detailRect.height - 32f));
        }

        private void DrawCourseList(Rect viewport)
        {
            if (catalog.subjects == null || catalog.subjects.Count == 0)
            {
                GUI.Label(viewport, "Add subjects to the JourneyContentCatalog asset in the Inspector.", bodyStyle);
                return;
            }

            float rowHeight = 42f;
            Rect content = new Rect(0f, 0f, viewport.width - 16f, catalog.subjects.Count * rowHeight);
            courseScroll = GUI.BeginScrollView(viewport, courseScroll, content);
            for (int i = 0; i < catalog.subjects.Count; i++)
            {
                UniversitySubjectEntry subject = catalog.subjects[i];
                string label = subject == null ? $"Course {i + 1}" : subject.title;
                Rect row = new Rect(0f, i * rowHeight, content.width, rowHeight - 4f);
                if (GUI.Button(row, label, i == selectedSubjectIndex ? selectedCourseStyle : GUI.skin.button))
                    selectedSubjectIndex = i;
            }
            GUI.EndScrollView();
        }

        private void DrawCourseDetails(Rect viewport)
        {
            if (selectedSubjectIndex < 0 || catalog.subjects == null || selectedSubjectIndex >= catalog.subjects.Count)
            {
                GUI.Label(viewport, "Select a course to see its description and related projects.", bodyStyle);
                return;
            }

            UniversitySubjectEntry subject = catalog.subjects[selectedSubjectIndex];
            if (subject == null)
                return;

            float contentHeight = 420f;
            if (subject.projects != null)
                contentHeight += subject.projects.Count * 180f;
            Rect content = new Rect(0f, 0f, viewport.width - 20f, contentHeight);
            detailScroll = GUI.BeginScrollView(viewport, detailScroll, content);

            GUI.Label(new Rect(0f, 0f, content.width, 36f), subject.title, titleStyle);
            GUI.Label(new Rect(0f, 44f, content.width, 110f), subject.description, bodyStyle);
            float y = 170f;
            if (subject.projects != null)
            {
                foreach (UniversityProjectEntry project in subject.projects)
                {
                    if (project == null)
                        continue;
                    GUI.Label(new Rect(0f, y, content.width, 30f), project.title, titleStyle);
                    GUI.Label(new Rect(0f, y + 34f, content.width, 86f), project.description, bodyStyle);
                    GUI.Label(new Rect(0f, y + 124f, content.width, 32f), "Technologies: " + project.technologies, smallStyle);
                    y += 176f;
                }
            }
            GUI.EndScrollView();
        }

        private void Close()
        {
            isOpen = false;
            if (journeyMotor != null && !wasAlreadyPaused)
                journeyMotor.Resume();
        }

        private void EnsureStyles()
        {
            if (titleStyle != null)
                return;
            titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 22,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(0.13f, 0.18f, 0.22f) }
            };
            bodyStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                wordWrap = true,
                normal = { textColor = new Color(0.14f, 0.17f, 0.2f) }
            };
            smallStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                wordWrap = true,
                normal = { textColor = new Color(0.32f, 0.36f, 0.39f) }
            };
            selectedCourseStyle = new GUIStyle(GUI.skin.button)
            {
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(0.1f, 0.22f, 0.32f), background = MakeTexture(new Color(0.74f, 0.86f, 0.9f)) },
                hover = { textColor = new Color(0.1f, 0.22f, 0.32f), background = MakeTexture(new Color(0.8f, 0.9f, 0.94f)) }
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
