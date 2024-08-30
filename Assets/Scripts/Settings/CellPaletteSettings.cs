using System;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "CellPalette", menuName = "ScriptableObjects/CellPaletteSettings", order = 1)]
    [Serializable]
    public class CellPaletteSettings: ScriptableObject
    {
        public Material selected;
        public Material focused;
        
        public Material whiteCell;
        public Material blackCell;
    }
}