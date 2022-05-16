using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Jatek.Logic.JatekLogic;

namespace Jatek.Logic
{
    public interface IGameControl
    {
        void Move(Directions direction);
        void Rotate(int uj);
        void Shoot();
        void PauseGame();
    }
}
