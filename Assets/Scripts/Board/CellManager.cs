using System;
using System.Collections.Generic;
using UnityEngine;

namespace Board
{
    public class CellManager : MonoBehaviour
    {
        private List<Cell> _board;
        private Unit[] _units;

        private void Start()
        {
            var boardGenerator = GetComponent<BoardGenerator>();
            _board = boardGenerator.Generate(OnCellClicked, transform);
        }
    
        private void OnCellClicked(Cell cell)
        {
        }
    }
}
