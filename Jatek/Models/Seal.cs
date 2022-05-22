using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jatek.Logic
{
    public class Seal
    {
        public bool Killed { get; set; }
        public int[] Position { get; set; }
        public Directions currentDirection { get; set; }
        public int KeptSameDirection { get; set; }
        public bool fishSwallowed { get; set; }
        public Seal(int x, int y)
        {
            Position = new int[] { x, y };
            Killed = false;
            fishSwallowed = false;
        }
        public void SealMovedTo(int x, int y, bool fish=false)
        {
            this.Position[0] = x;
            this.Position[1] = y;
        }
        public void FishSwallowed()
        {
            this.fishSwallowed = true;
        }
    }
}
