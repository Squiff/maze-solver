﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver.Model
{
    /// <summary>
    /// Evaluates Cells based on the nearest cell to the finish
    /// </summary>
    [AlgorithmName("Nearest First")]
    class NearestFirst : SolverBase
    {

        public NearestFirst(Maze maze) : base(maze) { }

        protected override void _Iterate()
        {
            // get coordinates that can be moved to
            Coordinate[] toEvaluate = Maze.UnvisitedNeighbours(Current.Coordinate);

            foreach (Coordinate c in toEvaluate)
            {
                Cell cell = Maze[c];

                double distanceAdd = Maze.CalcDistance(Current.Coordinate, cell.Coordinate);
                double distanceNew = Current.Distance + distanceAdd;

                if (distanceNew < cell.Distance | cell.Distance == 0)
                {   // new path to cell is shorter. Update cell properties and add to PQ
                    cell.Distance = distanceNew;
                    cell.FromCell = Current;

                    double priority = Maze.CalcDistance(cell.Coordinate, Maze.FinishCell.Coordinate);
                    PriorityQueue.Push(cell, priority);
                }
            }

            // mark current cell as evaluated and get next cell from PQ
            Current.Visited = true;
            Current = PriorityQueue.Pop();

            // review solved/complete flags
            if (Current == Maze.FinishCell)
                SetComplete(true);
            else if (Current == null) // run out of cells to evaluate
                SetComplete(false);
        }
    }
}
