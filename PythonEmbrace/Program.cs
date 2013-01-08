using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PythonEmbrace
{
    class Program
    {
        static void Main(string[] args)
        {
            PythonConverter.ConvertFile(args[0]);
        }
    }
}
