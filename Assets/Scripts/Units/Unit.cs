using System;
using System.Collections;
using Board;
using UnityEngine;

namespace Units
{
    public class Unit : BaseElement
    {
        public event Action OnMoveEndCallback;

        private const float MovingSpeed = 4f;
        public UnitDirection Direction { get; private set; }

        public void TurnAround()
        {
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

        public IEnumerator Move(Cell nextCell)
        {
            var newPosition = CalculateUnitPosition(nextCell);
            while (Vector3.Distance(transform.position, newPosition) >= 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position,
                    newPosition, MovingSpeed * Time.deltaTime);

                yield return null;
            }
            
            OnMoveEndCallback?.Invoke();
            
            SetNewPair(nextCell);
            nextCell.SetNewPair(this);
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
            Pair.SetSelected(selected);

            if (selected)
            {
                defaultMesh.sharedMaterial = cellPaletteSettings.selected;
            }
            else
            {
                SetDefaultColor();
            }
        }

        public Vector3 CalculateUnitPosition(Cell onCell)
        {
            return new Vector3(onCell.transform.position.x, 
                onCell.transform.position.y + onCell.transform.localScale.y / 2 + transform.localScale.y, 
                onCell.transform.position.z);
        }

        private void OnCollisionEnter()
        {
            Debug.Log("Trigger");
        }
    }
}
