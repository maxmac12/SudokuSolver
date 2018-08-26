using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

namespace SudokuSolver
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Create textBoxes.
            int x = 0;
            int y = 0;
            Color backColor = Color.LightGray;

            for (int i = 0; i < 9; i++)
            {
                this.textboxes.Add(new List<TextBox>());

                if (i % 3 != 0)
                {
                    if (Color.LightGray == backColor)
                    {
                        backColor = Color.White;
                    }
                    else
                    {
                        backColor = Color.LightGray;
                    }
                }

                for (int j = 0; j < 9; j++)
                {
                    this.textboxes[i].Add(new TextBox());

                    this.textboxes[i][j].Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    this.textboxes[i][j].Location = new System.Drawing.Point(x  , y);
                    this.textboxes[i][j].MaxLength = 1;
                    this.textboxes[i][j].Size = new System.Drawing.Size(35, 35);
                    this.textboxes[i][j].TabIndex = 0;
                    this.textboxes[i][j].Text = "";
                    this.textboxes[i][j].BackColor = backColor;
                    this.textboxes[i][j].TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
                    this.textboxes[i][j].TextChanged += new System.EventHandler(this.txtBox_Change);

                    if (j % 3 == 2)
                    {
                        if (Color.LightGray == backColor)
                        {
                            backColor = Color.White;
                        }
                        else
                        {
                            backColor = Color.LightGray;
                        }
                    }

                    this.Controls.Add(this.textboxes[i][j]);

                    x += 35;
                }

                x = 0;
                y += 35;
            }

            y += 35; 

            this.btnSolve.Location = new Point(x, y);
            this.btnSolve.Size = new Size(100, 35);
            this.btnSolve.Text = "Solve";
            this.btnSolve.Click += new System.EventHandler(this.btnSolve_Click);
            this.Controls.Add(btnSolve);

            y += 35;

            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(35*9, y);
            this.Name = "Form1";
            this.Text = "Sudoko Solver";
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        private List<List<TextBox> > textboxes = new List<List<TextBox>>();
        private Button btnSolve = new Button();


    }
}

