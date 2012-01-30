using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using MediaVF.Services.Core;

namespace MediaVF.Services.Console
{
    class Program
    {
        class TestShell : DependencyObject, IShell
        {
            public void Initialize()
            {
            }
        }

        static void Main(string[] args)
        {
            ServiceBootstrapper<TestShell> bootstrapper = new ServiceBootstrapper<TestShell>();
            bootstrapper.Run();

            System.Console.WriteLine("Hit [ENTER] to quit.");

            while (System.Console.ReadKey().Key != ConsoleKey.Enter) ;
        }
    }
}
