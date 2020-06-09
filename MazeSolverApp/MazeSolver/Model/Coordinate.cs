using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver.Model
{
    /// <summary>
    /// Data Type representing Row & Column location within a grid
    /// </summary>
    readonly struct Coordinate
    {
        public int Row { get; }
        public int Column { get; }

        public Coordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override string ToString()
        {
            return ($"({Column},{Row})");
        }
    } 
}
