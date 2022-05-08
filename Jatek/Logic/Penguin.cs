using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Jatek.Logic.JatekLogic;

namespace Jatek.Logic
{
    public class Penguin:GameItem
    {
        public Point Center { get; set; }
        public Directions direction { get; set; }
        public double Angle { get; set; }
        public Penguin(Size gameArea):base(gameArea,25)
        {
            Center = new Point(gameArea.Width/2,gameArea.Height/2);
        }
    }
}
