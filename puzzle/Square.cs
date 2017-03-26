using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
namespace puzzle
{

    public enum DIRECTION { RIGHT, LEFT, UP , DOWN }
    public delegate void MoveDoneCallback(int selfPos);
    abstract class Square:PictureBox
    {
        private int id;
        private int pos;
        private  Dictionary<int, DIRECTION> map = new Dictionary<int, DIRECTION> { { 1, DIRECTION.LEFT }, { -1, DIRECTION.RIGHT } };
        public event MoveDoneCallback moveDoneCallback;
        public Square(int id,int size,string style)
        {
            this.id = id;
            pos = id;
            SetStyle(size,id,style);
            Config();
        }

        public abstract void Config();

        protected  void MapConfig(DIRECTION d,int v){
            map[v] = d;
        }

        new public bool Move(int moveCode)
        {
            if (!map.ContainsKey(moveCode)) return false;
            DIRECTION direction = map[moveCode];
            switch (direction)
            {
                case DIRECTION.UP:
                    this.Top -= this.Height;
                    break;
                case DIRECTION.RIGHT:
                    this.Left += this.Width;
                    break;
                case DIRECTION.DOWN:
                    this.Top += this.Height;
                    break;
                case DIRECTION.LEFT:
                    this.Left -= this.Width;
                    break;
            }
            moveDoneCallback(this.pos);
            this.pos = this.pos - moveCode;
            return true;
        }

        public int Id
        {
            get { return id; }
        }
        public int Pos
        {
            get { return pos; }
            set { pos = value; }
        }
        private void SetStyle(int size, int id, string style)
        {
            this.Width = size;
            this.Height = size;
            this.BackgroundImage = Properties.Resources.ResourceManager.GetObject(style + id) as System.Drawing.Image;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.Margin = new Padding(0, 0, 0, 0);
        }

    }


    class Square_3 : Square
    {
        public Square_3(int id, int size, string style)
            : base(id,size,style)
        {

        }
        public override void Config()
        {
            base.MapConfig(DIRECTION.UP, 3);
            base.MapConfig(DIRECTION.DOWN, -3);
        }
    }

    class Square_4 : Square
    {
         public Square_4(int id, int size, string style)
            : base(id,size,style)
        {

        }
        public override void Config()
        {
            base.MapConfig(DIRECTION.UP, 4);
            base.MapConfig(DIRECTION.DOWN, -4);
        }
    }

    class Square_5 : Square
    {
        public Square_5(int id, int size, string style)
            : base(id, size, style)
        {

        }
        public override void Config()
        {
            base.MapConfig(DIRECTION.UP, 5);
            base.MapConfig(DIRECTION.DOWN, -5);
        }
    }
}
