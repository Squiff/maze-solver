using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver.Model
{

    /// <summary>
    /// Attribute to assign a name to Solvers. Used by SolverFactory to create Solver classes
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class)]
 
    class AlgorithmNameAttribute : Attribute
    {
        public string Name;

        public AlgorithmNameAttribute(string name)
        {
            Name = name;
        }
    }

}
