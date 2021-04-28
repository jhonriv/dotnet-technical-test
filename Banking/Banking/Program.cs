using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking
{
    class Program
    {
        static void Main(string[] args)
        {
            Customer customer = new Customer();
            string path = @"D:\Jhonatan Rivero\Documents\GitHub\dotnet-technical-test\customers.csv";
            customer.Initialise(path);
            Console.ReadKey();
        }
    }
}
