using System;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Board
{
    public class BoardGenerator : MonoBehaviour
    {
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private Unit unitPrefab;
        
        [SerializeField] private ColorType playableCellColor = ColorType.Black;
        [SerializeField] private int startingUnitRows = 3;
        
        private const int BOARD_SIZE = 8;

        public (List<Cell>, List<Unit>) Generate(Action<BaseBoardElement> onCellClicked)
        {
            if (FindAnyObjectByType<Cell>() is not null)
            {
                Debug.LogError("Game has already started!");
            }

            var units = new List<Unit>();
            var board = new Cell[BOARD_SIZE, BOARD_SIZE];
            var currentCellColor = ColorType.Black;

            var localScale = cellPrefab.transform.localScale;
            
            var cellSize = localScale.x;
            var cellOffsetX = localScale.x / 2;
            var cellOffsetZ = localScale.z / 2;

            for (var row = 0; row < BOARD_SIZE; row++)
            {
                for (var column = 0; column < BOARD_SIZE; column++)
                {
                    var position = new Vector3(cellOffsetX + row * cellSize, 0, cellOffsetZ + column * cellSize);
                    var newCell = CreateCell(position, transform, row, column, 
                        currentCellColor,
                        onCellClicked);
                    
                    Unit possibleNewUnit = null;
                    // если это начало доски и мы на играбельном цвете - белые шашки
                    if (currentCellColor == playableCellColor && 0 <= row && row < startingUnitRows)
                    {
                        possibleNewUnit = CreateUnit(newCell, transform, ColorType.White);
                    }
                    else
                        // если это конец доски и мы на играбельном цвете - черные шашки
                    if (currentCellColor == playableCellColor && row >= BOARD_SIZE - startingUnitRows)
                    {
                        possibleNewUnit = CreateUnit(newCell, transform, ColorType.Black);
                    }
                    
                    if (possibleNewUnit is not null)
                    {
                        newCell.CurrentUnit = possibleNewUnit;
                        units.Add(possibleNewUnit);
                    }

                    board[row, column] = newCell;

                    currentCellColor = GetOpponentColor(currentCellColor);
                }

                currentCellColor = GetOpponentColor(currentCellColor);
            }

            return (ConfigureNeighbours(board), units);
        }
        
        private Cell CreateCell(Vector3 cellPosition, Transform boardTransform, 
            int row, int column,
            ColorType color,
            Action<BaseBoardElement> onCellClicked)
        {
            Cell newCell = Instantiate(cellPrefab, cellPosition,
                Quaternion.identity,
                boardTransform);
            
            newCell.name = $"Cell{row}{column}";
            newCell.SetColor(color);
            newCell.OnPointerClickEvent += onCellClicked;

            return newCell;
        }

        private Unit CreateUnit(Cell cell, Transform boardTransform, ColorType color)
        {
            var position = new Vector3(cell.transform.position.x, 
                cell.transform.position.y + cell.transform.localScale.y / 2 + unitPrefab.transform.localScale.y, 
                cell.transform.position.z);
            
            Unit newUnit = Instantiate(unitPrefab, position, Quaternion.identity, boardTransform);
            
            newUnit.SetColor(color);
            newUnit.CurrentCell = cell;

            return newUnit;
        }


        private static ColorType GetOpponentColor(ColorType currentColor)
            => currentColor == ColorType.Black ? ColorType.White : ColorType.Black;

        private static List<Cell> ConfigureNeighbours(Cell[,] board)
        {
            var cells = new List<Cell>();
            for (var row = 0; row < BOARD_SIZE; row++)
            {
                for (var column = 0; column < BOARD_SIZE; column++)
                {
                    var cellNeighbors = new Dictionary<NeighbourType, Cell>
                    {
                        { NeighbourType.Left, column - 1 >= 0 ? board[row, column - 1] : null },
                        {
                            NeighbourType.UpLeft,
                            row + 1 < BOARD_SIZE && column - 1 >= 0 ? board[row + 1, column - 1] : null
                        },
                        { NeighbourType.Up, row + 1 < BOARD_SIZE ? board[row + 1, column] : null },
                        {
                            NeighbourType.UpRight,
                            row + 1 < BOARD_SIZE && column + 1 < BOARD_SIZE ? board[row + 1, column + 1] : null
                        },
                        { NeighbourType.Right, column + 1 < BOARD_SIZE ? board[row, column + 1] : null },
                        {
                            NeighbourType.DownRight,
                            row - 1 >= 0 && column + 1 < BOARD_SIZE ? board[row - 1, column + 1] : null
                        },
                        { NeighbourType.Down, row - 1 >= 0 ? board[row - 1, column] : null },
                        {
                            NeighbourType.DownLeft,
                            row - 1 >= 0 && column - 1 >= 0 ? board[row - 1, column - 1] : null
                        }
                    };

                    board[row, column].Set(cellNeighbors);
                    cells.Add(board[row, column]);
                }
            }

            return cells;
        }
    }
}