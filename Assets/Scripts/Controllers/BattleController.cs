using System.Collections.Generic;
using Board;
using Common;
using Units;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class BattleController : MonoBehaviour
    {
        private List<Cell> _cells;
        private List<Unit> _units;

        private Board.Board _board;
        private PlayerController _playerController;
        
        private Unit _selectedUnit;
        
        private Player _activePlayer;
        private bool _isMoving;

        private void Start()
        {
            (_cells, _units) = _board.Generate(MoveToCell, SelectUnit, EndUnitMove, CheckToEat);
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
            
            _board.HighlightPossiblePaths(_selectedUnit);
        }

        private void Unselect(Unit unit)
        {
            unit.SetSelected(false);

            foreach (var cell in _cells)
            {
                cell.SetSelected(false);
                ((Unit)cell.Pair)?.SetInDanger(false);
            }
        }

        private void EndUnitMove()
        {
            _selectedUnit = null;
            _activePlayer = _playerController.NextTurn(_activePlayer.Color.GetOpponentColor());
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
                Extensions.Hooray(unit);
            }

            unitToEat.OnPointerClickEvent -= SelectUnit;
            unitToEat.OnMoveEndCallback -= EndUnitMove;
            unitToEat.OnCrossAnotherUnit -= CheckToEat;
            
            Destroy(unitToEat.gameObject);
        }

        [Inject] 
        public void Init(Board.Board battlefield, PlayerController playerController)
        {
            _board = battlefield;
            _playerController = playerController;
        }
    }
}
