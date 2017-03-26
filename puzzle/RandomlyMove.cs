using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace puzzle
{
    class RandomlyMove
    {
        private Board board;
        private Timer timer;
        private static int lastId;
        private static Random random = new Random();
        private static int randomTimes;

        public RandomlyMove(Board board, int times = 10, int interval = 40)
        {
            this.board = board;
            timer = new Timer();
            timer.Tick += _rand;
            timer.Interval = interval;
            randomTimes = times;
        }

        private void _rand(object o, EventArgs e)
        {
            if (randomTimes == 0)
            {
                timer.Stop();

                //todo
                //for (int i = 0; i < board.SquareNums; i++)
                //{
                //    Console.WriteLine(i + "-" + board[i].Pos + "   ");
                //}
                    return;
            }
            int rand;
            //对随机出来的方块id进行判断 如果等于上次的id 则重新生成一个，有利于乱序
            while (true)
            {
                rand = random.Next(0, board.SquareNums);
                if (rand == lastId) continue;
                if (board.Move(rand)) break;
            }
            lastId = rand;
            randomTimes--;
        }

        public void Start()
        {
            timer.Start();
        }
        
 
    }
}
