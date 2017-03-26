using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace puzzle
{
    delegate void BoardHandle(ref int movetimes);
    class Board : Object
    {
        private Square[] squares;
        private int rank;
        private static int spacePos;
        private int moveTimes;

        public event BoardHandle callback;
        public Board(int rank, int size, string style)
        {
            this.rank = rank;
            spacePos = rank * rank - 1;
            int squaresNum = rank * rank - 1;
            squares = new Square[squaresNum];
            for (int i = 0; i < squaresNum; i++)
            {
                Square s;
                if (rank == 3)
                    s = new Square_3(i, size, style);
                else if(rank == 4)
                    s = new Square_4(i, size, style);
                else
                    s = new Square_5(i, size, style);
                s.Top = i / rank * size;
                s.Left = i % rank * size;
                s.Click += Square_Click;
                s.moveDoneCallback += Callback;
                squares[i] = s;
            }
            moveTimes = 0;
        }

        
        //把所有方块添加到 targetPanel
        public void AddTo(Panel targetPanel)
        {
            foreach (Square s in squares)
            {
                targetPanel.Controls.Add(s);
            }
        }

        //随机打乱方块
        public void RandomSquares(int times = 35, int interval = 40)
        {
            new RandomlyMove(this, times, interval).Start();
        }
        
        //检测是否成功
        public bool Win()
        {
            foreach (Square s in squares)
            {
                if (s.Pos != s.Id) return false;
            }
            return true;
        }

        //点击事件
        void Square_Click(object sender, EventArgs e)
        {
            if (Move((Square)sender)) moveTimes++;  //移动
            if (Win()) callback(ref moveTimes);     //判断是否拼图完成
        }

        //移动方块
        public bool Move(Square s)
        {
            int moveCode = MoveCode(s, spacePos);
            return s.Move(moveCode);
        }

        //移动指定id的方块
        public bool Move(int id)
        {
            return Move(squares[id]);
        }


        //获取方块个数
        public int SquareNums
        {
            get { return squares.Length; }
        }
        public int SpacePos
        {
            get { return spacePos; }
        }

        public Square this[int index]
        {
            get { return squares[index]; }
        }
        public int Rank
        {
            get { return rank; }
        }
        private void Callback(int squarePos)
        {
            spacePos = squarePos;
        }

        private int MoveCode(Square s, int spacePos)
        {
            if (s.Pos % rank == 0)
            {
                if ((spacePos + 1) % rank == 0) return 0;
            }
            else if ((s.Pos + 1) % rank == 0)
            {
                if (spacePos % rank == 0) return 0;
            }
            return s.Pos - spacePos;
        }
    }
}
