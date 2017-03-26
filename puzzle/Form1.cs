using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace puzzle
{
    public partial class Form1 : Form
    {
        private static int rank = 3;
        private Board board;
        private string picStyle = "v_";
        private int randomTimes = 35;
        private int interval = 100;
        public Form1()
        {
            InitializeComponent();
            this.randomTimes = Convert.ToInt32(times.Text);
            this.interval = Convert.ToInt32(inter.Text);
            Init();
        }
        private void Init()
        {
            panel.Controls.Clear();
            int squareSize = panel.Size.Width / rank;
            board = new Board(rank, squareSize, picStyle);
            board.AddTo(this.panel);
            board.callback += Win;
        }

        public void Win(ref int n)
        {
            MessageBox.Show("Win!! 共走了"+n+"步" );
            n = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            board.RandomSquares(randomTimes,interval);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            int temp = Convert.ToInt32(((RadioButton)sender).Tag);
            if (temp != rank)
            {
                rank = temp;
                switch (rank)
                {
                    case 3:
                        picStyle = "v_";break;
                    case 4:
                        picStyle = "v2_";break;
                    case 5:
                        picStyle = "v5_";break;
                }
                Init();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Recover(board).Go(interval);
        }

        private void times_TextChanged(object sender, EventArgs e)
        {
            this.randomTimes = Convert.ToInt32(times.Text);
        }

        private void inter_TextChanged(object sender, EventArgs e)
        {
            this.interval = Convert.ToInt32(inter.Text);
        }

    }
}
