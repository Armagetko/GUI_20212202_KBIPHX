using Jatek.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jatek.Logic
{
    public class Orca:Seal
    {
        public int OrcaHP { get; set; }
        public Orca(int x, int y):base(x,y)
        {
            OrcaHP = 1;
        }
    }
}
