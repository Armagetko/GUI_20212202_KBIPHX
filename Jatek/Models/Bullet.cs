using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jatek.Logic
{
    public class Bullet
    {
        public int[] Origin { get; set; }
        public Directions direction { get; set; }
        public Bullet(int[] orig, Directions direction)
        {
            Origin = orig;
            this.direction = direction;
        }
        public void newOrig(int uj0,int uj1)
        {
            this.Origin[0] = uj0;
            this.Origin[1] = uj1;
        }
    }
}
