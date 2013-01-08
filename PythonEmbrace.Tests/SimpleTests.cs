using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace PythonEmbrace.Tests
{
    public class SimpleTests
    {
        [Fact]
        public void SingleLine()
        {
            Test(@"
----
self.stream.unget(c)
----
self.stream.unget(c)
----
            ");
        }

        [Fact]
        public void SingleIf()
        {
            Test(@"
----
if c != u';':
    self.stream.unget(c)
----
if c != u';':
{
    self.stream.unget(c)
}
----
            ");
        }

        [Fact]
        public void BracesBeforeBlankLines()
        {
            Test(@"
----
if c != u';':
    self.stream.unget(c)

self.stream.unget(c)
----
if c != u';':
{
    self.stream.unget(c)
}

self.stream.unget(c)
----
            ");
        }

        [Fact]
        public void BlankLinesDoNotAffectIndentationLevel()
        {
            Test(@"
----
if c != u';':

    self.stream.unget(c)
----
if c != u';':
{

    self.stream.unget(c)
}
----
            ");
        }


        [Fact]
        public void BracketsAreImplicitLineContinuation()
        {
            Test(@"
----
if (c != u';' and
        c != ',')
    self.stream.unget(c)
----
if (c != u';' and
        c != ',')
{
    self.stream.unget(c)
}
----
            ");
        }

        [Fact]
        public void BracketsInStringsAreIgnored()
        {
            Test(@"
----
if (c != ')' and
        c != ',')
    self.stream.unget(c)
----
if (c != ')' and
        c != ',')
{
    self.stream.unget(c)
}
----
            ");
        }

        [Fact]
        public void EscapedQuotesDontEndStringsIgnored()
        {
            Test(@"
----
if (c != ')\'' and
        c != ',')
    self.stream.unget(c)
----
if (c != ')\'' and
        c != ',')
{
    self.stream.unget(c)
}
----
            ");
        }

        [Fact]
        public void ExplicitLineContinuation()
        {
            Test(@"
----
if c != \
        u';'
    self.stream.unget(c)
----
if c != \
        u';'
{
    self.stream.unget(c)
}
----
            ");
        }

        private void Test(string test)
        {
            string[] parts = test.Split(new string[] { "----" }, StringSplitOptions.None);

            Assert.Equal(4, parts.Length);
            Assert.Equal("", parts[0].Trim());
            Assert.Equal("", parts[3].Trim());

            string input = parts[1];
            string expected = parts[2];

            string actual = PythonConverter.ConvertString(input);

            Assert.Equal(expected, actual);
        }
    }
}
