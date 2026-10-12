using System.IO;
using InteractiveCareerJourney;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InteractiveCareerJourneyEditor
{
    /// <summary>Creates the first playable scene from reusable primitive geometry.</summary>
    public static class JourneyPrototypeSceneBuilder
    {
        private const string ScenePath = "Assets/Scenes/JourneyPrototype.unity";
        private const string MaterialFolder = "Assets/InteractiveCareerJourney/Materials";

        [MenuItem("Tools/Interactive Career Journey/Create Prototype Scene")]
        public static void CreatePrototypeScene()
        {
            if (File.Exists(ScenePath) && !EditorUtility.DisplayDialog(
                    "Replace prototype scene?",
                    "JourneyPrototype.unity already exists. Replace it with a fresh graybox scene?",
                    "Replace", "Cancel"))
                return;

            EnsureAssetFolder("Assets", "InteractiveCareerJourney");
            EnsureAssetFolder("Assets/InteractiveCareerJourney", "Materials");

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateMaterials();
            CreateLighting();
            CreateRoad();
            GameObject school = CreateSchool();
            GameObject character = CreateCharacter();
            CreateCamera(character.transform);
            CreateHud(character.GetComponent<CharacterJourneyMotor>());

            Selection.activeGameObject = character;
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, ScenePath);
            AddSceneToBuildSettings(ScenePath);
            AssetDatabase.SaveAssets();

            if (!Application.isBatchMode)
            {
                EditorUtility.DisplayDialog(
                    "Prototype scene created",
                    "JourneyPrototype is ready. Press Play to watch Alfio walk toward the school. " +
                    "Use the on-screen controls to pause, resume, or restart.",
                    "Open scene");
            }
            Debug.Log($"Created {ScenePath} with school milestone: {school.name}");
        }

        private static void CreateLighting()
        {
            RenderSettings.ambientLight = new Color(0.66f, 0.72f, 0.75f);
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;

            GameObject lightObject = new GameObject("Key Light");
            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.15f;
            light.color = new Color(1f, 0.91f, 0.76f);
            lightObject.transform.rotation = Quaternion.Euler(48f, -28f, 0f);
        }

        private static void CreateRoad()
        {
            CreatePrimitive("Ground", PrimitiveType.Cube, new Vector3(10f, -0.42f, 1.5f),
                new Vector3(100f, 0.25f, 28f), "Grass");
            CreatePrimitive("Road", PrimitiveType.Cube, new Vector3(13f, -0.12f, 0f),
                new Vector3(50f, 0.18f, 5.5f), "Road");
            CreatePrimitive("Near Road Edge", PrimitiveType.Cube, new Vector3(13f, -0.015f, -2.82f),
                new Vector3(50f, 0.035f, 0.12f), "Cream");

            for (int i = 0; i < 12; i++)
            {
                float x = -7f + i * 4f;
                CreatePrimitive($"Road Marking {i + 1}", PrimitiveType.Cube,
                    new Vector3(x, -0.01f, -0.2f), new Vector3(1.5f, 0.03f, 0.16f), "RoadLine");
            }

            for (int i = 0; i < 8; i++)
            {
                float x = -8f + i * 4.2f;
                CreatePrimitive($"Distant Tree Trunk {i + 1}", PrimitiveType.Cylinder,
                    new Vector3(x, 1.2f, 5.3f), new Vector3(0.4f, 1.2f, 0.4f), "Bark");
                CreatePrimitive($"Distant Tree Crown {i + 1}", PrimitiveType.Sphere,
                    new Vector3(x, 3.1f, 5.3f), new Vector3(2.1f, 2.4f, 1.8f), "Leaf");
            }
        }

        private static GameObject CreateSchool()
        {
            GameObject school = new GameObject("Middle School Milestone");
            CreatePrimitive("School Building", PrimitiveType.Cube, new Vector3(8.4f, 1.65f, 1.35f),
                new Vector3(5.4f, 3.3f, 2.2f), "School").transform.SetParent(school.transform, true);
            CreatePrimitive("School Roof Left", PrimitiveType.Cube, new Vector3(7.05f, 3.55f, 1.25f),
                new Vector3(3.1f, 0.28f, 2.65f), "Roof").transform.rotation = Quaternion.Euler(0f, 0f, -28f);
            CreatePrimitive("School Roof Right", PrimitiveType.Cube, new Vector3(9.75f, 3.55f, 1.25f),
                new Vector3(3.1f, 0.28f, 2.65f), "Roof").transform.rotation = Quaternion.Euler(0f, 0f, 28f);
            CreatePrimitive("School Door", PrimitiveType.Cube, new Vector3(8.4f, 0.9f, 0.18f),
                new Vector3(0.86f, 1.8f, 0.12f), "Door");
            CreatePrimitive("Window Left", PrimitiveType.Cube, new Vector3(6.8f, 2.15f, 0.18f),
                new Vector3(0.95f, 0.85f, 0.12f), "Window");
            CreatePrimitive("Window Right", PrimitiveType.Cube, new Vector3(10f, 2.15f, 0.18f),
                new Vector3(0.95f, 0.85f, 0.12f), "Window");

            GameObject sign = new GameObject("School Sign");
            sign.transform.position = new Vector3(8.4f, 4.45f, 0.1f);
            TextMesh text = sign.AddComponent<TextMesh>();
            text.text = "SCUOLA MEDIA";
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.characterSize = 0.32f;
            text.fontSize = 48;
            text.color = new Color(0.12f, 0.22f, 0.28f);

            CreatePrimitive("School Path", PrimitiveType.Cube, new Vector3(8.4f, 0.02f, -3.8f),
                new Vector3(2.4f, 0.08f, 2.2f), "Path");
            return school;
        }

        private static GameObject CreateCharacter()
        {
            GameObject character = new GameObject("Alfio - Prototype Character");
            character.transform.position = new Vector3(-5.5f, 0.9f, -0.8f);

            CreateCharacterPart("Body", PrimitiveType.Capsule, character.transform,
                new Vector3(0f, 0.08f, 0f), new Vector3(0.55f, 0.52f, 0.34f), "Shirt");
            CreateCharacterPart("Head", PrimitiveType.Sphere, character.transform,
                new Vector3(0f, 0.73f, 0f), new Vector3(0.42f, 0.42f, 0.4f), "Skin");
            CreateCharacterPart("Hair", PrimitiveType.Sphere, character.transform,
                new Vector3(0f, 0.84f, 0.015f), new Vector3(0.43f, 0.25f, 0.41f), "Hair");
            CreateCharacterPart("Backpack", PrimitiveType.Cube, character.transform,
                new Vector3(-0.34f, 0.05f, 0.21f), new Vector3(0.3f, 0.48f, 0.22f), "Backpack");

            Transform leftLeg = CreateLimb(character.transform, "Left Leg", -0.15f, -0.27f, "Trousers");
            Transform rightLeg = CreateLimb(character.transform, "Right Leg", 0.15f, -0.27f, "Trousers");
            Transform leftArm = CreateArm(character.transform, "Left Arm", -0.34f, 0.22f, "Skin");
            Transform rightArm = CreateArm(character.transform, "Right Arm", 0.34f, 0.22f, "Skin");

            CharacterJourneyMotor motor = character.AddComponent<CharacterJourneyMotor>();
            SerializedObject serializedMotor = new SerializedObject(motor);
            serializedMotor.FindProperty("walkingSpeed").floatValue = 2.2f;
            serializedMotor.FindProperty("stopAtX").floatValue = 5.5f;
            serializedMotor.FindProperty("gaitPivots").arraySize = 4;
            serializedMotor.FindProperty("gaitPhaseOffsets").arraySize = 4;
            Transform[] pivots = { leftLeg, rightLeg, leftArm, rightArm };
            float[] phases = { 0f, Mathf.PI, Mathf.PI, 0f };
            for (int i = 0; i < pivots.Length; i++)
            {
                serializedMotor.FindProperty("gaitPivots").GetArrayElementAtIndex(i).objectReferenceValue = pivots[i];
                serializedMotor.FindProperty("gaitPhaseOffsets").GetArrayElementAtIndex(i).floatValue = phases[i];
            }
            serializedMotor.ApplyModifiedPropertiesWithoutUndo();
            return character;
        }

        private static Transform CreateLimb(Transform parent, string name, float x, float y, string material)
        {
            GameObject pivot = new GameObject(name + " Pivot");
            pivot.transform.SetParent(parent, false);
            pivot.transform.localPosition = new Vector3(x, y, 0f);
            CreateCharacterPart(name, PrimitiveType.Cylinder, pivot.transform,
                new Vector3(0f, -0.22f, 0f), new Vector3(0.12f, 0.27f, 0.12f), material);
            return pivot.transform;
        }

        private static Transform CreateArm(Transform parent, string name, float x, float y, string material)
        {
            GameObject pivot = new GameObject(name + " Pivot");
            pivot.transform.SetParent(parent, false);
            pivot.transform.localPosition = new Vector3(x, y, 0f);
            CreateCharacterPart(name, PrimitiveType.Cylinder, pivot.transform,
                new Vector3(0f, -0.2f, 0f), new Vector3(0.09f, 0.23f, 0.09f), material);
            return pivot.transform;
        }

        private static GameObject CreateCharacterPart(string name, PrimitiveType shape, Transform parent,
            Vector3 localPosition, Vector3 localScale, string materialName)
        {
            GameObject part = GameObject.CreatePrimitive(shape);
            part.name = name;
            part.transform.SetParent(parent, false);
            part.transform.localPosition = localPosition;
            part.transform.localScale = localScale;
            Object.DestroyImmediate(part.GetComponent<Collider>());
            part.GetComponent<Renderer>().sharedMaterial = LoadMaterial(materialName);
            return part;
        }

        private static GameObject CreatePrimitive(string name, PrimitiveType shape, Vector3 position,
            Vector3 scale, string materialName)
        {
            GameObject item = GameObject.CreatePrimitive(shape);
            item.name = name;
            item.transform.position = position;
            item.transform.localScale = scale;
            Collider collider = item.GetComponent<Collider>();
            if (collider != null)
                Object.DestroyImmediate(collider);
            item.GetComponent<Renderer>().sharedMaterial = LoadMaterial(materialName);
            return item;
        }

        private static void CreateCamera(Transform target)
        {
            GameObject cameraObject = new GameObject("Journey Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(target.position.x, 4.2f, -16f);
            cameraObject.transform.rotation = Quaternion.Euler(9f, 0f, 0f);
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 5.4f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.69f, 0.82f, 0.87f);
            cameraObject.AddComponent<AudioListener>();

            JourneyCameraFollow follow = cameraObject.AddComponent<JourneyCameraFollow>();
            SerializedObject serializedFollow = new SerializedObject(follow);
            serializedFollow.FindProperty("target").objectReferenceValue = target;
            serializedFollow.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void CreateHud(CharacterJourneyMotor motor)
        {
            GameObject hud = new GameObject("Journey Prototype Controls");
            JourneyPrototypeHud component = hud.AddComponent<JourneyPrototypeHud>();
            SerializedObject serializedHud = new SerializedObject(component);
            serializedHud.FindProperty("character").objectReferenceValue = motor;
            serializedHud.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void CreateMaterials()
        {
            CreateMaterial("Grass", new Color(0.38f, 0.55f, 0.38f));
            CreateMaterial("Road", new Color(0.28f, 0.34f, 0.37f));
            CreateMaterial("RoadLine", new Color(0.93f, 0.82f, 0.61f));
            CreateMaterial("Cream", new Color(0.97f, 0.88f, 0.68f));
            CreateMaterial("Bark", new Color(0.38f, 0.25f, 0.19f));
            CreateMaterial("Leaf", new Color(0.35f, 0.52f, 0.36f));
            CreateMaterial("School", new Color(0.91f, 0.73f, 0.48f));
            CreateMaterial("Roof", new Color(0.57f, 0.31f, 0.25f));
            CreateMaterial("Door", new Color(0.36f, 0.25f, 0.21f));
            CreateMaterial("Window", new Color(0.45f, 0.72f, 0.78f));
            CreateMaterial("Path", new Color(0.82f, 0.74f, 0.58f));
            CreateMaterial("Shirt", new Color(0.27f, 0.48f, 0.64f));
            CreateMaterial("Skin", new Color(0.83f, 0.62f, 0.48f));
            CreateMaterial("Hair", new Color(0.2f, 0.17f, 0.16f));
            CreateMaterial("Trousers", new Color(0.24f, 0.31f, 0.4f));
            CreateMaterial("Backpack", new Color(0.77f, 0.43f, 0.24f));
        }

        private static void CreateMaterial(string materialName, Color color)
        {
            string path = $"{MaterialFolder}/{materialName}.mat";
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                Shader shader = Shader.Find("Universal Render Pipeline/Lit");
                if (shader == null)
                    shader = Shader.Find("Standard");
                material = new Material(shader) { name = materialName };
                AssetDatabase.CreateAsset(material, path);
            }

            if (material.HasProperty("_BaseColor"))
                material.SetColor("_BaseColor", color);
            if (material.HasProperty("_Color"))
                material.SetColor("_Color", color);
            EditorUtility.SetDirty(material);
        }

        private static Material LoadMaterial(string materialName)
        {
            return AssetDatabase.LoadAssetAtPath<Material>($"{MaterialFolder}/{materialName}.mat");
        }

        private static void EnsureAssetFolder(string parent, string folderName)
        {
            string path = $"{parent}/{folderName}";
            if (!AssetDatabase.IsValidFolder(path))
                AssetDatabase.CreateFolder(parent, folderName);
        }

        private static void AddSceneToBuildSettings(string scenePath)
        {
            EditorBuildSettingsScene[] current = EditorBuildSettings.scenes;
            foreach (EditorBuildSettingsScene entry in current)
            {
                if (entry.path == scenePath)
                    return;
            }

            EditorBuildSettingsScene[] updated = new EditorBuildSettingsScene[current.Length + 1];
            current.CopyTo(updated, 0);
            updated[updated.Length - 1] = new EditorBuildSettingsScene(scenePath, true);
            EditorBuildSettings.scenes = updated;
        }
    }
}
