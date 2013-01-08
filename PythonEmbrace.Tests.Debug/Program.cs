using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PythonEmbrace.Tests.Debug
{
    /// <summary>
    /// Debug harness for investigating failing unit tests (VS Express sucks)
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            new PythonEmbrace.Tests.SimpleTests().BracketsAreImplicitLineContinuation();
        }
    }
}
