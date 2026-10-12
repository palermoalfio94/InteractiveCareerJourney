using System;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveCareerJourney
{
    /// <summary>Inspector-editable narrative text and university portfolio content.</summary>
    [CreateAssetMenu(menuName = "Interactive Career Journey/Content Catalog", fileName = "JourneyContentCatalog")]
    public sealed class JourneyContentCatalog : ScriptableObject
    {
        [Header("Narrative dialogue")]
        [TextArea(2, 4)] public string middleSchoolQuestion = "Hey, you seem a smart kid! Do you wanna learn Computer Science?";
        [TextArea(1, 2)] public string middleSchoolAnswer = "Why not?";
        public string middleSchoolReplyButton = "Reply";
        public string continueButton = "Continue";
        public string yearsLaterCaption = "5 YEARS LATER";
        [Min(0.1f)] public float yearsLaterDuration = 3f;
        [TextArea(4, 8)] public string highSchoolReflection = "WOW! I really enjoyed Computer Science! Now I can program in C++, C and C#, and I also know how to design and implement a database! I also have the basics of systems and networking! This is fantastic!";

        [Header("University and work in parallel")]
        public string workCompany = "eyePlus Tecnowork";
        public string workRole = "GAME PROGRAMMER";
        public string workTools = "Unity  •  Blender";
        public string studyBubble = "STUDYING";
        public string oneYearLaterCaption = "ONE YEAR LATER";
        [Min(0.1f)] public float workStoryDuration = 10f;
        [Min(0.1f)] public float oneYearCaptionDuration = 1.8f;
        [Min(0.1f)] public float workCloneExitDuration = 0.7f;
        public string graduationResult = "GRADUATED  •  110/110";
        public string graduationThesis = "Thesis: Semantic Segmentation with Deep Learning";

        [Header("University")]
        public string universityTitle = "UNICT Informatica Triennale";
        [TextArea(1, 3)] public string mockContentNotice = "SAMPLE CONTENT — Replace these demo subjects and projects with your own.";
        public List<UniversitySubjectEntry> subjects = new List<UniversitySubjectEntry>();
    }

    [Serializable]
    public sealed class UniversitySubjectEntry
    {
        public string title;
        [TextArea(2, 5)] public string description;
        public List<UniversityProjectEntry> projects = new List<UniversityProjectEntry>();
    }

    [Serializable]
    public sealed class UniversityProjectEntry
    {
        public string title;
        [TextArea(2, 6)] public string description;
        public string technologies;
    }
}
