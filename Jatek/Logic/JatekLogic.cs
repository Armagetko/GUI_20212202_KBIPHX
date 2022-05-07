using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jatek.Logic
{
    public class JatekLogic
    {
        public enum JatekElements
        {
            Player, Wall, Floor, Exit
        }
        public enum Direction
        {
            Up, Down, Left, Right
        }
        public interface IGameModel
        {
            JatekElements[,] GameMatrix { get; set; }
        }
    }
}
