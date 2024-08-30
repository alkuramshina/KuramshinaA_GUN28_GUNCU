using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;

namespace Board
{
    public class Cell : BaseElement
    {
        public Dictionary<NeighbourType, Cell> Neighbours;
        
        [SerializeField] private MeshRenderer focus; 
        [SerializeField] private MeshRenderer select;

        public ColorType? IsVictoriousFor { get; private set; }
        public bool IsEmpty => Pair is null;

        public bool IsNeighbour(Cell cell)
            => Neighbours.Values.Contains(cell);

        private void Start()
        {
            focus.sharedMaterial = cellPaletteSettings.focused;
            select.sharedMaterial = cellPaletteSettings.selected;
        }

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

        protected override void SetFocused(bool focused)
        {
            focus.enabled = focused;
        }

        public override void SetSelected(bool selected)
        {
            SetFocused(false);
            
            IsSelected = selected;
            select.enabled = selected;
        }
    }
}
