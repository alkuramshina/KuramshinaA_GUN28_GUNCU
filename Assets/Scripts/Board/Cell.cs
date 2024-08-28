using System.Collections.Generic;
using Units;

namespace Board
{
    public class Cell : BaseBoardElement
    {
        public Unit CurrentUnit;
        private Dictionary<NeighbourType, Cell> _neighbours;
        
        public void Set(Dictionary<NeighbourType, Cell> neighbours)
        {
            _neighbours = neighbours;
        }
    }
}
