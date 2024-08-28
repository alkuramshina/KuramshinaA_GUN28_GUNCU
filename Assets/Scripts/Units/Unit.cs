using System;
using Board;

namespace Units
{
    public class Unit : BaseBoardElement
    {
        public Cell CurrentCell;
        public event Action OnMoveEndCallback;

        private void Move(Cell to)
        {
            // TODO: move
        
            CurrentCell = to;
            to.CurrentUnit = this;
        
            OnMoveEndCallback?.Invoke();
        }
    }
}
