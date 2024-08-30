using System;
using System.Collections.Generic;
using System.Linq;
using Board;
using Units;
using UnityEngine;

namespace Common
{
    public static class Extensions
    {
        public static bool CheckIfAnyAlive(this IEnumerable<Unit> units,
            ColorType playerColor)
            => units.Any(x => x.GetColor == playerColor);

        public static bool AtTheEndOfBoardFor(this Cell cell, UnitDirection direction)
        {
            return direction switch
            {
                UnitDirection.Up =>
                    cell.Neighbours[NeighbourType.Top] is null
                    && cell.Neighbours[NeighbourType.TopLeft] is null
                    && cell.Neighbours[NeighbourType.TopRight] is null,
                UnitDirection.Down =>
                    cell.Neighbours[NeighbourType.Bottom] is null
                    && cell.Neighbours[NeighbourType.Left] is null
                    && cell.Neighbours[NeighbourType.BottomRight] is null,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static NeighbourType GetOpposite(this NeighbourType type)
        {
            return type switch
            {
                NeighbourType.Left => NeighbourType.Right,
                NeighbourType.TopLeft => NeighbourType.BottomRight,
                NeighbourType.Top => NeighbourType.Bottom,
                NeighbourType.TopRight => NeighbourType.BottomLeft,
                NeighbourType.Right => NeighbourType.Left,
                NeighbourType.BottomRight => NeighbourType.TopLeft,
                NeighbourType.Bottom => NeighbourType.Top,
                NeighbourType.BottomLeft => NeighbourType.TopRight,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        
        public static ColorType GetOpponentColor(this ColorType currentColor)
            => currentColor == ColorType.Black ? ColorType.White : ColorType.Black;

        public static void Hooray(Unit unit)
        {
            Debug.Log($"{unit.GetColor.ToString()} победили!");
        }
    }
}