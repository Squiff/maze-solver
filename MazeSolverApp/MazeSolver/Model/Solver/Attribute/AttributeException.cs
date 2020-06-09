using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver.Model
{
    /// <summary>
    /// Custom exception for raising Attribute related errors
    /// </summary>
    class AttributeException : Exception
    {
        public AttributeException(string exceptionText) : base(exceptionText)
        {

        }
    }
}
