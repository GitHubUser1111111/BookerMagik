using System.Collections.Generic;
using System.Text;

namespace BookerMagikCore.Infrastructure
{
    public class LongestCommonSubsequence : ILongestCommonSubsequence
    {
        private readonly LCS lcs;

        public LongestCommonSubsequence()
        {
            lcs = new LCS();
        }

        public string Find(string a, string b)
        {
            return lcs.Find(a, b);
        }

        protected class Cell
        {
            public enum Directions
            {
                None,
                Up,
                Left,

                /// <summary>
                ///     Refers to Top,Left
                /// </summary>
                Diagonal
            }

            public int LCS { get; set; }

            public Directions Direction { get; set; }

            //backward reference so that we can backtrack
            public Cell From { get; set; }

            /// <summary>
            ///     C will only have values for row 0 and column 0
            /// </summary>
            public char C { get; set; }

            public int X { get; set; }
            public int Y { get; set; }

            public Directions GetDirection(Cell c)
            {
                if (c.X == X - 1 && c.Y == Y - 1)
                    return Directions.Diagonal;
                if (c.X == X && c.Y == Y - 1)
                    return Directions.Up;
                if (c.X == X - 1 && c.Y == Y)
                    return Directions.Left;
                return Directions.None;
            }
        }

        protected class LCS
        {
            public string Find(string x, string y)
            {
                // columns and rows + 1 because we need to create an empty cell at [0,0]
                var columns = x.Length + 1;
                var rows = y.Length + 1;

                var cells = new Cell[rows * columns];
                cells[0] = new Cell();

                //initialise column 0 cells all to '0'
                for (var c = 1; c < columns; c++) cells[c] = new Cell {X = c, Y = 0, C = x[c - 1]};

                //initialise row 0 cells all to '0'
                for (var r = 1; r < rows; r++) cells[r * columns] = new Cell {X = 0, Y = r, C = y[r - 1]};

                //up till now are initialisation steps. the LCS algo starts here

                for (var r = 1; r < rows; r++)
                for (var c = 1; c < columns; c++)
                {
                    var cell = new Cell {X = c, Y = r};

                    var thisrow = cells[r * columns];
                    var thiscol = cells[c];

                    //compare row and column, if they have the same character, select diagonal cell's LCS
                    if (thisrow.C == thiscol.C)
                    {
                        var diagcell = cells[(r - 1) * columns + c - 1];
                        cell.LCS = diagcell.LCS + 1;
                        cell.From = diagcell;
                    }
                    else
                    {
                        var uppercell = cells[(r - 1) * columns + c];
                        var leftcell = cells[r * columns + c - 1];

                        //take the larger LCS, if not use the upper cell's LCS
                        if (leftcell.LCS > uppercell.LCS)
                        {
                            cell.LCS = leftcell.LCS;
                            cell.From = leftcell;
                        }
                        else
                        {
                            cell.LCS = uppercell.LCS;
                            cell.From = uppercell;
                        }
                    }

                    cells[r * columns + c] = cell;
                }

                //start backtracking
                //we will be getting characters in reverse, so we will use reverse
                var stack = new Stack<char>();

                //last cell i.e bottom right most
                var curr = cells[rows * columns - 1];
                var length = curr.LCS;
                while (curr.From != null)
                {
                    var from = curr.From;
                    var dir = curr.GetDirection(from);
                    if (dir == Cell.Directions.Diagonal)
                    {
                        var c = cells[curr.X].C;
                        stack.Push(c);
                    }

                    curr = from;
                }

                var sbLcs = new StringBuilder();
                for (var i = 0; i < length; i++) sbLcs.Append(stack.Pop());
                return sbLcs.ToString();
            }
        }
    }
}