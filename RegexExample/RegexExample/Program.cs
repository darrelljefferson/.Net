using System;
using System.Collections.Generic;
using System.Text;

namespace RegexExample
{
    class Program
    {
        static void Main(string[] args)
        {
            RegexFunctions regex = new RegexFunctions();
            bool foo = regex.EmailRegex("foo@foo.com","strict");
            Console.WriteLine(foo.ToString());

        }
    }
}
