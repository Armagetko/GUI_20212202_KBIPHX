using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jatek.Logic
{
    public class Bullet:GameItem
    {
        public Bullet(Size gameArea,int[] whereAmI, int speedX,int speedY):base(gameArea, 8)
        {
            Center = new Point(whereAmI[0], whereAmI[1]);
            SpeedX = speedX;
            SpeedY = speedY;
        }
    }
}
