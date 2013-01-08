using System;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace PythonEmbrace
{
    class LogicalLine
    {
        private int _lineNumber;
        private StringBuilder _text;

        public string Text { get { return _text.ToString(); } }

        public LogicalLine(int lineNumber, string physicalLine)
        {
            _lineNumber = lineNumber;
            _text = new StringBuilder(physicalLine.TrimEnd());
        }

        public void Append(string physicalLine)
        {
            Debug.Assert( ! IsComplete );
            _text.AppendLine();
            _text.Append(physicalLine.TrimEnd());
        }

        public int IndentDepth
        {
            get
            {
                int i = 0;
                while (i < _text.Length && Char.IsWhiteSpace(_text[i]))
                {
                    i++;
                }
                return i;
            }
        }

        public bool IsComplete
        {
            get { return BracketBalance == 0 && Text.LastOrDefault() != '\\'; }
        }

        public bool IsBlank
        {
            get { return IndentDepth == _text.Length; }
        }


        // Characters considered for implicit line continuation - see Python language spec 2.1.6
        private static readonly char[] Opening = new char[] { '{', '(', '[' };
        private static readonly char[] Closing = new char[] { '}', ')', ']' };

        private int BracketBalance
        {
            get
            {
                int balance = 0;
                bool inString = false;
                char stringStart = '\0';
                char prev = '\0';

                foreach (char c in Text)
                {
                    if (!inString)
                    {
                        if (c == '"' || c == '\'')
                        {
                            inString = true;
                            stringStart = c;
                        }
                        else if (Opening.Contains(c))
                        {
                            balance++;
                        }
                        else if (Closing.Contains(c))
                        {
                            balance--;
                        }
                    }
                    else
                    {
                        if (c == stringStart && prev != '\\')
                        {
                            inString = false;
                        }
                    }

                    if (c == '\\' && prev == '\\')
                    {
                        prev = '\0';
                    }
                    else
                    {
                        prev = c;
                    }
                }
                return balance;
            }
        }

    }
}
