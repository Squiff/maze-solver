using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MazeSolver.Model
{
    /// <summary>
    /// Factory for Creating Solver Classes
    /// </summary>
    static class SolverFactory
    {
        private static Dictionary<string, Type> _solverDict;

        static SolverFactory()
        {
            _solverDict = GetAlgorithmNames();
        }

        /// <summary>
        /// Get alorithm name to Type mapping
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Type> GetSolvers()
        {
            return _solverDict;
        }

        /// <summary>
        /// Create solver from provided algorithm name
        /// </summary>
        /// <param name="maze">The maze reference to pass to the solver constructor</param>
        /// <param name="algorithmName">The Algorithm name specified by the AlgorithmNameAttribute on the solver</param>
        /// <returns></returns>
        public static ISolver CreateSolver(Maze maze, string algorithmName)
        {
            if (!_solverDict.ContainsKey(algorithmName))
                throw new ArgumentException("Unknown algorithm name provided");

            Type solverType = _solverDict[algorithmName];

            // create solver - Constructor should only require the maze reference
            var constructorArgs = new object[] { maze };
            ISolver solver = (ISolver)Activator.CreateInstance(solverType, constructorArgs);

            return solver;
        }

        /// <summary>
        /// Get a new instance of the provided solver
        /// </summary>
        public static ISolver Reset(ISolver solver)
        {
            string algorithmName = GetAlgorithmName(solver.GetType());

            return CreateSolver(solver.Maze, algorithmName);
        }

        /// <summary>
        /// Get all classes that implement ISolver and have AlgorithmNameAttribute  
        /// </summary>
        public static Dictionary<string, Type> GetAlgorithmNames()
        {
            var matches = new List<KeyValuePair<string, Type>>();

            // get types that implement ISolver
            IEnumerable<Type> solvers = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.GetInterfaces().Contains(typeof(ISolver)));

            // Get algorithm Name Attribute
            foreach (var solver in solvers)
            {
                string algorithmName = GetAlgorithmName(solver);
                if (algorithmName != null)
                    matches.Add(new KeyValuePair<string, Type>(algorithmName, solver));
            }

            // check for duplicate algorithm Name
            var duplicates = matches.GroupBy(x => x.Key)
                                    .Where(group => group.Count() > 1)
                                    .Select(group => group.Key);

            if (duplicates.Count() > 0)
                throw new AttributeException($"Duplicate Algorithm Name Found: {duplicates.ElementAt(0)}");

            // No Duplicates. Add to dictionary
            var returnDict = new Dictionary<string, Type>();

            foreach (var m in matches)
            {
                returnDict.Add(m.Key, m.Value);
            }

            return returnDict;
        }

        /// <summary>
        /// Get Algorithm Name or Null if no attribute found
        /// </summary>
        private static string GetAlgorithmName(Type solverType)
        {
            Type algorithmType = typeof(AlgorithmNameAttribute);
            AlgorithmNameAttribute algorithmNameAtt = (AlgorithmNameAttribute)solverType.GetCustomAttribute(algorithmType);

            if (algorithmNameAtt != null)
                return algorithmNameAtt.Name;
            else
                return null;
        }

    }
}
