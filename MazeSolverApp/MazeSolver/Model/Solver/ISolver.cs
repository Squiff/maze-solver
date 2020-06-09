using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver.Model
{
    /// <summary>
    /// Inerface for classes that are used to solve a Maze
    /// </summary>
    interface ISolver
    {
        Maze Maze { get; }
        Cell Current { get; }
        bool Started { get; } 
        bool Solved { get; }   
        bool Complete { get; }



        /// <summary>
        /// Run a single iteration of a pathfinding algorithm
        /// </summary>
        void Iterate();

        /// <summary>
        /// Run iterations of a pathfinding algorithm
        /// </summary>
        /// <param name="iterations">number of iterations to run</param>
        void Iterate(int iterations);

        /// <summary>
        /// Run Solver until solution found or until completion (with no solution)
        /// </summary>
        void Solve();
    }
}
