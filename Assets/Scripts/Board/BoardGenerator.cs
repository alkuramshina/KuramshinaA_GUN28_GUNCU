using System;
using System.Collections.Generic;
using Common;
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

        public (List<Cell>, List<Unit>) Generate(Action<BaseElement> onCellClicked,
            Action<BaseElement> onUnitClicked,
            Action onUnitMoveEnded,
            CrossAnotherUnitHandler onCrossedAnother)
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
                        possibleNewUnit = CreateUnit(newCell, transform, ColorType.White, onUnitClicked,
                            onUnitMoveEnded, onCrossedAnother);
                    }
                    else
                        // если это конец доски и мы на играбельном цвете - черные шашки
                    if (currentCellColor == playableCellColor && row >= BOARD_SIZE - startingUnitRows)
                    {
                        possibleNewUnit = CreateUnit(newCell, transform, ColorType.Black, onUnitClicked,
                            onUnitMoveEnded, onCrossedAnother);
                    }

                    if (possibleNewUnit is not null)
                    {
                        newCell.SetNewPair(possibleNewUnit);
                        units.Add(possibleNewUnit);
                    }

                    board[row, column] = newCell;

                    currentCellColor = currentCellColor.GetOpponentColor();
                }

                currentCellColor = currentCellColor.GetOpponentColor();
            }

            return (ConfigureNeighbours(board), units);
        }

        private Cell CreateCell(Vector3 cellPosition, Transform boardTransform,
            int row, int column,
            ColorType color,
            Action<BaseElement> onCellClicked)
        {
            Cell newCell = Instantiate(cellPrefab, cellPosition,
                Quaternion.identity,
                boardTransform);
            
            newCell.name = $"Cell{row}{column}";
            newCell.SetColor(color);
            newCell.OnPointerClickEvent += onCellClicked;

            return newCell;
        }

        private Unit CreateUnit(Cell cell, Transform boardTransform, ColorType color,
            Action<BaseElement> onUnitClicked,
            Action onUnitMoveEnded,
            CrossAnotherUnitHandler onCrossedAnother)
        {
            var position = unitPrefab.CalculateUnitPosition(cell);
            
            Unit newUnit = Instantiate(unitPrefab, position, Quaternion.identity, boardTransform);
            
            newUnit.SetColor(color);
            newUnit.SetNewPair(cell);
            
            newUnit.OnPointerClickEvent += onUnitClicked;
            newUnit.OnMoveEndCallback += onUnitMoveEnded;
            newUnit.OnCrossAnotherUnit += onCrossedAnother;

            return newUnit;
        }

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
                            NeighbourType.TopLeft,
                            row + 1 < BOARD_SIZE && column - 1 >= 0 ? board[row + 1, column - 1] : null
                        },
                        { NeighbourType.Top, row + 1 < BOARD_SIZE ? board[row + 1, column] : null },
                        {
                            NeighbourType.TopRight,
                            row + 1 < BOARD_SIZE && column + 1 < BOARD_SIZE ? board[row + 1, column + 1] : null
                        },
                        { NeighbourType.Right, column + 1 < BOARD_SIZE ? board[row, column + 1] : null },
                        {
                            NeighbourType.BottomRight,
                            row - 1 >= 0 && column + 1 < BOARD_SIZE ? board[row - 1, column + 1] : null
                        },
                        { NeighbourType.Bottom, row - 1 >= 0 ? board[row - 1, column] : null },
                        {
                            NeighbourType.BottomLeft,
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