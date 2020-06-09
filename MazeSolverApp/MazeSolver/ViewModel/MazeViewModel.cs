using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using MazeSolver.Model;

namespace MazeSolver.ViewModel
{
    /// <summary>
    /// Primary View Model for Maze Solver App
    /// </summary>
    class MazeViewModel : ValidationBase
    {
        public List<CellViewModel> Cells { get; private set; }
        public MazeOptions MazeOptions { get; } = new MazeOptions();
        public RelayCommand UpdateMazeCommand { get; }
        public RelayCommand StartStopCommand { get; }
        public RelayCommand ResetCommand { get; }
        public RelayCommand ClearCommand { get; }
        // Private/Backing fields
        private CancellationTokenSource _cancelToken;
        private bool _isRunning = false;
        private Maze _maze;
        private Cell _revealCell;
        public bool SolutionRevealed { get; private set; }

        public MazeViewModel()
        {
            Maze = new Maze(MazeOptions.Rows, MazeOptions.Columns);

            UpdateMazeCommand = new RelayCommand(UpdateMazeDimensions);
            StartStopCommand = new RelayCommand(StartStop);
            ResetCommand = new RelayCommand(Reset);
            ClearCommand = new RelayCommand(Clear);
        }

        /// <summary>
        /// Maze object from model
        /// </summary>
        public Maze Maze
        {
            get { return _maze; }
            set
            {
                bool DimensionsChanged = false;

                if (_maze == null || _maze.Rows != value.Rows || _maze.Columns != value.Columns)
                    DimensionsChanged = true;

                _maze = value;
                ResetVMProps();

                if (DimensionsChanged)
                    SetCells();

                OnPropertyChanged("Maze");
            }
        }

        /// <summary>
        /// Number of columns in the maze
        /// </summary>
        public int Columns
        {
            get { return Maze.Columns; }
        }

        /// <summary>
        /// number of rows in the maze
        /// </summary>
        public int Rows
        {
            get { return Maze.Rows; }
        }

        /// <summary>
        /// The CellViewModel that contains the maze start cell
        /// </summary>
        public CellViewModel StartCellVM
        {
            get 
            {
                if (Maze.StartCell == null)
                    return null;
                else
                    return this[Maze.StartCell.Coordinate]; 
            }

            set { Maze.StartCell = value.Cell; }
        }

        /// <summary>
        /// The CellViewModel that contains the maze finish cell
        /// </summary>
        public CellViewModel FinishCellVM
        {
            get 
            {
                if (Maze.FinishCell == null)
                    return null;
                else
                    return this[Maze.FinishCell.Coordinate]; 
            }

            set { Maze.FinishCell = value.Cell; }
        }

        /// <summary>
        /// Has the solver finished (Does not indicate whether a solution path was found)
        /// </summary>
        public bool Complete
        {
            get { return Maze.Solver.Complete; }
        }

