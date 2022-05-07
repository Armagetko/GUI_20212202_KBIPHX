using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Jatek.Logic.JatekLogic;

namespace Jatek.Logic
{
    public interface IGameControl
    {
        void Move(Directions direction);
        void Turn(Directions direction);
        void Shoot();
    }
}
