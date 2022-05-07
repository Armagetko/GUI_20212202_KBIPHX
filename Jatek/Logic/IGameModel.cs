using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Jatek.Logic.JatekLogic;

namespace Jatek.Logic
{
    public interface IGameModel
    {
        Penguin Penguin { get; set; }
        JatekElements[,] GameMatrix { get; set; }
        List<Seal> Seals { get; set; }
        List<Bulletfish> Bulletfishes { get; set; }
        List<Bullet> Bullets { get; set; }

        event EventHandler Changed;
    }
}
