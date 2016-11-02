using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DLibrary.Enumerations;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.StringParser
{
    public class ParsedString : IEnumerable<ParsedString.Char>
    {
        private readonly Char[] chars;

        public ParsedString(string str, char escape)
        {
            var buffer = new List<Char>();
            var escaped = false;
            //предлагаю заменить на методы с ref параметрами, подумать, как их использовать
            foreach (var c in str)
            {
                if (!escaped)
                {
                    if (c == escape)
                    {
                        escaped = true;
                    }
                    else
                    {
                        buffer.Add(new Char(c, true));
                    }
                }
                else
                {
                    buffer.Add(new Char(c, false));
                    escaped = false;
                }
            }
            chars = buffer.ToArray();
        }

        public int Length => chars.Length;

        public bool IsOrdinalEqual(string other, int start = 0)
        {
            for (int i = 0; i < other.Length; i++)
            {
                if (!this[i + start].IsOrdinal) return false;
                if (this[i + start].Value != other[i]) return false;
            }
            return true;
        }

        public Char this[int index] => chars[index];
        
        public IEnumerator<Char> GetEnumerator()
        {
            return (IEnumerator<Char>)chars.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return chars.GetEnumerator();
        }

        public override string ToString() => chars.SequenceToString(c => c.Value.ToString(), "", "", "");

        public class Char
        {
            public readonly char Value;
            public readonly bool IsOrdinal;

            public Char(char value, bool isOrdinal)
            {
                Value = value;
                IsOrdinal = isOrdinal;
            }
        }
    }
}