using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jatek.Logic
{
    public class Seal
    {
        public static Random r;
        public bool Killed { get; set; }
        public int[] Distances { get; }
        public int[] Position { get; set; }
        public int CurrentDistance { get; set; }
        public Seal(int x, int y)
        {
            r = new Random();
            this.Distances = new int[] { -1, -1, 1, 1, 1, 1, -1,-1 }; //[8]
            Position = new int[] { x,  y};
            Killed = false;
            CurrentDistance = r.Next(1,4);
        }
    }
}
