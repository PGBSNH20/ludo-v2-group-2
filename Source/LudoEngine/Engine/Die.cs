using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoEngine.Engine
{
    public class Die
    {
        // Just hold a method that Rolls your die. (Static)
        private static readonly Random _randomNum = new();

        public static int Roll()
        {
            return Roll(1, 6);
        }

        public static int Roll(int min, int max)
        {
            int die = _randomNum.Next(min, max + 1);
            return die;
        }
    }
}
