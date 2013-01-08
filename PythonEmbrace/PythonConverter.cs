using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PythonEmbrace
{
    public static class PythonConverter
    {
        public static string ConvertString(string pythonCode)
        {
            return Convert(pythonCode);
        }

        public static void ConvertFile(string pythonFilePath)
        {
            string outputFilePath =
                Path.Combine(
                    Path.GetDirectoryName(pythonFilePath),
                    Path.GetFileNameWithoutExtension(pythonFilePath) + ".cs"
                );

            File.WriteAllText(outputFilePath, Convert(File.ReadAllText(pythonFilePath)));
        }

        private static string Convert(string pythonCode)
        {
            StringBuilder output = new StringBuilder();

            Stack<int> indentStack = new Stack<int>(new[] { 0 });

            LogicalLine currentLine = null;

            int lineNumber = 0;

            int blankLineCount = 0;

            foreach (string physicalLine in pythonCode.Split('\n'))
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
                            output.AppendLine("{".PadLeft(indentStack.First() + 1));
                            indentStack.Push(currentLine.IndentDepth);
                        }
                        else
                        {
                            while (currentLine.IndentDepth != indentStack.First())
                            {
                                indentStack.Pop();
                                output.AppendLine("}".PadLeft(indentStack.First() + 1));
                            }
                        }

                        while (blankLineCount > 0)
                        {
                            output.AppendLine();
                            blankLineCount--;
                        }

                        output.AppendLine(currentLine.Text);
                    }
                }
            }

            // Close off braces at end of file
            while (indentStack.Count > 1)
            {
                indentStack.Pop();
                output.AppendLine("}".PadLeft(indentStack.First() + 1));
            }

            return output.ToString();
        }

    }
}
