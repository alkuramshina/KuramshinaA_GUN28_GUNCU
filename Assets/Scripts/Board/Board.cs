using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Units;
using UnityEngine;
using Zenject;

namespace Board
{
    public class Board : MonoBehaviour
    {
        private BoardGenerator _boardGenerator;

        public (List<Cell>, List<Unit>) Generate(Action<BaseElement> onCellClicked,
            Action<BaseElement> onUnitClicked,
            Action onUnitMoveEnded,
            CrossAnotherUnitHandler onCrossedAnother)
        {
            return _boardGenerator.Generate(onCellClicked, onUnitClicked, onUnitMoveEnded, onCrossedAnother);
        }
        
        public void HighlightPossiblePaths(Unit selectedUnit)
        {
            var currentCell = (Cell) selectedUnit.Pair;
            
            if (selectedUnit.Direction == UnitDirection.Up)
            {
                HighlightPath(selectedUnit, currentCell, NeighbourType.TopRight);
                HighlightPath(selectedUnit, currentCell, NeighbourType.TopLeft);
            }
            else
            {
                HighlightPath(selectedUnit, currentCell, NeighbourType.BottomLeft);
                HighlightPath(selectedUnit, currentCell, NeighbourType.BottomRight);
            }
        }

        private void HighlightPath(Unit selectedUnit, Cell currentCell, NeighbourType direction)
        {
            var path = new List<Cell> { currentCell };
            
            HighlightAvailable(selectedUnit, path, direction, attackOnly: false);
            HighlightAvailable(selectedUnit, path, direction.GetOpposite(), attackOnly: true);
        }

        private void HighlightAvailable(Unit selectedUnit, List<Cell> path,
            NeighbourType neighborType,
            bool attackOnly)
        {
            var currentCell = path.Last();
            var availableCell = currentCell.Neighbours[neighborType];
            if (availableCell is null)
            {
                return;
            }

            if (availableCell.IsEmpty)
            {
                availableCell.SetSelected(true);
                path.Add(availableCell);
            }
            else if (availableCell.Pair.GetColor == selectedUnit.GetColor.GetOpponentColor())
            {
                var cellToMoveAfterEating = availableCell.Neighbours[neighborType];
                if (cellToMoveAfterEating is null || !cellToMoveAfterEating.IsEmpty) 
                    return;
                
                ((Unit)availableCell.Pair).SetInDanger(true);
                cellToMoveAfterEating.SetSelected(true);
                path.Add(cellToMoveAfterEating);

                HighlightAvailable(selectedUnit, path, neighborType, attackOnly);
            }
        }
        
        [Inject]
        private void Init(BoardGenerator battlefieldGenerator)
        {
            _boardGenerator = battlefieldGenerator;
        }
    }
}