using System;
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

        public static bool AtTheEndOfBoard(this Cell cell, UnitDirection direction)
        {
            return direction switch
            {
                UnitDirection.Up => !cell.Neighbours.ContainsKey(NeighbourType.Top) &&
                                      !cell.Neighbours.ContainsKey(NeighbourType.TopLeft) &&
                                      !cell.Neighbours.ContainsKey(NeighbourType.TopRight),
                UnitDirection.Down => !cell.Neighbours.ContainsKey(NeighbourType.Bottom) &&
                                        !cell.Neighbours.ContainsKey(NeighbourType.BottomLeft) &&
                                        !cell.Neighbours.ContainsKey(NeighbourType.BottomRight),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static void Hooray(Unit unit)
        {
            Debug.Log($"{unit.GetColor.ToString()} победили!");
        }
    }
}