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

        public string Text
        {
            get
            {
                if (_lastSignificantCharIndex != -1)
                {
                    if (_text[_lastSignificantCharIndex] == ':')
                    {
                        // Block start
                        // FIXME remove colon and bracket if necessary
                    }
                    else
                    {
                        // End with a semicolon
                        _text.Insert(_lastSignificantCharIndex + 1, ';');
                    }
                }

                return _text.ToString();
            }
        }

        public LogicalLine(int lineNumber, string physicalLine)
        {
            _lineNumber = lineNumber;
            _text = new StringBuilder(physicalLine.TrimEnd());
            Parse();
        }

        public void Append(string physicalLine)
        {
            Debug.Assert( ! IsComplete );
            _text.AppendLine();
            _text.Append(physicalLine.TrimEnd());
            Parse();
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

        private bool EndsWithLineContinuationCharacter
        {
            get { return _text.Length != 0 && _text[_text.Length - 1] == '\\'; }
        }


        public bool IsComplete
        {
            get { return _bracketBalance == 0 && ! EndsWithLineContinuationCharacter; }
        }

        public bool IsBlank
        {
            get { return IndentDepth == _text.Length; }
        }


        // Characters considered for implicit line continuation - see Python language spec 2.1.6
        private static readonly char[] Opening = new char[] { '{', '(', '[' };
        private static readonly char[] Closing = new char[] { '}', ')', ']' };

        private int _bracketBalance;
        private int _lastSignificantCharIndex; // Index of the last non-whitespace character before any EOL comment

        private void Parse()
        {
            _bracketBalance = 0;
            _lastSignificantCharIndex = -1;

            bool inString = false;
            bool inComment = false;
            char stringStart = '\0';
            char prev = '\0';

            for(int i=0; i < _text.Length; i++)
            {
                char c = _text[i];

                if (inComment)
                {
                    if (c == '\n')
                    {
                        inComment = false;
                    }
                }
                else if (!inString)
                {
                    if (c == '#')
                    {
                        inComment = true;
                    }
                    else if (!Char.IsWhiteSpace(c))
                    {
                        _lastSignificantCharIndex = i;

                        if (c == '"' || c == '\'')
                        {
                            inString = true;
                            stringStart = c;
                        }
                        else if (Opening.Contains(c))
                        {
                            _bracketBalance++;
                        }
                        else if (Closing.Contains(c))
                        {
                            _bracketBalance--;
                        }
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
        }

    }
}
