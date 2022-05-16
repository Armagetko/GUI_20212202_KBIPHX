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
        int BulletNumber { get; set; }

        event EventHandler Changed;
        public event EventHandler LifeLost;
        public event EventHandler GameOver;
        public event EventHandler GameWon;
        int Lives { get; set; }
    }
}
