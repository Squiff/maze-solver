using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver.Model
{
    class Cell
    {
        public Maze Maze { get; }
        public Coordinate Coordinate { get; }
        public bool Visited { get; set; }
        public double Distance { get; set; }
        public Cell FromCell { get; set; }
        public Cell ToCell { get; set; }
        private bool _isWall = false;

        /// <summary>
        /// Cell Constructor
        /// </summary>
        /// <param name="maze">Parent Maze to which the Cell belongs to</param>
        /// <param name="coordinate">The Coordinate within the Maze</param>
        public Cell(Maze maze, Coordinate coordinate)
        {
            Coordinate = coordinate;
            Maze = maze;
        }

        /// <summary>
        /// Specifies whether this is a Start, Finish, Wall or Path point
        /// </summary>
        public CellType CellType
        {
            get
            {
                if (this == Maze.StartCell)
                    return CellType.Start;
                else if (this == Maze.FinishCell)
                    return CellType.Finish;
                else if (_isWall == true)
                    return CellType.Wall;
                else
                    return CellType.Path;
            }

            set
            {
                switch (value)
                {
                    case CellType.Path:
                        _isWall = false;
                        break;
                    case CellType.Wall:
                        _isWall = true;
                        break;
                    case CellType.Start:
                        Maze.StartCell = this;
                        break;
                    case CellType.Finish:
                        Maze.FinishCell = this;
                        break;
                }

                // if setting to path or wall, remove start/finish reference
                if (value == CellType.Path || value == CellType.Wall)
                {
                    if (this == Maze.StartCell)
                        Maze.StartCell = null;
                    else if (this == Maze.FinishCell)
                        Maze.FinishCell = null;
                }
            }
        }

        /// <summary>
        /// Is this a Wall or not. All non-Wall types can be moved to.
        /// </summary>
        public bool IsWall { get { return CellType == CellType.Wall; } }

        /// <summary>
        /// Is this cell part of the Start -> Finish Solution Path
        /// </summary>
        public bool IsSolution
        {
            get
            {
                if (ToCell == null)
                    return false;
                else
                    return true;
            }
        }
    }


}
