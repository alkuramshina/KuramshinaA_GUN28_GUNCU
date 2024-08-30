using System;
using System.Collections;
using Board;
using Settings;
using Unity.VisualScripting;
using UnityEngine;

namespace Units
{
    public class Unit : BaseElement
    {
        public event Action OnMoveEndCallback;
        public event CrossAnotherUnitHandler OnCrossAnotherUnitHandler;
        
        private const float MovingSpeed = 4f;

        public IEnumerator Move(Cell nextCell)
        {
            var newPosition = CalculateUnitPosition(nextCell);
            while (Vector3.Distance(transform.position, newPosition) >= 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position,
                    newPosition, MovingSpeed * Time.deltaTime);

                yield return null;
            }

            if (this.CheckIfAtTheEndOfBoard(nextCell))
            {
                VictoryConditions.Hooray(this);
                yield break;
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
    
    public delegate void CrossAnotherUnitHandler(Unit player, Unit crossed);
}
