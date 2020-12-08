using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper2
{
    // For us to use
    public partial class Form1 : Form
    {
         private System.Windows.Forms.Button[,] buttons;
        // private System.Windows.Forms.Button button1;
        // private System.Windows.Forms.Button button2;
        // private System.Windows.Forms.Button button3;
        // private System.Windows.Forms.Button button4;
        // private System.Windows.Forms.Button button5;
        // private System.Windows.Forms.Button button6;
        public Form1()
        {
            InitializeComponent();
            Columns = 5;
            Rows = 20;

            InitializeComponent2();
        }

        public int Columns { get; set; }
        public int Rows { get; set; }
        private void InitializeComponent2()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttons = new Button[Rows,Columns];
            for (int r = 0; r < Rows; r++)
            for (int c = 0; c < Columns; c++)
                this.buttons[r,c] = new System.Windows.Forms.Button();

            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = Columns;

            for (int c = 0; c < Columns; c++)
                this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F / Columns));

            for (int r = 0; r < Rows; r++)
            for (int c = 0; c < Columns; c++)
            {
                this.tableLayoutPanel1.Controls.Add(this.buttons[r, c], c, r);
            }

            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = Rows;
            for (int r = 0; r < Rows; r++)
                this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F/Rows));

            this.tableLayoutPanel1.Size = new System.Drawing.Size(1392, 1213);
            this.tableLayoutPanel1.TabIndex = 0;


            for (int r = 0; r < Rows; r++)
            for (int c = 0; c < Columns; c++)
            {
                this.buttons[r,c].Dock = System.Windows.Forms.DockStyle.Fill;
                this.buttons[r,c].Location = new System.Drawing.Point(3, 3);
                this.buttons[r,c].Name = "button2";
                this.buttons[r,c].Size = new System.Drawing.Size(315, 600);
                this.buttons[r,c].TabIndex = 1;
                this.buttons[r, c].UseVisualStyleBackColor = true;
                this.buttons[r, c].Tag = new {Row=r,Column =c};
                this.buttons[r, c].Click += new System.EventHandler(this.button2_Click);
            }
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1392, 1213);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
