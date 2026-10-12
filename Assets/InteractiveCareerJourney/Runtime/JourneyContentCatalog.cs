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
