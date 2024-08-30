using System;
using System.Collections.Generic;
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
            HighlightAvailable(selectedUnit, currentCell, direction);
            HighlightAvailableOnlyForAttack(selectedUnit, currentCell, direction.GetOpposite());
        }

        private void HighlightAvailable(Unit selectedUnit, Cell currentCell,
            NeighbourType neighborType)
        {
            var availableCell = currentCell.Neighbours[neighborType];
            if (availableCell is null)
            {
                return;
            }

            if (availableCell.IsEmpty)
            {
                availableCell.SetSelected(true);
            }
            else if (availableCell.Pair.GetColor == selectedUnit.GetColor.GetOpponentColor())
            {
                var cellToMoveAfterEating = availableCell.Neighbours[neighborType];
                if (cellToMoveAfterEating is null || !cellToMoveAfterEating.IsEmpty) 
                    return;
                
                ((Unit)availableCell.Pair).SetInDanger(true);
                cellToMoveAfterEating.SetSelected(true);

                HighlightAvailable(selectedUnit, cellToMoveAfterEating, neighborType);
            }
        }
        
        private void HighlightAvailableOnlyForAttack(Unit selectedUnit, Cell currentCell,
            NeighbourType neighborType)
        {
            var nextCell = currentCell.Neighbours[neighborType];
            if (nextCell is null || nextCell.IsEmpty || nextCell.Pair.GetColor == selectedUnit.GetColor)
            {
                return;
            }
            
            var cellToMoveAfterEating = nextCell.Neighbours[neighborType];
            if (cellToMoveAfterEating is null || !cellToMoveAfterEating.IsEmpty)
            {
                return;
            }
            
            ((Unit)nextCell.Pair).SetInDanger(true);
            cellToMoveAfterEating.SetSelected(true);

            HighlightAvailableOnlyForAttack(selectedUnit, cellToMoveAfterEating, neighborType);
        }
        
        [Inject]
        private void Init(BoardGenerator battlefieldGenerator)
        {
            _boardGenerator = battlefieldGenerator;
        }
    }
}