using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jatek.Logic
{
    public class Penguin:GameItem
    {
        public int Center { get; set; }
        public Penguin(Size gameArea):base(gameArea,25)
        {
            
        }
    }
}
