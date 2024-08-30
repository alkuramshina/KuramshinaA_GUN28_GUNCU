using System.Collections.Generic;
using System.Linq;
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
            (_board, _units) = _battlefield.Generate(MoveToCell, SelectUnit, EndUnitMove, CheckToEat);
            _activePlayer = _playerController.Start(ColorType.White);
        }
    
        private void MoveToCell(BaseElement nextCell)
        {
            if (_playerController.IsLocked || _isMoving)
                return;
            
            if (!nextCell.IsSelected || _selectedUnit is null)
                return;
            
            Unselect(_selectedUnit);

            _isMoving = true;
            
            StartCoroutine(_selectedUnit.Move((Cell)nextCell));

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
            HighlightPossiblePaths();
        }

        private void HighlightPossiblePaths()
        {
            var currentCell = (Cell) _selectedUnit.Pair;
            
            if (_selectedUnit.Direction == UnitDirection.Up)
            {
                HighlightPath(currentCell, NeighbourType.TopRight);
                HighlightPath(currentCell, NeighbourType.TopLeft);
            }
            else
            {
                HighlightPath(currentCell, NeighbourType.BottomLeft);
                HighlightPath(currentCell, NeighbourType.BottomRight);
            }
        }

        private void HighlightPath(Cell currentCell, NeighbourType direction)
        {
            var path = new List<Cell> { currentCell };
            HighlightAvailable(path, direction);
        }

        private void HighlightAvailable(List<Cell> path,
            NeighbourType neighborType)
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
            else if (availableCell.Pair.GetColor == Battlefield.GetOpponentColor(_activePlayer.Color))
            {
                var cellToMoveAfterEating = availableCell.Neighbours[neighborType];
                if (cellToMoveAfterEating is null || !cellToMoveAfterEating.IsEmpty) 
                    return;
                
                ((Unit)availableCell.Pair).SetInDanger(true);
                cellToMoveAfterEating.SetSelected(true);
                path.Add(cellToMoveAfterEating);

                HighlightAvailable(path, neighborType);
            }
        }

        private void Unselect(Unit unit)
        {
            unit.SetSelected(false);

            foreach (var cell in _board)
            {
                cell.SetSelected(false);
                ((Unit)cell.Pair)?.SetInDanger(false);
            }
        }

        private void EndUnitMove()
        {
            _selectedUnit = null;
            _activePlayer = _playerController.NextTurn(Battlefield.GetOpponentColor(_activePlayer.Color));
        }
        
        private void CheckToEat(Unit unit, Unit unitToEat)
        {
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
            unitToEat.OnCrossAnotherUnit -= CheckToEat;
            
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
