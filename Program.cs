using System;
using System.Threading;

namespace Tetris
{
    class Board
    {
        Cell cell;
        private int height;
        private int width;

        public Board(int height, int width)
        {
            this.height = height; this.width = width;
        }

        internal void setCell(Cell cell) => this.cell = cell;

        internal void Draw()
        {
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (cell.cells[i, j].isBorder)
                        Console.ForegroundColor = ConsoleColor.White;
                    else
                        Console.ForegroundColor = ConsoleColor.Black;

                    cell.drawCell(cell.cells[i, j]);
                }
                Console.Write("\n");
            }
        }
    }

    class Cell
    {
        Board board;
        internal Cell[,] cells = new Cell[25, 10];

        internal string view;
        internal bool isBorder;
        internal bool isBlock;

        public Cell(Board board, string view, bool isBorder)
        {
            this.board = board; this.view = view; this.isBorder = isBorder;
        }

        internal void initialiseCells()
        {
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (i == 0 || j == 0 || i == cells.GetLength(0) - 1 || j == cells.GetLength(1) - 1)
                        cells[i, j] = new Cell(board,"[*]",true);
                    else
                        cells[i, j] = new Cell(board, "[*]", false);
                }
            }
        }

        internal void drawCell(Cell cell)
        {
            if (cell.isBlock)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("*");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write("]");
            }
            else
            {
                Console.Write(view);
            }
        }

        internal void CreateBlock(int x, int y)
        {
            cells[x, y].isBlock = true;
            Move(x, y);
        }

        internal void Move(int x, int y)
        {
            while (!cells[x+1, y].isBorder && !cells[x+1, y].isBlock)
            {
                cells[x, y].isBlock = false;
                cells[++x, y].isBlock = true;
                board.Draw();

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.RightArrow && !cells[x, y + 1].isBorder && !cells[x, y + 1].isBlock)
                    {
                        cells[x, y].isBlock = false;
                        y++;
                    }
                    if (key.Key == ConsoleKey.LeftArrow && !cells[x, y - 1].isBorder && !cells[x, y - 1].isBlock)
                    {
                        cells[x, y].isBlock = false;
                        y--;
                    }
                    if (key.Key == ConsoleKey.DownArrow && !cells[x+1, y].isBorder && !cells[x+1, y].isBlock)
                    {
                        cells[x, y].isBlock = false;
                        x++;
                    }
                }

                Thread.Sleep(40);
            }
            
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board(25, 10);
            Cell cell = new Cell(board, "[*]", false);
            board.setCell(cell);

            cell.initialiseCells();
            
            Console.CursorVisible = false;

            while (board != null)
            {
                board.Draw();
                Thread.Sleep(10);
                cell.CreateBlock(2, 1);
                cell.CreateBlock(2, 2);
                cell.CreateBlock(2, 3);
                cell.CreateBlock(2, 4);
                cell.CreateBlock(2, 5);
                cell.CreateBlock(2, 6);
            }
        }
    }
}
