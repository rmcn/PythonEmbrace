PythonEmbrace
=============

A tool to help with the manual conversion of Python code to C family languages (C#, Java, Javascript, C++).

It aims to take some of the grunt work out of the manual conversion. Run it on a *valid* python input file to produce an output file with:
* Braces `{}` added, based on Python's indentation rules
* Semicolons `;` added to the end of statements
* Colons `:` removed from the end of lines that begin blocks
* Brackets added to unbracked `if`, `elif`, `for`, `while`, and `except` statements
* `except` changed into `catch`
* `elif` changed into `else if`

Usage: `PythonEmbrace.exe file.py`

Creates a `file.cs` output file in the same directory