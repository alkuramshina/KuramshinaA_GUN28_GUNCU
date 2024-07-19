using System;
using System.Linq;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    private Cell[,] _cells;
    private Unit[] _units;

    private void Start()
    {
        //_cells = GetComponents<Cell>();
        _units = GetComponents<Unit>();

        foreach (var cell in _cells)
        {
            cell.OnPointerClickEvent += OnCellClicked;
        }
    }

    private void OnCellClicked(Cell cell)
    {
        
    }
}
