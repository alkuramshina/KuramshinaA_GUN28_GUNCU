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
            var currentCell = (Cell)selectedUnit.Pair;

            if (selectedUnit.IsLeveledUp)
            {
                HighlightAvailableCells(selectedUnit, currentCell, NeighbourType.TopRight);
                HighlightAvailableCells(selectedUnit, currentCell, NeighbourType.TopLeft);
                HighlightAvailableCells(selectedUnit, currentCell, NeighbourType.BottomLeft);
                HighlightAvailableCells(selectedUnit, currentCell, NeighbourType.BottomRight);
            }
            else if (selectedUnit.Direction == UnitDirection.Up)
            {
                HighlightAvailableCells(selectedUnit, currentCell, NeighbourType.TopRight);
                HighlightAvailableOnlyForAttackCells(selectedUnit, currentCell, NeighbourType.BottomLeft);

                HighlightAvailableCells(selectedUnit, currentCell, NeighbourType.TopLeft);
                HighlightAvailableOnlyForAttackCells(selectedUnit, currentCell, NeighbourType.BottomRight);
            }
            else
            {
                HighlightAvailableCells(selectedUnit, currentCell, NeighbourType.BottomLeft);
                HighlightAvailableOnlyForAttackCells(selectedUnit, currentCell, NeighbourType.TopRight);

                HighlightAvailableCells(selectedUnit, currentCell, NeighbourType.BottomRight);
                HighlightAvailableOnlyForAttackCells(selectedUnit, currentCell, NeighbourType.TopLeft);
            }
        }

        private void HighlightAvailableCells(Unit selectedUnit, Cell currentCell,
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
                
                if (selectedUnit.IsLeveledUp)
                    HighlightAvailableCells(selectedUnit, availableCell, neighborType);
                    
            }
            else if (availableCell.Pair.GetColor == selectedUnit.GetColor.GetOpponentColor())
            {
                var cellToMoveAfterEating = availableCell.Neighbours[neighborType];
                if (cellToMoveAfterEating is null || !cellToMoveAfterEating.IsEmpty) 
                    return;
                
                ((Unit)availableCell.Pair).SetInDanger(true);
                cellToMoveAfterEating.SetSelected(true);

                HighlightAvailableCells(selectedUnit, cellToMoveAfterEating, neighborType);
            }
        }
        
        private void HighlightAvailableOnlyForAttackCells(Unit selectedUnit, Cell currentCell,
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

            HighlightAvailableOnlyForAttackCells(selectedUnit, cellToMoveAfterEating, neighborType);
        }
        
        [Inject]
        private void Init(BoardGenerator battlefieldGenerator)
        {
            _boardGenerator = battlefieldGenerator;
        }
    }
}