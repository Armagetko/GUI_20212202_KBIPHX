using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jatek.Logic
{
    public interface IGameControl
    {
        void Move(Direction direction);
        void Shoot();
    }
}
