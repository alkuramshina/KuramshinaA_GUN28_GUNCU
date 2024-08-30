using System.Collections.Generic;
using System.Linq;
using Board;
using Units;
using UnityEngine;

namespace Settings
{
    public static class VictoryConditions
    {
        public static bool CheckIfAnyAlive(this IEnumerable<Unit> units,
            ColorType playerColor)
            => units.Any(x => x.GetColor == playerColor);

        public static bool CheckIfAtTheEndOfBoard(this Unit unit,
            Cell cell)
            => cell.IsVictoriousFor == unit.GetColor;

        public static void Hooray(Unit unit)
        {
            Debug.Log($"{unit.GetColor.ToString()} победили!");
        }
    }
}