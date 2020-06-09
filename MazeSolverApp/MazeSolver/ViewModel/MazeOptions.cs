using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeSolver.Model;

namespace MazeSolver.ViewModel
{
    /// <summary>
    /// Container for maze options
    /// </summary>
    class MazeOptions : ValidationBase
    {
        public CellTypeVM[] CellTypeOptions { get; }
        public string[] AlgorithmOptions { get; }
        public int Columns { get; private set; } // set via ColumnsRaw
        public int Rows { get; private set; }    // set via RowsRaw
        // private backing fields
        private string _columnsRaw;
        private string _rowsRaw;
        private CellTypeVM _selectedCellType;
        private string _selectAlgorithm;

        public MazeOptions()
        {
            CellTypeOptions = (CellTypeVM[])Enum.GetValues(typeof(CellTypeVM));
            SelectedCellType = CellTypeVM.Wall;

            AlgorithmOptions = GetAlgorithms();
            SelectedAlgorithm = AlgorithmOptions[0];

            ColumnsRaw = "10";
            RowsRaw = "10";
        }

        /// <summary>
        /// Currently Selected CellType Option
        /// </summary>
        public CellTypeVM SelectedCellType
        {
            get { return _selectedCellType; }
            set
            {
                _selectedCellType = value;
                OnPropertyChanged("SelectedCellType");
            }
        }

        /// <summary>
        /// Currently Selected Algorithm Name
        /// </summary>
        public string SelectedAlgorithm
        {
            get { return _selectAlgorithm; }
            set
            {
                _selectAlgorithm = value;
                OnPropertyChanged("SelectedAlgorithm");
            }
        }

        /// <summary>
        /// Raw Column value bound to UI
        /// </summary>
        public string ColumnsRaw
        {
            get { return _columnsRaw; }
            set 
            { 
                _columnsRaw = value;

                try
                {
                    Columns = int.Parse(value);
                    Validate("ColumnsRaw");
                }
                catch (FormatException)
                {
                    RaiseError("ColumnsRaw", "Maze Column Length is not a valid integer");
                }
                catch (OverflowException)
                {
                    RaiseError("ColumnsRaw", "Maze Column Length should be between 5 and 30");
                }

                OnPropertyChanged("ColumnsRaw");
            }
        }

        /// <summary>
        /// Raw row  value bound to UI
        /// </summary>
        public string RowsRaw
        {
            get { return _rowsRaw; }
            set
            {
                _rowsRaw = value;

                try
                {
                    Rows = int.Parse(value);
                    Validate("RowsRaw");
                }
                catch (FormatException)
                {
                    RaiseError("RowsRaw", "Maze Row Length is not a valid integer");
                }
                catch (OverflowException)
                {
                    RaiseError("RowsRaw", "Maze Row Length should be between 5 and 30");
                }

                OnPropertyChanged("RowsRaw");
            }
        }

        /// <summary>
        /// Can the Maze Dimensions be updated
        /// </summary>
        public bool CanUpdateDimensions 
        { 
            get {
                if (PropertyHasErrors("ColumnsRaw") || PropertyHasErrors("RowsRaw"))
                    return false;
                else
                    return true;
            }
        }

        /// <summary>
        /// Get a list of available solving algorithms
        /// </summary>
        private string[] GetAlgorithms()
        {
            return SolverFactory.GetSolvers().Keys.ToArray();
        }

        /// <summary>
        /// Run validation for supplied property
        /// </summary>
        public override void Validate(string propertyName)
        {
            ClearErrors(propertyName);

            switch (propertyName)
            {
                case "ColumnsRaw":
                    if (Columns < 5 || Columns > 30)
                        AddError(propertyName, "Maze Column Length should be between 5 and 30");
                    break;

                case "RowsRaw":
                    if (Rows < 5 || Rows > 30)
                        AddError(propertyName, "Maze Row Length should be between 5 and 30");
                    break;
            }

            OnErrorsChanged(propertyName);
        }
    }
}
