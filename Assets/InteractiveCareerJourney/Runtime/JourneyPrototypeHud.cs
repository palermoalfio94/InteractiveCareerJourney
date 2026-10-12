using UnityEngine;

namespace InteractiveCareerJourney
{
    /// <summary>Small in-game controls for the first playable graybox.</summary>
    public sealed class JourneyPrototypeHud : MonoBehaviour
    {
        [SerializeField] private CharacterJourneyMotor character;

        private GUIStyle panelStyle;
        private GUIStyle titleStyle;
        private GUIStyle statusStyle;
        private GUIStyle buttonStyle;

        private void OnGUI()
        {
            if (character == null)
                return;

            EnsureStyles();

            const float panelWidth = 356f;
            const float panelHeight = 112f;
            Rect panel = new Rect(18f, 18f, panelWidth, panelHeight);
            GUI.Box(panel, GUIContent.none, panelStyle);
            GUI.Label(new Rect(panel.x + 18f, panel.y + 12f, panelWidth - 36f, 25f),
                "IL VIAGGIO DI ALFIO", titleStyle);

            string status = character.IsPaused
                ? "In pausa"
                : character.IsWalking
                    ? "In cammino verso la scuola"
                    : "Prima tappa raggiunta";
            GUI.Label(new Rect(panel.x + 18f, panel.y + 41f, panelWidth - 36f, 22f), status, statusStyle);

            float buttonY = panel.y + 70f;
            float buttonWidth = 152f;
            if (GUI.Button(new Rect(panel.x + 18f, buttonY, buttonWidth, 30f),
                    character.IsPaused ? "Riprendi" : "Pausa", buttonStyle))
            {
                if (character.IsPaused)
                    character.Resume();
                else
                    character.Pause();
            }

            if (GUI.Button(new Rect(panel.x + 186f, buttonY, buttonWidth, 30f), "Ricomincia", buttonStyle))
                character.RestartJourney();
        }

        private void EnsureStyles()
        {
            if (panelStyle != null)
                return;

            panelStyle = new GUIStyle(GUI.skin.box)
            {
                normal = { background = MakeTexture(new Color(0.08f, 0.12f, 0.16f, 0.92f)) },
                border = new RectOffset(12, 12, 12, 12)
            };
            titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(0.96f, 0.91f, 0.78f) }
            };
            statusStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                normal = { textColor = new Color(0.78f, 0.84f, 0.86f) }
            };
            buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold
            };
        }

        private static Texture2D MakeTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
    }
}
