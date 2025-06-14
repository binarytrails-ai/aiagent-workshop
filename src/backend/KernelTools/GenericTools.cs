using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace AIAgent.API.KernelTools
{
    /// <summary>
    /// Define Generic Agent functions (tools)
    /// </summary>
    public static class GenericTools
    {
        public static string AgentName => "Generic_Agent"; // Match AgentType.GENERIC

        /// <summary>
        /// This is a placeholder function, for a proper Azure AI Search RAG process.
        /// </summary>
        public static Task<string> DummyFunctionAsync()
        {
            // This is a placeholder function, for a proper Azure AI Search RAG process.
            return Task.FromResult("This is a placeholder function");
        }

        /// <summary>
        /// Returns a dictionary of all public static methods in this class.
        /// </summary>
        public static Dictionary<string, MethodInfo> GetAllKernelFunctions()
        {
            var kernelFunctions = new Dictionary<string, MethodInfo>();
            var methods = typeof(GenericTools).GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
            {
                if (method.Name == nameof(GetAllKernelFunctions))
                    continue;
                kernelFunctions[method.Name] = method;
            }
            return kernelFunctions;
        }
    }
}
