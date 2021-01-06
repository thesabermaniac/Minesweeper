using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Minesweeper2
{
    // For us to use
    public partial class Form1 : Form
    {
        private int MAXBOMBS = 15;
        private bool GameStarted = false;
        private Button[,] buttons;
        List<Button> ButtonList;
        public Form1()
        {
            InitializeComponent();
            Columns = 10;
            Rows = 8;

            InitializeComponent2();
        }

        public int Columns { get; set; }
        public int Rows { get; set; }
        private void InitializeComponent2()
        {
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.buttons = new Button[Rows, Columns];
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                    this.buttons[r, c] = new Button();
            ButtonList = buttons.Cast<Button>().ToList();

            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = Columns;

            for (int c = 0; c < Columns; c++)
                this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / Columns));

            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    this.tableLayoutPanel1.Controls.Add(this.buttons[r, c], c, r);
                }
            }

            this.tableLayoutPanel1.Dock = DockStyle.Fill;
            this.tableLayoutPanel1.Location = new Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = Rows;
            for (int r = 0; r < Rows; r++)
                this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / Rows));

            this.tableLayoutPanel1.Size = new Size(1392, 1213);
            this.tableLayoutPanel1.TabIndex = 0;


            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                {
                    IDictionary<string, int> Coords = new Dictionary<string, int>();
                    Coords.Add("Row", r);
                    Coords.Add("Column", c);
                    Coords.Add("Bomb", 0);
                    Coords.Add("SurroundingBombs", 0);
                    Coords.Add("Active", 0);
                    Coords.Add("Flag", 0);
                    this.buttons[r, c].Dock = DockStyle.Fill;
                    this.buttons[r, c].Location = new Point(3, 3);
                    this.buttons[r, c].Name = "button2";
                    this.buttons[r, c].Size = new Size(315, 600);
                    this.buttons[r, c].TabIndex = 1;
                    this.buttons[r, c].UseVisualStyleBackColor = true;
                    this.buttons[r, c].Tag = Coords;
                    this.buttons[r, c].MouseDown += new MouseEventHandler(this.Button2_Click);
                    this.buttons[r, c].BackColor = Color.Blue;
                }
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(13F, 32F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1392, 1213);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void setSurroundingBombs()
        {
            List<(int, int)> directions = new List<(int, int)>() { (-1, -1), (-1, 0), (-1, 1), (0, 1), (1, 1), (1, 0), (1, -1), (0, -1) };
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    int bombs = 0;
                    IDictionary<string, int> tag = (IDictionary<string, int>)this.buttons[i, j].Tag;
                    foreach (var dir in directions)
                    {
                        if (IsInRange(i + dir.Item1, j + dir.Item2))
                        {
                            IDictionary<string, int> SurroundingTag = (IDictionary<string, int>)this.buttons[i + dir.Item1, j + dir.Item2].Tag;
                            if ((Math.Abs(tag["Column"] - SurroundingTag["Column"]) == 1 && Math.Abs(tag["Row"] - SurroundingTag["Row"]) == 1)
                                || (Math.Abs(tag["Column"] - SurroundingTag["Column"]) == 1 && Math.Abs(tag["Row"] - SurroundingTag["Row"]) == 0)
                                || (Math.Abs(tag["Column"] - SurroundingTag["Column"]) == 0 && Math.Abs(tag["Row"] - SurroundingTag["Row"]) == 1))
                            {
                                if (SurroundingTag["Bomb"] == 1)
                                {
                                    bombs++;
                                }
                            }

                        }
                    }
                    tag["SurroundingBombs"] = bombs;
                }
            }
        }

        private void checkSurroundings(IDictionary<string, int> dict)
        {
            int row = dict["Row"];
            int col = dict["Column"];
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (j == 0 && i == 0)
                    {
                        continue;
                    }
                    int newRow = row + i;
                    int newCol = col + j;
                    if (IsInRange(newRow, newCol))
                    {
                        IDictionary<string, int> tag = (IDictionary<string, int>)this.buttons[newRow, newCol].Tag;
                        if (tag["Active"] == 0)
                        {
                            RevealCell(tag);
                        }
                    }
                }
            }
        }

        private bool IsBoardFull()
        {
            var q = ButtonList.Where(x => (((IDictionary<string, int>)x.Tag)["Active"] == 0) && (((IDictionary<string, int>)x.Tag)["Flag"] == 0));
            var FlagQuery = getQuery("Flag");
            foreach (var btn in FlagQuery)
            {
                IDictionary<string, int> dict = (IDictionary<string, int>)btn.Tag;
                if (dict["Bomb"] == 0)
                {
                    return false;
                }
            }
            if (q.Count() == 0)
            {
                return true;
            }
            return false;

        }

        private IEnumerable<Button> getQuery(string str)
        {
            return ButtonList.Where(x => ((IDictionary<string, int>)x.Tag)[str] == 1);
        }

        private void GameOver()
        {
            var BombQuery = getQuery("Bomb");
            var FlagQuery = getQuery("Flag");
            foreach (var btn in BombQuery)
            {
                IDictionary<string, int> dict = (IDictionary<string, int>)btn.Tag;
                if (dict["Flag"] == 0)
                {
                    btn.BackColor = Color.Red;
                    btn.Text = "Bomb";
                }
            }
            foreach (var btn in FlagQuery)
            {
                IDictionary<string, int> dict = (IDictionary<string, int>)btn.Tag;
                if (dict["Bomb"] == 0)
                {
                    btn.BackColor = Color.Yellow;
                    btn.Text = "X";
                }

            }
            string txt = "Game Over.";
            var ActiveBombs = ButtonList.Where(x => (((IDictionary<string, int>)x.Tag)["Bomb"] == 1) && (((IDictionary<string, int>)x.Tag)["Active"] == 1));
            if (ActiveBombs.Count() > 0)
            {
                txt += "\nYou Lose :(";
            }
            else
                txt += "\nYou Win!";
            MessageBox.Show(txt);
            this.Close();
        }

        private void RevealCell(IDictionary<string, int> dict)
        {
            int row = dict["Row"];
            int col = dict["Column"];
            Button btn = this.buttons[row, col];
            if (dict["Bomb"] == 1)
            {
                btn.BackColor = Color.Red;
                btn.Text = "Bomb";
                dict["Active"] = 1;
                GameOver();
                return;
            }
            else if (dict["SurroundingBombs"] == 0 && dict["Active"] == 0)
            {
                this.buttons[row, col].BackColor = Color.Beige;
                dict["Active"] = 1;
                checkSurroundings(dict);
            }
            else
            {
                btn.BackColor = Color.FromArgb(214, 206, 141);
                btn.Text = dict["SurroundingBombs"] + "";
                dict["Active"] = 1;
                return;
            }
        }

        private bool IsInRange(int i, int j)
        {
            return (i >= 0 && i < Rows) && (j >= 0 && j < Columns);
        }

        private void Button2_Click(object Sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            Button btn = (Button)Sender;
            IDictionary<string, int> SenderTag = (IDictionary<string, int>)btn.Tag;
            if (SenderTag["Active"] == 0)
            {
                if (me.Button == MouseButtons.Right)
                {
                    if (SenderTag["Flag"] == 0)
                    {
                        btn.BackColor = Color.Green;
                        btn.Text = "FLAG";
                        SenderTag["Flag"] = 1;
                    }
                    else
                    {
                        btn.BackColor = Color.Blue;
                        btn.Text = "";
                        SenderTag["Flag"] = 0;
                    }
                }
                if (me.Button == MouseButtons.Left && SenderTag["Flag"] == 0)
                {
                    if (!GameStarted)
                    {
                        btn.BackColor = Color.Beige;
                        int bombs = 0;
                        while (bombs < MAXBOMBS)
                        {
                            int row = new Random().Next(0, Rows);
                            int col = new Random().Next(0, Columns);
                            IDictionary<string, int> tag = (IDictionary<string, int>)this.buttons[row, col].Tag;

                            //Make sure there are no bombs placed around any of the cells adjdacent to the starting cell, or the starting cell itself
                            if ((this.buttons[row, col] != btn)
                                && (Math.Abs(SenderTag["Column"] - tag["Column"]) > 2 || Math.Abs(SenderTag["Row"] - tag["Row"]) > 2)
                                && (Math.Abs(SenderTag["Column"] - tag["Column"]) > 2 || Math.Abs(SenderTag["Row"] - tag["Row"]) != 0)
                                && (Math.Abs(SenderTag["Column"] - tag["Column"]) != 0 || Math.Abs(SenderTag["Row"] - tag["Row"]) > 2))
                            {
                                tag["Bomb"] = 1;
                                bombs++;
                            }
                        }
                        setSurroundingBombs();

                        GameStarted = true;
                    }
                    RevealCell(SenderTag);
                }
                if (IsBoardFull())
                {
                    GameOver();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}