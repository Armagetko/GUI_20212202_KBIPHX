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
        int Bulletfishes { get; set; }
        int BulletNumber { get; set; }
        public List<Bullet> Bullets { get; set; }
        int lives { get; set; }

        //event EventHandler Changed;
    }
}
