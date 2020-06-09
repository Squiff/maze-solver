using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MazeSolver.ViewModel
{   
    /// <summary>
    /// Base Class with error handling functionality
    /// </summary>
    abstract class ValidationBase : ViewModelBase, INotifyDataErrorInfo
    {
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        public bool HasErrors
        {
            get
            {
                if (_errors.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Raise ErrorsChanged Event
        /// </summary>
        public void OnErrorsChanged(string propertyName)
        {
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Does property have any errors
        /// </summary>
        public bool PropertyHasErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Clear any errors for property
        /// </summary>
        /// <param name="propertyName"></param>
        public void ClearErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
            {
                _errors.Remove(propertyName);
            }
        }

        /// <summary>
        /// Add an error for a property
        /// </summary>
        public void AddError(string propertyName, string errormsg)
        {
            
            if (_errors.ContainsKey(propertyName))
            {   // error already present. Add to existing error list
                _errors[propertyName].Add(errormsg);
            }
            else
            {   // no error present. add new error list to dict
                _errors.Add(propertyName, new List<string>() { errormsg });
            }
        }

        /// <summary>
        /// Validation Method to be overriden by the inheriting class
        /// </summary>
        public abstract void Validate(string propertyName);

        /// <summary>
        /// Get all errors for property
        /// </summary>
        public IEnumerable GetErrors(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName];
            else
                return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Get first error for a property
        /// </summary>
        public string GetFirstError(string propertyName)
        {
            if (_errors.ContainsKey(propertyName))
                return _errors[propertyName][0];
            else
                return string.Empty;
        }

        /// <summary>
        /// Add and Raise a single Error
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="errorMsg"></param>
        public void RaiseError(string propertyName, string errorMsg)
        {
            ClearErrors(propertyName);
            AddError(propertyName, errorMsg);
            OnErrorsChanged(propertyName);
        }

    }
}
