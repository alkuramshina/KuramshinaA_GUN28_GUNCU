using System;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Board
{
    public class Cell : BaseElement
    {
        public Dictionary<NeighbourType, Cell> Neighbours;
        
        [SerializeField] private MeshRenderer focus; 
        [SerializeField] private MeshRenderer select;

        public ColorType? IsVictoriousFor { get; private set; }
        public bool IsEmpty => Pair is null;
        
        public void Set(Dictionary<NeighbourType, Cell> neighbours)
        {
            if (Neighbours is not null) return;
            
            Neighbours = neighbours;
            
            IsVictoriousFor = neighbours[NeighbourType.TopLeft] is null && neighbours[NeighbourType.TopRight] is null
                ? ColorType.White
                : neighbours[NeighbourType.BottomLeft] is null &&
                  neighbours[NeighbourType.BottomRight] is null
                    ? ColorType.Black
                    : null;
        }
        
        public void SetSelected(bool selected)
        {
            select.enabled = selected;
            select.sharedMaterial = cellPaletteSettings.selected;
        }

        public override void SetFocused(bool focused)
        {
            focus.enabled = focused;
            focus.sharedMaterial = cellPaletteSettings.selected;
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            UpdateFocus(this, true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            UpdateFocus(this, false);
        }
    }
}
