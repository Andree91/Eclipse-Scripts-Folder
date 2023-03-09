using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    [System.Serializable]
    public class ClassStats
    {
        [Header("Class Name")]
        public string className;

        [Header("Class Level")]
        public int classLevel;

        [TextArea]
        public string classDescription;

        [Header("Class Stats")]
        public int healthLevel;
        public int staminaLevel;
        public int manaLevel;
        public int strenghtLevel;
        public int dexterityLevel;
        public int intelligenceLevel;
        public int faithLevel;
        public int arcaneLevel;
    }
}
