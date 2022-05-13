using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Jatek.Controller;
using Jatek.Logic;
using static Jatek.Logic.JatekLogic;

namespace Jatek.Controller
{
    public class GameController
    {
        IGameControl control;

        public GameController(IGameControl control)
        {
            this.control = control;
        }
        public void KeyPressed(Key key)
        {
            switch (key)
            {
                case Key.Up:
                    control.Rotate(0);
                    control.Move(Directions.up);
                    break;
                case Key.Down:
                    control.Rotate(2);
                    control.Move(Directions.down);
                    break;
                case Key.Left:
                    control.Rotate(1);
                    control.Move(Directions.left);
                    break;
                case Key.Right:
                    control.Rotate(3);
                    control.Move(Directions.right);
                    break;
                case Key.Space:
                    control.Shoot();
                    break;
            }
        }
    }
}
