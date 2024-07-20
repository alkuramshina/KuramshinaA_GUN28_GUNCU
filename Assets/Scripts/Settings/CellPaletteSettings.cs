using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "CellPalette", menuName = "ScriptableObjects/CellPaletteSettings", order = 1)]
    public class CellPaletteSettings: ScriptableObject
    {
        public Material Selected;
        public Material CanMove;
        public Material CanStrike;
        
        public Material whiteCell;
        public Material blackCell;
    }
}