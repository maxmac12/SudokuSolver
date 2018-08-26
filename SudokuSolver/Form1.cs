using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int[,] grid = new int[9, 9];

        private class Cell
        {
            public Cell(int x, int y)
            {
                this.x = x;
                this.y = y;
                this.value = 0;

                // Determine the upper left cell of the quadrant that the cell resides in.
                if (this.x >= 0 && this.x < 3 && this.y >= 0 && this.y < 3)
                {
                    // Quadrant 1
                    this.qX = 0;
                    this.qY = 0;
                }
                else if (this.x >= 0 && this.x < 3 && this.y >= 3 && this.y < 6)
                {
                    // Quadrant 2
                    this.qX = 0;
                    this.qY = 3;
                }
                else if (this.x >= 0 && this.x < 3 && this.y >= 6 && this.y < 9)
                {
                    // Quadrant 3
                    this.qX = 0;
                    this.qY = 6;
                }
                else if (this.x >= 3 && this.x < 6 && this.y >= 0 && this.y < 3)
                {
                    // Quadrant 4
                    this.qX = 3;
                    this.qY = 0;
                }
                else if (this.x >= 3 && this.x < 6 && this.y >= 3 && this.y < 6)
                {
                    // Quadrant 5
                    this.qX = 3;
                    this.qY = 3;
                }
                else if (this.x >= 3 && this.x < 6 && this.y >= 6 && this.y < 9)
                {
                    // Quadrant 6
                    this.qX = 3;
                    this.qY = 6;
                }
                else if (this.x >= 6 && this.x < 9 && this.y >= 0 && this.y < 3)
                {
                    // Quadrant 7
                    this.qX = 6;
                    this.qY = 0;
                }
                else if (this.x >= 6 && this.x < 9 && this.y >= 3 && this.y < 6)
                {
                    // Quadrant 8
                    this.qX = 6;
                    this.qY = 3;
                }
                else if (this.x >= 6 && this.x < 9 && this.y >= 6 && this.y < 9)
                {
                    // Quadrant 9
                    this.qX = 6;
                    this.qY = 6;
                }
            }

            public int x;      // Row index the cell resides in.
            public int y;      // Column index the cell resides in.
            public int qX;     // Row index of the upper left cell of the quadrant that the cell resides in.
            public int qY;     // Column index of the upper left cell of the quadrant that the cell resides in.
            public int value;  // Current value of the cell.
        }


        #region Events
        private void btnSolve_Click(object sender, EventArgs e)
        {
            List<Cell> unsolvedCells = new List<Cell>();

            // Make list of unsolved cells.
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    int temp = 0;

                    if (!int.TryParse(this.textboxes[x][y].Text, out temp))
                    {
                        // Found an unsolved square.
                        unsolvedCells.Add(new Cell(x, y));

                    }

                    grid[x, y] = temp;
                }
            }

            solver(unsolvedCells, 0);
        }


        private void txtBox_Change(object sender, EventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            int input;
            
            if (!int.TryParse(txtBox.Text, out input))
            {
                txtBox.Text = "";
            }
            else if (input == 0)  // Don't allow 0 to be entered.
            {
                txtBox.Text = "";
            }
        }
        #endregion


        private void solver(List<Cell> unsolved, int index)
        {
            if (index >= unsolved.Count || index < 0)
            {
                return;
            }

            Cell currCell = unsolved[index];
            int x = currCell.x;
            int y = currCell.y;

            if (currCell.value > 9)
            {
                // All values have been exhausted. Backtrack to previous cell.
                // Reset this cells value for next iteration.
                currCell.value = 0;
                grid[x, y] = 0;

                setCellAttributes(x, y, Color.Yellow, "");

                solver(unsolved, --index);
            }
            else if (isValid(currCell))
            {
                // Found a valid solution for this cell. Move to next unsolved cell.
                grid[x, y] = currCell.value;

                setCellAttributes(x, y, Color.LimeGreen, currCell.value.ToString());

                solver(unsolved, ++index);
            } 
            else
            {
                // Current value is invalid. Try next value for this cell.
                currCell.value++;

                setCellAttributes(x, y, Color.Red, currCell.value.ToString());

                solver(unsolved, index);
            }
        }


        // Determines if a value is valid for the given position (x, y) where (0, 0) is the upper left of 
        // the puzzle and (9, 9) is the bottom right.
        //
        // Return false if:
        //    1. x < 0 or x > 9 or y < 0 or y > 9, OR
        //    2. Cell value is invalid if it matches any currently filled spaces within its quadrant, OR
        //    3. Cell value is invalid if it matches any currently filled spaces on the row it resides in, OR
        //    4. Cell value is invalid if it matches any currently filled spaces on the column it resides in.
        // otherwise return true.
        private bool isValid(Cell cell)
        {          
            if (!isValidCell(cell))
            {
                return false;
            }

            // Check the quadrant that the cell resides in.
            if (!checkQuadrant(cell))
            {
                return false;
            }

            // Check the row that the cell resides in.
            if (!checkRow(cell))
            {
                return false;
            }

            // Check the column that the cell resides in.
            if (!checkColumn(cell))
            {
                return false;
            }

            return true;
        }


        // Checks the quadrant that the given cell resides and returns true if the cell value is valid.
        //
        //           ----------
        //  (0,0) -> |Q1|Q2|Q3|
        //           ----------
        //           |Q4|Q5|Q6|
        //           ----------
        //           |Q7|Q8|Q9| <- (9,9)
        //           ----------
        private bool checkQuadrant(Cell cell)
        {
            int endX = cell.qX + 3;
            int endY = cell.qY + 3;

            // Check the current quadrant.
            for (int x = cell.qX; x < endX; x++)
            {
                for (int y = cell.qY; y < endY; y++)
                {
                    Color prevColor = this.textboxes[x][y].BackColor;
                    setCellAttributes(x, y, Color.LightBlue, this.textboxes[x][y].Text);
                    
                    int temp = grid[x, y];

                    setCellAttributes(x, y, prevColor, this.textboxes[x][y].Text);

                    if (temp == cell.value)
                    {
                        return false;
                    }


                }
            }

            return true;
        }


        // Checks the row that the given cell resides and returns true if the cell value is valid.
        private bool checkRow(Cell cell)
        {
            // Check all horizontal cells.
            for (int y = 0; y < 9; y++)
            {
                Color prevColor = this.textboxes[cell.x][y].BackColor;
                setCellAttributes(cell.x, y, Color.LightBlue, this.textboxes[cell.x][y].Text);

                int temp = grid[cell.x, y];

                setCellAttributes(cell.x, y, prevColor, this.textboxes[cell.x][y].Text);

                if (temp == cell.value)
                {
                    return false;
                }
            }

            return true;
        }


        // Checks the column that the given cell resides and returns true if the cell value is valid.
        private bool checkColumn(Cell cell)
        {
            // Check all vertical cells.
            for (int x = 0; x < 9; x++)
            {
                Color prevColor = this.textboxes[x][cell.y].BackColor;
                setCellAttributes(x, cell.y, Color.LightBlue, this.textboxes[x][cell.y].Text);

                int temp = grid[x, cell.y];

                setCellAttributes(x, cell.y, prevColor, this.textboxes[x][cell.y].Text);

                if (temp == cell.value)
                {
                    return false;
                }
            }

            return true;
        }


        private void setCellAttributes(int x, int y, Color color, String text)
        {
            // Bound check x and y
            if (x >= 0 && x < 9 && y >= 0 && y < 9)
            {
                this.textboxes[x][y].Text = text;
                this.textboxes[x][y].BackColor = color;
                this.textboxes[x][y].Refresh();
            }
        }


        private bool isValidCell(Cell cell)
        {
            // Bound check x and y
            if (cell.x < 0 || cell.x >= 9 ||
                cell.y < 0 || cell.y >= 9)
            {
                return false;
            }

            // Bound check values.
            if (cell.value <= 0 || cell.value > 9)
            {
                return false;
            }

            return true;
        }
    }
}