        /// <summary>
        /// Is the solver currently in progress
        /// </summary>
        public bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                _isRunning = value;
                OnPropertyChanged("IsRunning");
            }
        }

        /// <summary>
        /// ISolver being used to solve the maze
        /// </summary>
        public ISolver Solver
        {
            get { return Maze.Solver; }
        }

        /// <summary>
        /// Has the solver started
        /// </summary>
        public bool Started
        {
            get { return Solver.Started; }
        }

        /// <summary>
        /// Show validation error when attempting to start solving. Set to Null to clear errors.
        /// </summary>
        public string StartSolvingValidation
        {
            get { return GetFirstError("StartSolvingValidation"); }

            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    ClearErrors("StartSolvingValidation");
                    OnPropertyChanged("StartSolvingValidation");
                }
            }
        }

        /// <summary>
        /// Indexer for retrieving the CellViewModel for the referenced Coordinate
        /// </summary>
        public CellViewModel this[Coordinate coordinate]
        {
            get
            {
                // MazeViewModel uses a flat list instead of a 2d array as in the model
                int position = (coordinate.Row * Columns) + coordinate.Column;
                return Cells[position];
            }
        }

        /// <summary>
        /// Initialize Cells list. Populated with a CellViewModel for each cell in the Maze
        /// </summary>
        private void SetCells()
        {
            Cells = new List<CellViewModel>();

            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Columns; c++)
                {
                    Cells.Add(new CellViewModel(this, new Coordinate(r,c)));
                }

            OnPropertyChanged("Cells");
            OnPropertyChanged("Columns");
            OnPropertyChanged("Rows");
        }

        /// <summary>
        /// Starts or stops the solver depending on it's current state
        /// </summary>
        public void StartStop()
        {
            if (!IsRunning)
                Start();
            else
                _cancelToken.Cancel();
        }

        /// <summary>
        /// Start Solving
        /// </summary>
        private async void Start()
        {
            if (!Started) // if first time starting the solver
            {
                Validate("StartSolvingValidation");

                if (PropertyHasErrors("StartSolvingValidation"))
                    return;

                // lock in the select algorithm
                Maze.SetSolver(MazeOptions.SelectedAlgorithm);
            }

            _cancelToken = new CancellationTokenSource();
            IsRunning = true;

            await Solve(_cancelToken.Token);

            IsRunning = false;
        }

        /// <summary>
        /// Iterate the solver until complete or cancelled
        /// </summary>
        /// <param name="cancellationToken">Cancel token to stop iterating</param>
        public async Task Solve(CancellationToken cancellationToken)
        {
            while (!Complete)
            {
                Cell currentCell = Solver.Current;

                Maze.Solver.Iterate(); // run single iteration

                if (currentCell != null)
                    this[currentCell.Coordinate].Refresh(); // refresh visited cell

                await Task.Delay(200);

                if (cancellationToken.IsCancellationRequested)
                    return;
            }

            await RevealSolutionPath(cancellationToken);
        }

        /// <summary>
        /// Walk the solution path from start to finish
        /// </summary>
        private async Task RevealSolutionPath(CancellationToken cancellationToken)
        {   
            if (!Solver.Solved) // no valid solution, do nothing.
                return;

            if (_revealCell == null)
                _revealCell = Maze.StartCell;

            SolutionRevealed = true;

            while (_revealCell != Maze.FinishCell)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                this[_revealCell.Coordinate].Refresh();
                _revealCell = _revealCell.ToCell;

                await Task.Delay(200);
            }
        }

        /// <summary>
        /// Update the current Maze Dimensions
        /// </summary>
        public void UpdateMazeDimensions()
        {
            if (!MazeOptions.CanUpdateDimensions || IsRunning)
                return;

            // only run if selected dimensions are different to current dimensions
            if (Maze.Rows != MazeOptions.Rows || Maze.Columns != MazeOptions.Columns)
                Maze = new Maze(MazeOptions.Rows, MazeOptions.Columns);
        }

        /// <summary>
        /// Reset the Solver, but retain maze layout (Start, finish, Wall etc.)
        /// </summary>
        public void Reset()
        {
            if (IsRunning)
                _cancelToken.Cancel();

            // get cells that do not have a starting status and Will need to be refreshed after the reset
            CellViewModel[] toRefresh = Cells.Where(cell => !cell.IsInitialStatus).ToArray();

            Maze.Reset();
            ResetVMProps();

            foreach (CellViewModel cellViewModel in toRefresh)
                cellViewModel.Refresh();

        }

        /// <summary>
        /// Completly clear the maze
        /// </summary>
        public void Clear()
        {
            if (IsRunning)
                return;

            // all non-path cells will need to be refreshed
            CellViewModel[] toRefresh = Cells.Where(cell => cell.Status != CellStatus.Path).ToArray();

            Maze = new Maze(Maze.Rows, Maze.Columns);

            foreach (CellViewModel cellViewModel in toRefresh)
                cellViewModel.Refresh();
        }

        /// <summary>
        /// Resets properties that are linked to the current solution
        /// </summary>
        private void ResetVMProps()
        {
            StartSolvingValidation = string.Empty;
            _revealCell = null;
            SolutionRevealed = false;
        }

        /// <summary>
        /// Run Validation on supplied property
        /// </summary>
        public override void Validate(string propertyName)
        {
            ClearErrors(propertyName);

            switch (propertyName)
            {
                case "StartSolvingValidation":
                    if (StartCellVM == null)
                        AddError("StartSolvingValidation", "No Start Point is Set");

                    if (FinishCellVM == null)
                        AddError("StartSolvingValidation", "No Finish Point is Set");

                    OnPropertyChanged("StartSolvingValidation");
                    break;
            }
        }

    }
}
