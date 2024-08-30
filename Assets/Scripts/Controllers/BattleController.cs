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
        private PlayerController _playerController;
        
        private ColorType _currentPlayerColor;
        private Unit _selectedUnit;

        private bool IsMoving;

        private void Start()
        {
            (_board, _units) = _battlefield.Generate(MoveToCell, SelectUnit, CheckToEat, EndUnitMove);
            
            _currentPlayerColor = ColorType.White;
            _playerController.FirstMove(_currentPlayerColor);
        }
    
        private void MoveToCell(BaseElement nextCell)
        {
            if (_playerController.IsLocked || IsMoving)
                return;
            
            if (!nextCell.IsSelected || _selectedUnit is null)
                return;

            IsMoving = true;

            StartCoroutine(_selectedUnit.Move((Cell) nextCell));

            IsMoving = false;
        }
        
        private void SelectUnit(BaseElement unit)
        {
            if (_playerController.IsLocked || IsMoving)
                return;
            
            if (unit.GetColor != _currentPlayerColor)
            {
                return;
            }
            
            if (_selectedUnit is not null)
            {
                Unselect(_selectedUnit);
            }

            unit.SetSelected(true);

            var cell = (Cell) unit.Pair;
            cell.SetSelected(true);
            
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
                availableCell.SetSelected(true);
            }
            else if (availableCell.Pair.GetColor == Battlefield.GetOpponentColor(_currentPlayerColor))
            {
                var cellToMoveAfterEating = availableCell.Neighbours[neighborType];
                if (cellToMoveAfterEating is not null && cellToMoveAfterEating.IsEmpty)
                {
                    cellToMoveAfterEating.SetSelected(true);
                }
            }
        }

        private static void Unselect(Unit unit)
        {
            unit.SetSelected(false);
                
            var cell = (Cell) unit.Pair;
            cell.SetSelected(false);
            
            foreach (var cellNeighbor in cell.Neighbours)
            {
                cellNeighbor.Value?.SetSelected(false);
            }
        }

        private void EndUnitMove()
        {
            Unselect(_selectedUnit);
            _selectedUnit = null;
            
            _currentPlayerColor = Battlefield.GetOpponentColor(_currentPlayerColor);
            _playerController.NextMove(_currentPlayerColor);
        }
        
        private void CheckToEat(Unit unit, Unit unitToEat)
        {
            Debug.Log("Checking");
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
            unitToEat.OnMoveEndCallback -= EndUnitMove;
            
            Destroy(unitToEat.gameObject);
        }

        [Inject] 
        public void Init(Battlefield battlefield, PlayerController playerController)
        {
            _battlefield = battlefield;
            _playerController = playerController;
        }
    }
}
