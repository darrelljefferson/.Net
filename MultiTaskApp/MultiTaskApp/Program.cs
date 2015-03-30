using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultiTaskApp
{
    class Program
    {
        static void Main(string[] args)
        {

            Thread mythread = new Thread(ProcessFile1);
            mythread.Start();
            for (int i = 0; i < 100000; i++)
            {
                Console.WriteLine("Main " + i);
            }
            mythread.Join();
            Thread mythread2 = new Thread(ProcessFile2);
            mythread2.Start();
            for (int i = 0; i < 100000; i++)
            {
                Console.WriteLine("Main " + i);
            }
        }

        static void ProcessFile1()
        {

            for (int i = 0; i < 100000; i++)
            {
                Console.WriteLine("ProcessFile1 " + i);
            }

        }

        static void ProcessFile2()
        {

            for (int i = 0; i < 100000; i++)
            {
                Console.WriteLine("ProcessFile2 " + i);
                printme("hello from ProcessFile 2");
            }

        }

        static void printme(string v1)
        {
            Console.WriteLine(v1);
        }
    }
}
