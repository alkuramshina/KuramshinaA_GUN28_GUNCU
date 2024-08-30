using System.Collections.Generic;
using Board;
using Settings;
using Units;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class BattleController : MonoBehaviour
    {
        private List<Cell> _board;
        private List<Unit> _units;

        private Battlefield _battlefield;
        private CameraMover _cameraMover;
        
        private ColorType _currentPlayerColor;
        private Unit _selectedUnit;

        private void Start()
        {
            (_board, _units) = _battlefield.Generate(SelectCell, SelectUnit, CheckToEat, FocusOn, EndMove);
            _currentPlayerColor = ColorType.White;
        }
        
        private void FocusOn(BaseElement element, bool focused)
        {
            if (element.IsHighlighted) return;
            element.SetFocused(focused);
            element.Pair?.SetFocused(focused);
        }
    
        private void SelectCell(BaseElement nextCell)
        {
            if (!nextCell.IsHighlighted || _selectedUnit is null)
            {
                return;
            }

            StartCoroutine(_selectedUnit.Move((Cell) nextCell));
        }
        
        private void SelectUnit(BaseElement unit)
        {
            if (unit.GetColor != _currentPlayerColor)
            {
                return;
            }
            
            if (_selectedUnit is not null)
            {
                RemoveHighlight(_selectedUnit);
            }

            unit.SetHighlighted(true);

            var cell = (Cell) unit.Pair;
            cell.SetHighlighted(true);
            
            HighlightNeighborIfAvailable(cell, NeighbourType.TopRight);

            if (unit.GetColor == ColorType.White)
            {
                HighlightNeighborIfAvailable(cell, NeighbourType.TopRight);
                HighlightNeighborIfAvailable(cell, NeighbourType.TopLeft);
            }
            else
            {
                HighlightNeighborIfAvailable(cell, NeighbourType.BottomLeft);
                HighlightNeighborIfAvailable(cell, NeighbourType.BottomRight);
            }
            
            _selectedUnit = (Unit) unit;
        }
        
        private void HighlightNeighborIfAvailable(Cell currentCell,
            NeighbourType neighborType)
        {
            var availableCell = currentCell.Neighbours[neighborType];
            if (availableCell is null)
            {
                return;
            }
            
            if (availableCell.IsEmpty)
            {
                availableCell.SetHighlighted(true);
            }
            else if (availableCell.Pair.GetColor == Battlefield.GetOpponentColor(_currentPlayerColor))
            {
                var cellToMoveAfterEating = availableCell.Neighbours[neighborType];
                if (cellToMoveAfterEating is not null && cellToMoveAfterEating.IsEmpty)
                {
                    cellToMoveAfterEating.SetHighlighted(true);
                }
            }
        }

        private static void RemoveHighlight(Unit unit)
        {
            unit.SetHighlighted(false);
                
            var cell = (Cell) unit.Pair;
            cell.SetHighlighted(false);
            
            foreach (var cellNeighbor in cell.Neighbours)
            {
                cellNeighbor.Value?.SetHighlighted(false);
            }
        }

        private void EndMove()
        {
            RemoveHighlight(_selectedUnit);
            _selectedUnit = null;
            
            _currentPlayerColor = Battlefield.GetOpponentColor(_currentPlayerColor);
            Debug.Log($@"Ход у {(_currentPlayerColor switch
            { ColorType.White => "белых",
                ColorType.Black => "черных",
                _ => "кого-то" })}.");

            StartCoroutine(_cameraMover.MoveCameraToNextPov());
        }
        
        private void CheckToEat(Unit unit, Unit unitToEat)
        {
            // Если триггер не для фишки игрока или пересечение не с фишкой оппонента
            if (unit.GetColor != _currentPlayerColor
                || unit.GetColor == unitToEat.GetColor)
            {
                return;
            }

            unitToEat.SetNewPair(null);
            _units.Remove(unitToEat);

            if (!_units.CheckIfAnyAlive(unitToEat.GetColor))
            {
                VictoryConditions.Hooray(unit);
            }

            unitToEat.OnPointerClickEvent -= SelectUnit;
            unitToEat.OnCrossAnotherUnitHandler -= CheckToEat;
            unitToEat.OnFocusEventHandler -= FocusOn;
            unitToEat.OnMoveEndCallback -= EndMove;
            
            Destroy(unitToEat.gameObject);
        }

        [Inject] 
        public void Init(Battlefield battlefield, CameraMover cameraMover)
        {
            _battlefield = battlefield;
            _cameraMover = cameraMover;
        }
    }
}
