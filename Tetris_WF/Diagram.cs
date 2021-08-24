using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris_WF
{
    class Diagram
    {
        internal int X
        {
            get;
            private set;
        }
        internal int Y
        {
            get;
            private set;
        }
        internal int Turn
        {
            get;
            private set;
        }
        internal int BlockNum
        {
            get;
            private set;
        }
        internal Diagram()
        {
            Reset();
        }

        internal void Reset()
        {
            Random random = new Random();
            X = GameRule.SX;
            Y = GameRule.SY;
            Turn = random.Next() % 4;
            BlockNum = random.Next() % 13;
        }

        internal void MoveLeft()
        {
            X--;
        }
        internal void MoveRight()
        {
            X++;
        }
        internal void MoveDown()
        {
            Y++;
        }
        internal void MoveTurn()
        {
            Turn = (Turn + 1) % 4;
        }

    }
}
