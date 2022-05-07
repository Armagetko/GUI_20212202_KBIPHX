﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Jatek.Controller;

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
                    control.Move(Direction.Up);
                    break;
                case Key.Down:
                    control.Move(Direction.Down);
                    break;
                case Key.Left:
                    control.Move(Direction.Left);
                    break;
                case Key.Right:
                    control.Move(Direction.Right);
                    break;
            }
        }
    }
}
