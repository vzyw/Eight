using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace puzzle
{

    class Status:IComparable
    {
        public int lastMovedId;     //移动哪个方块
        public int residueSteps;    //移动后剩余方块需要移动端步数
        public Status parent;
        public int[] board;
        public int spacePos;
        public Status(int lastMovedId, int steps, int[] board ,int spacePos,Status parent)
        {
            this.lastMovedId = lastMovedId;
            this.residueSteps = steps;
            this.parent = parent;
            this.board = board;
            this.spacePos = spacePos;
        }
        public int CompareTo(Object other){
            return this.residueSteps - ((Status)other).residueSteps;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (GetType() != obj.GetType()) return false;
            for (int i = 0; i < this.board.Length; i++)
            {
                if (this.board[i] != ((Status)obj).board[i]) return false;
            }
            return true;
        }
        public override int GetHashCode()
        {
            return this.board.ToString().GetHashCode();
        }
    }
    class Recover
    {
        private Board board;
        private QriorityQueue statusQueue;
        private Stack<int> steps;
        private Dictionary<Status, bool> check;

        private int rank;   //阶
        private int spaceId;//空格的id

        private Timer timer;
        public Recover(Board board)
        {
            this.board = board;
            statusQueue = new QriorityQueue();
            steps = new Stack<int>();
            rank = board.Rank;
            spaceId = rank * rank - 1;
            check = new Dictionary<Status, bool>();
            timer = new Timer();
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (steps.Count == 0)
            {
                timer.Stop();
                return;
            }
            //Console.Write(steps.Peek());
            board.Move(steps.Pop());
        }

        public void Go(int interval)
        {
            if (board.Win()) return;
            Status finalStatus = _Go();
            if (finalStatus != null)
            {
                while (finalStatus.parent != null)
                {
                    steps.Push(finalStatus.lastMovedId);
                    finalStatus = finalStatus.parent;
                }
            }

            timer.Interval = interval;
            timer.Start();
        }


        public Status _Go()
        {
            int spacePos = board.SpacePos;
            int[] pos = new int[rank * rank];
            pos[spacePos] = spaceId;
            for (int i = 0; i < board.SquareNums; i++)
            {
                pos[board[i].Pos] = i;
            }


            //int[] pos = { 0,12, 3, 4, 5, 6, 7, 8, 9, 10, 11, 1, 2, 13, 14, 15 };
            //int spacePos = 15;
            int steps = RecoverSteps(pos);

            statusQueue.Add(new Status(-1, steps, pos , spacePos, null));


            while (!statusQueue.Empty())
            {
                
                Status status = statusQueue.Top();
                check[status] = true;
                spacePos = status.spacePos;
                List<int> movebalePos = MovebalePos(spacePos);
 
                foreach (int x in movebalePos)
                {
                    if (status.board[x] == status.lastMovedId) continue;
                    Status newStatus = CreatNewStatus(status, x);
                    if (newStatus.residueSteps == 0) return newStatus;
                    else if(!check.ContainsKey(newStatus))
                    {
                        statusQueue.Add(newStatus);
                        
                    }
                }
            }

            return null;
        }


        private Status CreatNewStatus(Status lastStatus,int movebalePos)
        {
            int[] newPos = (int[])lastStatus.board.Clone();     //新建一个棋盘
            int movedId = newPos[movebalePos];
            Swap(ref newPos[movebalePos], ref newPos[lastStatus.spacePos]);       //交换空格和x上的方块
            int steps = RecoverSteps(newPos);
            return new Status(movedId, steps, newPos, movebalePos, lastStatus);
        }

        private bool CanMove(int pos , int spacePos)
        {
            if (pos > rank * rank - 1 || pos < 0) return false;
            if (pos % rank == 0)
            {
                if ((spacePos + 1) % rank == 0) return false;
            }
            else if ((pos + 1) % rank == 0)
            {
                if (spacePos % rank == 0) return false;
            }
            return true;
        }
        //可移动的位置
        private List<int> MovebalePos(int spacePos)
        {
            int[] temp = new int[] { spacePos - rank, spacePos + 1, spacePos + rank, spacePos - 1 };
            List<int> res = new List<int>();
            foreach (int x in temp)
            {
                if(CanMove(x,spacePos))res.Add(x);
            }
            return res;
        }

        private int RecoverSteps(int[] board)
        {
            int step = 0;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == i || board[i] == board.Length - 1 ) continue;
                step += Distance(board[i], i);
            }
            return step;
        }

        //方块到达目标的距离
        private int Distance(int id , int pos)
        {
            return Math.Abs(id / rank - pos / rank) + Math.Abs(id % rank - pos % rank);
        }

        private void Swap( ref int spacePos,ref int currPos)
        {
            int temp = spacePos;
            spacePos = currPos;
            currPos = temp;
        }

        private bool Win(int[] board)
        {
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] != i) return false;
            }
            return true;
        }
    }
}
