using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jatek.Logic
{
    public class Seal
    {
        public int DiffFromStart { get; set; }
        public int prevPlace { get { return DiffFromStart - 1; } }
        private int[] position;
        public int[] Position { get { return position; } }
        public Seal(int x, int y)
        {
            DiffFromStart = 0;
            position = new int[] { x,  y};
        }
    }
}
