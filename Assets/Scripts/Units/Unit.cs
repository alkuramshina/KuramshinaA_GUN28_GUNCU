using System;
using System.Collections;
using Board;
using Settings;
using UnityEngine;
using UnityEngine.EventSystems;

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
        
        public override void SetFocused(bool focus)
        {
            if (!focus)
            {
                SetDefaultColor();
                return;
            }
            
            defaultMesh.sharedMaterial = cellPaletteSettings.selected;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            UpdateFocus((Cell) Pair, true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            UpdateFocus((Cell) Pair, false);
        }

        public Vector3 CalculateUnitPosition(Cell onCell)
        {
            return new Vector3(onCell.transform.position.x, 
                onCell.transform.position.y + onCell.transform.localScale.y / 2 + transform.localScale.y, 
                onCell.transform.position.z);
        }
    }
    
    public delegate void CrossAnotherUnitHandler(Unit player, Unit crossed);
}
