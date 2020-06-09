using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver.Model
{
    class Maze
    {
        public int Rows { get; }
        public int Columns { get; }
        public Cell[,] Cells { get; private set; }
        public bool AllowDiag { get; set; }
        public ISolver Solver { get; private set; }
        // backing fields
        private Cell _startCell;
        private Cell _finishCell;


        /// <summary>
        /// Maze Constructor
        /// </summary>
        /// <param name="rows">number of rows to be created in Maze</param>
        /// <param name="columns">number of columns to be created in Maze</param>
        public Maze(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Cells = new Cell[rows, columns];

            InitializeCells();
            SetSolver("A*");
        }
        
        /// <summary>
        /// The starting cell of the Maze
        /// </summary>
        public Cell StartCell
        {
            get { return _startCell; }
            set
            {
                _startCell = value;

                if (_startCell != null && _startCell == FinishCell)
                    FinishCell = null;
            }
        }

        /// <summary>
        /// The finishing Cell of the Maze
        /// </summary>
        public Cell FinishCell
        {
            get { return _finishCell; }
            set
            {
                _finishCell = value;

                if (_finishCell != null && _finishCell == StartCell)
                    StartCell = null;
            }
        }

        /// <summary>
        /// Indexer to get Cell from Coordinate
        /// </summary>
        public Cell this[Coordinate coordinate]
        {
            get { return Cells[coordinate.Row, coordinate.Column]; }
        }

        /// <summary>
        /// Get all unvisited coordinates that can be accessed from the provided coordinate
        /// </summary>
        public Coordinate[] UnvisitedNeighbours(Coordinate coordinate)
        {
            return AllNeighbours(coordinate).Where(C => !this[C].IsWall && !this[C].Visited).ToArray();
        }

        /// <summary>
        /// Calculate the euclidean distance between two Coordinates in the maze
        /// </summary>
        public double CalcDistance(Coordinate c1, Coordinate c2)
        {
            var r = Math.Pow((c1.Row - c2.Row), 2);
            var c = Math.Pow((c1.Column - c2.Column), 2);

            return Math.Sqrt(r + c);
        }

        /// <summary>
        /// Update the solver being used. Will also clear any progress on current maze solution
        /// </summary>
        /// <param name="algorithmName"></param>
        public void SetSolver(string algorithmName)
        {
            Solver = SolverFactory.CreateSolver(this, algorithmName);
            ResetCells();
        }

        /// <summary>
        /// Resets Solver Progress, but keep the Maze layout (Start, Finish, Walls etc.)
        /// </summary>
        public void Reset()
        {
            ResetCells();
            ResetSolver();
        }

        /// <summary>
        /// initializes the Cell array
        /// </summary>
        private void InitializeCells()
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                {
                    Cells[r, c] = new Cell(this, new Coordinate(r, c));
                }
        }

        /// <summary>
        /// Reinitializes the cell array. Retains Cell.CellType on the new cells
        /// </summary>
        private void ResetCells()
        {
            // if solving has not started, then no need to do anything
            if (!Solver.Started)
                return;

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                {
                    Cell newCell = new Cell(this, new Coordinate(r, c));
                    newCell.CellType = Cells[r, c].CellType;
                    Cells[r, c] = newCell;
                }
        }

        /// <summary>
        /// Resets Solver property to a new instance of currently assigned solver
        /// </summary>
        private void ResetSolver()
        {
            if (Solver.Started)
                Solver = SolverFactory.Reset(Solver);
        }

        /// <summary>
        /// Get all coordinates that can be traversed to
        /// </summary>
        private List<Coordinate> AllNeighbours(Coordinate coordinate)
        {
            List<Coordinate> valid_coordinates = new List<Coordinate>();

            for (int col_offset = -1; col_offset <= 1; col_offset++)
                for (int row_offset = -1; row_offset <= 1; row_offset++)
                {
                    int row = coordinate.Row + row_offset;
                    int column = coordinate.Column + col_offset;
                    Coordinate coordinateToTest = new Coordinate(row, column);

                    if (this.Contains(coordinateToTest)                         // is in Maze
                        && (AllowDiag || col_offset == 0 || row_offset == 0)    // diagonal movement is allowed or is horizontal or is vertical
                        && !(col_offset == 0 && row_offset == 0))               // not the current coordinate
                    {
                        valid_coordinates.Add(coordinateToTest);
                    }
                }

            return valid_coordinates;
        }

        /// <summary>
        /// Tests whether a coordinate is a valid position within the grid
        /// </summary>
        private bool Contains(Coordinate coordinate)
        {
            return coordinate.Row >= 0
                && coordinate.Row < Rows
                && coordinate.Column >= 0
                && coordinate.Column < Columns;
        }
    }
}
