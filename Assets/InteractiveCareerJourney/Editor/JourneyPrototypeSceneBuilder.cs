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
        private const string CatalogPath = "Assets/InteractiveCareerJourney/Data/JourneyContentCatalog.asset";

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
            EnsureAssetFolder("Assets/InteractiveCareerJourney", "Data");

            JourneyContentCatalog catalog = EnsureContentCatalog();

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateMaterials();
            CreateLighting();
            CreateRoad();
            GameObject school = CreateSchool();
            GameObject character = CreateCharacter();
            GameObject mentor = CreateComputerScienceMentor();
            CreateHighSchool();
            CreateUniversity(catalog, character.GetComponent<CharacterJourneyMotor>());
            CreateCamera(character.transform);
            JourneyDialogueSequence dialogue = new GameObject("Journey Dialogue Sequence").AddComponent<JourneyDialogueSequence>();
            SerializedObject serializedDialogue = new SerializedObject(dialogue);
            serializedDialogue.FindProperty("alfio").objectReferenceValue = character.GetComponent<CharacterJourneyMotor>();
            serializedDialogue.FindProperty("mentor").objectReferenceValue = mentor;
            serializedDialogue.FindProperty("contentCatalog").objectReferenceValue = catalog;
            serializedDialogue.FindProperty("universityStopX").floatValue = 47f;
            serializedDialogue.ApplyModifiedPropertiesWithoutUndo();
            CreateHud(character.GetComponent<CharacterJourneyMotor>(), dialogue);

            Selection.activeGameObject = character;
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene, ScenePath);
            AddSceneToBuildSettings(ScenePath);
            AssetDatabase.SaveAssets();

            if (!Application.isBatchMode)
            {
                EditorUtility.DisplayDialog(
                    "Prototype scene created",
                    "JourneyPrototype is ready. Press Play to follow Alfio to the university; click the UNICT building to explore the demo course catalog.",
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
            CreatePrimitive("Ground", PrimitiveType.Cube, new Vector3(20f, -0.42f, 1.5f),
                new Vector3(120f, 0.25f, 28f), "Grass");
            CreatePrimitive("Road", PrimitiveType.Cube, new Vector3(28f, -0.12f, 0f),
                new Vector3(100f, 0.18f, 5.5f), "Road");
            CreatePrimitive("Near Road Edge", PrimitiveType.Cube, new Vector3(28f, -0.015f, -2.82f),
                new Vector3(100f, 0.035f, 0.12f), "Cream");

            for (int i = 0; i < 22; i++)
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
                new Vector3(3.1f, 0.28f, 2.65f), "Roof").transform.rotation = Quaternion.Euler(0f, 0f, 28f);
            CreatePrimitive("School Roof Right", PrimitiveType.Cube, new Vector3(9.75f, 3.55f, 1.25f),
                new Vector3(3.1f, 0.28f, 2.65f), "Roof").transform.rotation = Quaternion.Euler(0f, 0f, -28f);
            CreatePrimitive("School Door", PrimitiveType.Cube, new Vector3(8.4f, 0.9f, 0.18f),
                new Vector3(0.86f, 1.8f, 0.12f), "Door");
            CreatePrimitive("Window Left", PrimitiveType.Cube, new Vector3(6.8f, 2.15f, 0.18f),
                new Vector3(0.95f, 0.85f, 0.12f), "Window");
            CreatePrimitive("Window Right", PrimitiveType.Cube, new Vector3(10f, 2.15f, 0.18f),
                new Vector3(0.95f, 0.85f, 0.12f), "Window");

            GameObject sign = new GameObject("School Sign");
            sign.transform.position = new Vector3(8.4f, 4.45f, 0.1f);
            TextMesh text = sign.AddComponent<TextMesh>();
            text.text = "MIDDLE SCHOOL";
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.characterSize = 0.32f;
            text.fontSize = 48;
            text.color = new Color(0.12f, 0.22f, 0.28f);

            CreatePrimitive("School Path", PrimitiveType.Cube, new Vector3(8.4f, 0.02f, -3.8f),
                new Vector3(2.4f, 0.08f, 2.2f), "Path");
            return school;
        }

        private static GameObject CreateComputerScienceMentor()
        {
            GameObject mentor = new GameObject("Computer Science Mentor");
            mentor.transform.position = new Vector3(6.8f, 0.9f, -0.8f);
            CreateCharacterPart("Mentor Body", PrimitiveType.Capsule, mentor.transform,
                new Vector3(0f, 0.08f, 0f), new Vector3(0.58f, 0.58f, 0.36f), "Cream");
            CreateCharacterPart("Mentor Head", PrimitiveType.Sphere, mentor.transform,
                new Vector3(0f, 0.78f, 0f), new Vector3(0.44f, 0.44f, 0.4f), "Skin");
            CreateCharacterPart("Mentor Hair", PrimitiveType.Sphere, mentor.transform,
                new Vector3(0f, 0.9f, 0.015f), new Vector3(0.45f, 0.25f, 0.41f), "Hair");
            return mentor;
        }

        private static void CreateHighSchool()
        {
            CreatePrimitive("High School Building", PrimitiveType.Cube, new Vector3(27f, 1.8f, 1.4f),
                new Vector3(7f, 3.6f, 2.4f), "School");
            CreatePrimitive("High School Roof", PrimitiveType.Cube, new Vector3(27f, 3.72f, 1.4f),
                new Vector3(7.4f, 0.3f, 2.7f), "Roof");
            CreatePrimitive("High School Door", PrimitiveType.Cube, new Vector3(27f, 0.9f, 0.12f),
                new Vector3(1f, 1.8f, 0.12f), "Door");
            GameObject sign = new GameObject("High School Sign");
            sign.transform.position = new Vector3(27f, 4.45f, 0.1f);
            TextMesh text = sign.AddComponent<TextMesh>();
            text.text = "HIGH SCHOOL";
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.characterSize = 0.32f;
            text.fontSize = 48;
            text.color = new Color(0.12f, 0.22f, 0.28f);
            CreatePrimitive("High School Path", PrimitiveType.Cube, new Vector3(27f, 0.02f, -3.8f),
                new Vector3(2.4f, 0.08f, 2.2f), "Path");
        }

        private static void CreateUniversity(JourneyContentCatalog catalog, CharacterJourneyMotor motor)
        {
            GameObject university = new GameObject("UNICT Informatica Triennale - Click to Explore");
            university.transform.position = new Vector3(55f, 0f, 0f);

            CreatePrimitive("University Building", PrimitiveType.Cube, new Vector3(55f, 2.15f, 1.5f),
                new Vector3(10f, 4.3f, 2.8f), "School").transform.SetParent(university.transform, true);
            CreatePrimitive("University Roof", PrimitiveType.Cube, new Vector3(55f, 4.48f, 1.5f),
                new Vector3(10.6f, 0.35f, 3.1f), "Roof").transform.SetParent(university.transform, true);
            CreatePrimitive("University Entrance", PrimitiveType.Cube, new Vector3(55f, 1.05f, 0.04f),
                new Vector3(1.5f, 2.1f, 0.14f), "Door").transform.SetParent(university.transform, true);
            for (int i = 0; i < 4; i++)
            {
                float x = 51.8f + i * 2.15f;
                CreatePrimitive($"University Window {i + 1}", PrimitiveType.Cube, new Vector3(x, 2.75f, 0.02f),
                    new Vector3(1.25f, 1.15f, 0.14f), "Window").transform.SetParent(university.transform, true);
            }

            GameObject sign = new GameObject("UNICT Informatica Triennale Sign");
            sign.transform.SetParent(university.transform, false);
            sign.transform.localPosition = new Vector3(0f, 5.6f, 0.05f);
            TextMesh label = sign.AddComponent<TextMesh>();
            label.text = "UNICT\nInformatica\nTriennale";
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.characterSize = 0.3f;
            label.fontSize = 48;
            label.color = new Color(0.12f, 0.22f, 0.28f);

            GameObject prompt = new GameObject("University Click Prompt");
            prompt.transform.SetParent(university.transform, false);
            prompt.transform.localPosition = new Vector3(0f, 0.08f, -3.4f);
            TextMesh promptText = prompt.AddComponent<TextMesh>();
            promptText.text = "CLICK TO EXPLORE";
            promptText.anchor = TextAnchor.MiddleCenter;
            promptText.alignment = TextAlignment.Center;
            promptText.characterSize = 0.2f;
            promptText.fontSize = 36;
            promptText.color = new Color(0.12f, 0.22f, 0.28f);

            BoxCollider collider = university.AddComponent<BoxCollider>();
            collider.center = new Vector3(0f, 2.7f, 1.2f);
            collider.size = new Vector3(11f, 5.8f, 3.2f);
            UniversityCourseMenu courseMenu = university.AddComponent<UniversityCourseMenu>();
            SerializedObject serializedMenu = new SerializedObject(courseMenu);
            serializedMenu.FindProperty("catalog").objectReferenceValue = catalog;
            serializedMenu.FindProperty("journeyMotor").objectReferenceValue = motor;
            serializedMenu.ApplyModifiedPropertiesWithoutUndo();
        }

        private static JourneyContentCatalog EnsureContentCatalog()
        {
            JourneyContentCatalog catalog = AssetDatabase.LoadAssetAtPath<JourneyContentCatalog>(CatalogPath);
            if (catalog != null)
                return catalog;

            catalog = ScriptableObject.CreateInstance<JourneyContentCatalog>();
            catalog.subjects = CreateMockSubjects();
            AssetDatabase.CreateAsset(catalog, CatalogPath);
            EditorUtility.SetDirty(catalog);
            AssetDatabase.SaveAssets();
            return catalog;
        }

        private static System.Collections.Generic.List<UniversitySubjectEntry> CreateMockSubjects()
        {
            return new System.Collections.Generic.List<UniversitySubjectEntry>
            {
                Subject("Programming Fundamentals", "Demo description: variables, control flow, functions, and problem decomposition."),
                Subject("Algorithms and Data Structures", "Demo description: sorting, searching, complexity, lists, trees, and graphs.",
                    Project("Route Planner (DEMO)", "Mock project: compute a route through a small graph and compare traversal strategies.", "C# · Graphs")),
                Subject("Mathematical Analysis", "Demo description: limits, derivatives, integrals, and mathematical reasoning."),
                Subject("Linear Algebra and Geometry", "Demo description: vectors, matrices, linear transformations, and geometry."),
                Subject("Computer Architecture", "Demo description: processor organization, memory hierarchy, and machine-level representation."),
                Subject("Operating Systems", "Demo description: processes, threads, synchronization, memory, and file systems."),
                Subject("Database Systems", "Demo description: relational modelling, SQL, normalization, and transactions.",
                    Project("Campus Library Database (DEMO)", "Mock project: design a relational database for books, loans, members, and reservations.", "SQL · ER modelling")),
                Subject("Software Engineering", "Demo description: requirements, architecture, testing, version control, and teamwork.",
                    Project("Team Task Manager (DEMO)", "Mock project: a small application for organizing tasks, users, and project milestones.", "C# · UML · Git")),
                Subject("Computer Networks", "Demo description: network layers, addressing, routing, and client-server communication.",
                    Project("Client-Server Chat (DEMO)", "Mock project: exchange text messages between a simple client and server.", "Sockets · TCP")),
                Subject("Web Technologies", "Demo description: build accessible web experiences with client and server technologies.",
                    Project("Student Services Portal (DEMO)", "Mock project: a sample portal with course pages and a student dashboard.", "HTML · CSS · JavaScript")),
                Subject("Programming Languages", "Demo description: language paradigms, interpreters, compilers, and type systems."),
                Subject("Artificial Intelligence", "Demo description: search, planning, knowledge representation, and intelligent agents.",
                    Project("Maze Search Agent (DEMO)", "Mock project: compare search strategies for navigating a generated maze.", "C# · Search algorithms")),
                Subject("Computer Graphics and Game Development", "Demo description: rendering concepts, interaction, and real-time game systems.",
                    Project("2D Platform Game (DEMO)", "Mock project: a small platform game with movement, levels, and collectible objects.", "Unity · C#")),
                Subject("Machine Learning", "Demo description: prepare data, train models, and evaluate predictive performance.",
                    Project("Image Classifier (DEMO)", "Mock project: train and evaluate a basic classifier on a sample image dataset.", "Python · ML")),
                Subject("Final Project", "Demo entry: replace this with the real title and description of your university final project.")
            };
        }

        private static UniversitySubjectEntry Subject(string title, string description,
            params UniversityProjectEntry[] projects)
        {
            return new UniversitySubjectEntry
            {
                title = title,
                description = description,
                projects = new System.Collections.Generic.List<UniversityProjectEntry>(projects)
            };
        }

        private static UniversityProjectEntry Project(string title, string description, string technologies)
        {
            return new UniversityProjectEntry { title = title, description = description, technologies = technologies };
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

        private static void CreateHud(CharacterJourneyMotor motor, JourneyDialogueSequence dialogueSequence)
        {
            GameObject hud = new GameObject("Journey Prototype Controls");
            JourneyPrototypeHud component = hud.AddComponent<JourneyPrototypeHud>();
            SerializedObject serializedHud = new SerializedObject(component);
            serializedHud.FindProperty("character").objectReferenceValue = motor;
            serializedHud.FindProperty("dialogueSequence").objectReferenceValue = dialogueSequence;
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
