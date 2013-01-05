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
            Convert(args[0]);
        }

        static void Convert(string pythonFilePath)
        {
            string outputFilePath =
                Path.Combine(
                    Path.GetDirectoryName(pythonFilePath),
                    Path.GetFileNameWithoutExtension(pythonFilePath) + ".cs"
                );

            Stack<int> indentStack = new Stack<int>( new [] {0} );

            LogicalLine currentLine = null;

            int lineNumber = 0;

            int blankLineCount = 0;

            using (var sw = new StreamWriter(outputFilePath))
            {
                foreach (string physicalLine in File.ReadAllLines(pythonFilePath))
                {
                    lineNumber++;

                    if (currentLine == null || currentLine.IsComplete)
                    {
                        currentLine = new LogicalLine(lineNumber, physicalLine);
                    }
                    else
                    {
                        currentLine.Append(physicalLine);
                    }

                    if (currentLine.IsComplete)
                    {
                        if (currentLine.IsBlank)
                        {
                            // Blank lines have no effect on indentation levels
                            // Store them up, rather than rendering inline, because we want them to appear after any braces
                            blankLineCount++;
                        }
                        else
                        {
                            if (currentLine.IndentDepth > indentStack.First())
                            {
                                sw.WriteLine("{".PadLeft(indentStack.First() + 1));
                                indentStack.Push(currentLine.IndentDepth);
                            }
                            else
                            {
                                while (currentLine.IndentDepth != indentStack.First())
                                {
                                    indentStack.Pop();
                                    sw.WriteLine("}".PadLeft(indentStack.First() + 1));
                                }
                            }

                            while (blankLineCount > 0)
                            {
                                sw.WriteLine();
                                blankLineCount--;
                            }

                            sw.WriteLine(currentLine.Text);
                            sw.Flush();
                        }
                    }
                }

                // Close off braces at end of file
                while (indentStack.Count > 1)
                {
                    indentStack.Pop();
                    sw.WriteLine("}".PadLeft(indentStack.First() + 1));
                }
            }
        }

    }
}
