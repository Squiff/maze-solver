using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MazeSolver.Model;

namespace MazeSolver.ViewModel
{
    /// <summary>
    /// View Model for individual Cells
    /// </summary>
    class CellViewModel : ViewModelBase
    {
        public MazeViewModel MazeViewModel { get; }
        public Coordinate Coordinate { get; }
        
        public ICommand SetCellTypeCommand { get; }

        public CellViewModel(MazeViewModel mazeViewModel, Coordinate coordinate)
        {
            MazeViewModel = mazeViewModel;
            Coordinate = coordinate;
            SetCellTypeCommand = new RelayCommand(SetCellType);
        }

        public MazeOptions MazeOptions
        {
            get { return MazeViewModel.MazeOptions; }
        }

        /// <summary>
        /// Get Cell related to Coordinate property
        /// </summary>
        public Cell Cell 
        { 
            get { return MazeViewModel.Maze[Coordinate]; }
        }

        /// <summary>
        /// Settable Cell Type
        /// </summary>
        public CellTypeVM CellTypeVM
        {   
            set
            {
                CellViewModel refreshCellVM = null;

                switch (value)
                {
                    case CellTypeVM.Path:
                        Cell.CellType = CellType.Path;
                        break;
                    case CellTypeVM.Wall:
                        Cell.CellType = CellType.Wall;
                        break;
                    case CellTypeVM.Start:
                        refreshCellVM = MazeViewModel.StartCellVM;
                        Cell.CellType = CellType.Start;
                        break;
                    case CellTypeVM.Finish:
                        refreshCellVM = MazeViewModel.FinishCellVM;
                        Cell.CellType = CellType.Finish;
                        break;
                }

                // fire status change event for this and related CellVM
                Refresh();

                if (refreshCellVM != null)
                    refreshCellVM.Refresh();
            }
        }

        /// <summary>
        /// Displayed Cell Status
        /// </summary>
        public CellStatus Status
        {
            get
            {
                if (Cell.CellType == CellType.Wall)
                    return CellStatus.Wall;
                else if (Cell.CellType == CellType.Start)
                    return CellStatus.Start;
                else if (Cell.CellType == CellType.Finish)
                    return CellStatus.Finish;
                else if (MazeViewModel.SolutionRevealed && Cell.IsSolution)
                    return CellStatus.SolutionPath;
                else if (Cell.Visited)
                    return CellStatus.PathVisited;
                else
                    return CellStatus.Path;                
            }
        }

        /// <summary>
        /// Is the status one of the starting status types. i.e. path, wall, start or finish
        /// </summary>
        public bool IsInitialStatus
        {
            get
            {
                switch (Status)
                {
                    case CellStatus.Path:
                        return true;
                    case CellStatus.Wall:
                        return true;
                    case CellStatus.Start:
                        return true;
                    case CellStatus.Finish:
                        return true;
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Publish property changed event for Status.
        /// </summary>
        public void Refresh()
        {
            OnPropertyChanged("Status");
        }

        /// <summary>
        /// Set the cell type to the selected option (Wall, Path etc.)
        /// </summary>
        public void SetCellType()
        {
            if (!MazeViewModel.Started)
                CellTypeVM = MazeOptions.SelectedCellType;
        }

    }
}
