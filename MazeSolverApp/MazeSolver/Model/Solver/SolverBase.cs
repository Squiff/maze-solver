using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver.Model
{
    /// <summary>
    /// Base Implementation of ISolver
    /// </summary>
    abstract class SolverBase : ISolver
    {
        public Maze Maze { get; }
        public Cell Current { get; protected set; }
        public bool Started { get; protected set; }
        public bool Solved { get; protected set; }
        public bool Complete { get; protected set; }
        public PriorityQueue<Cell> PriorityQueue { get; } = new PriorityQueue<Cell>();

        public SolverBase(Maze maze)
        {
            Maze = maze;
        }

        /// <summary>
        /// Single iteration of pathfinding algorithm to be set by inheriting class
        /// </summary>
        abstract protected void _Iterate();

        public void Iterate()
        {
            Start();

            if (!Complete)
            {
                _Iterate();
            }
        }

        public void Iterate(int iterations)
        {
            Start();

            for (int i = 0; i < iterations; i++)
            {
                if (Complete)
                    break;

                _Iterate();
            }
        }

        public void Solve()
        {
            Start();

            while (!Complete)
            {
                _Iterate();
            }
        }

        /// <summary>
        /// Set Completion status
        /// </summary>
        /// <param name="solved">whether or not the maze was solved</param>
        public void SetComplete(bool solved)
        {
            Complete = true;
            Solved = solved;
            SetForwardPath();
        }

        /// <summary>
        /// Initialize start properties and Validate whether solving can start
        /// </summary>
        private void Start()
        {
            if (Started)
                return;

            if (Maze.StartCell == null)
                throw new InvalidOperationException("Maze Does not have a start point");

            if (Maze.FinishCell == null)
                throw new InvalidOperationException("Maze Does not have a finish point");

            Started = true;
            Current = Maze.StartCell;
        }

        /// <summary>
        /// Set Cell.ToCell Property so solution can be walked from Start to Finish
        /// </summary>
        private void SetForwardPath()
        {
            if (!Solved)
                return;

            Cell cell = Maze.FinishCell;

            while (cell != Maze.StartCell)
            {
                cell.FromCell.ToCell = cell;
                cell = cell.FromCell;
            }
        }
    }
}
