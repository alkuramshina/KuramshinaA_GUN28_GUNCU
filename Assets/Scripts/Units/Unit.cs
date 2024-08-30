using System;
using System.Collections;
using Board;
using Settings;
using UnityEngine;

namespace Units
{
    public class Unit : BaseElement
    {
        [SerializeField] private MeshRenderer inDanger; 
        
        public event Action OnMoveEndCallback;
        public event CrossAnotherUnitHandler OnCrossAnotherUnit;

        private const float MovingSpeed = 4f;
        public UnitDirection Direction { get; private set; }

        private void LevelUp()
        {
            Debug.Log("End of board! Level Up!");
            Direction = Direction == UnitDirection.Down 
                ? UnitDirection.Up 
                : UnitDirection.Down;
        }

        public override void SetColor(ColorType color)
        {
            base.SetColor(color);
            Direction = color == ColorType.White 
                ? UnitDirection.Up 
                : UnitDirection.Down;
        }

        public IEnumerator Move(Cell targetCell)
        {
            var newPosition = CalculateUnitPosition(targetCell);
            while (Vector3.Distance(transform.position, newPosition) >= 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position,
                    newPosition, MovingSpeed * Time.deltaTime);

                yield return null;
            }
            
            OnMoveEndCallback?.Invoke();
            
            SetNewPair(targetCell);
            targetCell.SetNewPair(this);
            
            if (targetCell.AtTheEndOfBoardFor(Direction))
            {
                LevelUp();
            }
        }

        protected override void SetFocused(bool focused)
        {
            if (IsSelected) return;
            
            if (focused)
            {
                defaultMesh.sharedMaterial = cellPaletteSettings.focused;
            }
            else
            {
                SetDefaultColor();
            }
        }

        public override void SetSelected(bool selected)
        {
            IsSelected = selected;

            if (selected)
            {
                defaultMesh.sharedMaterial = cellPaletteSettings.selected;
            }
            else
            {
                SetDefaultColor();
            }
        }

        public void SetInDanger(bool danger)
        {
            inDanger.enabled = danger;
        }

        public Vector3 CalculateUnitPosition(Cell onCell)
        {
            return new Vector3(onCell.transform.position.x, 
                onCell.transform.position.y + onCell.transform.localScale.y / 2 + transform.localScale.y, 
                onCell.transform.position.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Unit>(out var crossed))
            {
                OnCrossAnotherUnit?.Invoke(this, crossed);
            }
        }
    }
    
    public delegate void CrossAnotherUnitHandler(Unit player, Unit crossed);
}
