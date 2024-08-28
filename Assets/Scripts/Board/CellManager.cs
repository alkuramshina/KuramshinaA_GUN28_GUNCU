using System.Collections.Generic;
using Units;
using UnityEngine;
using Zenject;

namespace Board
{
    public class CellManager : MonoBehaviour
    {
        private List<Cell> _board;
        private List<Unit> _units;

        [Inject] private BoardGenerator _boardGenerator;

        private void Start()
        {
            (_board, _units) = _boardGenerator.Generate(OnCellClicked);
        }
    
        private void OnCellClicked(BaseBoardElement cell)
        {
        }
    }
}
