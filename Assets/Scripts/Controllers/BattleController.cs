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
        
        private Unit _selectedUnit;
        private Player _activePlayer;

        private bool _isMoving;

        private void Start()
        {
            (_board, _units) = _battlefield.Generate(MoveToCell, SelectUnit, EndUnitMove);
            
            _activePlayer = _playerController.Start(ColorType.White);
        }
    
        private void MoveToCell(BaseElement nextCell)
        {
            if (_playerController.IsLocked || _isMoving)
                return;
            
            if (!nextCell.IsSelected || _selectedUnit is null)
                return;

            _isMoving = true;

            var cell = (Cell)nextCell;
            
            StartCoroutine(_selectedUnit.Move(cell));
            
            if (cell.AtTheEndOfBoard(_selectedUnit.Direction))
            {
                _selectedUnit.TurnAround();
            }

            _isMoving = false;
        }
        
        private void SelectUnit(BaseElement unit)
        {
            if (_playerController.IsLocked || _isMoving)
                return;
            
            if (unit.GetColor != _activePlayer.Color)
            {
                return;
            }
            
            if (_selectedUnit is not null)
            {
                Unselect(_selectedUnit);
            }

            _selectedUnit = (Unit) unit;
            
            unit.SetSelected(true);
            HighlightPossibleMoves();
        }

        private void HighlightPossibleMoves()
        {
            var currentCell = (Cell) _selectedUnit.Pair;

            if (_selectedUnit.Direction == UnitDirection.Up)
            {
                HighlightNeighborIfAvailable(currentCell, NeighbourType.TopRight);
                HighlightNeighborIfAvailable(currentCell, NeighbourType.TopLeft);
            }
            else
            {
                HighlightNeighborIfAvailable(currentCell, NeighbourType.BottomLeft);
                HighlightNeighborIfAvailable(currentCell, NeighbourType.BottomRight);
            }
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
            else if (availableCell.Pair.GetColor == Battlefield.GetOpponentColor(_activePlayer.Color))
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
            foreach (var cellNeighbor in cell.Neighbours)
            {
                cellNeighbor.Value?.SetSelected(false);
            }
        }

        private void EndUnitMove()
        {
            Unselect(_selectedUnit);
            
            _selectedUnit = null;
            _activePlayer = _playerController.NextTurn(Battlefield.GetOpponentColor(_activePlayer.Color));
        }
        
        private void CheckToEat(Unit unit, Unit unitToEat)
        {
            Debug.Log("Checking");
            // Если триггер не для фишки игрока или пересечение не с фишкой оппонента
            if (unit.GetColor != _activePlayer.Color
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
