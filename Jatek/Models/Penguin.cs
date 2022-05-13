using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jatek.Logic
{
    public class Penguin
    {
        public Directions direction { get; set; }
        public Point Center { get; set; }
        public Rectangle Rectangle{ get; set; }
        public Penguin()
        {
            direction = (Directions)3;
        }
        public void NewCenter(Point movedTo)
        {
            this.Center = movedTo;
        }
        public void Rotate(int uj)
        {
            this.direction = (Directions)uj;
        }

    }
}
