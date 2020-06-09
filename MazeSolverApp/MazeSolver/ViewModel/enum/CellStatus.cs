using System;

namespace MazeSolver.ViewModel
{
    public enum CellStatus
    {
        Path = 0,
        PathVisited = 1,
        Wall = 2,
        Start = 3,
        Finish = 4,
        SolutionPath = 5
    }
}