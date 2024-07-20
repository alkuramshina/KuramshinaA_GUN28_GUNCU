using System;
using System.Collections.Generic;
using UnityEngine;

namespace Board
{
    public class BoardGenerator : MonoBehaviour
    {
        [SerializeField] private Cell cellPrefab;
        private const int BOARD_SIZE = 8;

        public List<Cell> Generate(Action<Cell> onCellClicked,
            Transform boardTransform)
        {
            var board = new Cell[BOARD_SIZE, BOARD_SIZE];
            var currentCellColor = ColorType.Black;

            var cellSize = cellPrefab.transform.localScale.x;
            var cellOffsetX = cellPrefab.transform.localScale.x / 2;
            var cellOffsetZ = cellPrefab.transform.localScale.z / 2;

            for (var row = 0; row < BOARD_SIZE; row++)
            {
                for (var column = 0; column < BOARD_SIZE; column++)
                {
                    var position = new Vector3(cellOffsetX + row * cellSize, 0, cellOffsetZ + column * cellSize);

                    Cell newCell = Instantiate(cellPrefab, position,
                        Quaternion.Euler(90f, 0f, 0f),
                        boardTransform);

                    newCell.name = $"Cell{row}{column}";
                    newCell.SetColor(currentCellColor);
                    newCell.OnPointerClickEvent += onCellClicked;

                    board[row, column] = newCell;

                    currentCellColor = GetOpponentColor(currentCellColor);
                }

                currentCellColor = GetOpponentColor(currentCellColor);
            }

            return ConfigureNeighbours(board);
        }

        private static ColorType GetOpponentColor(ColorType currentColor)
            => currentColor == ColorType.Black ? ColorType.White : ColorType.Black;

        private List<Cell> ConfigureNeighbours(Cell[,] board)
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