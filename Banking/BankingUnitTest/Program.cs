using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banking;

namespace BankingUnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Customer customer = new Customer();
            string path = @"D:\Jhonatan Rivero\Documents\GitHub\dotnet-technical-test\customers.csv";
            try
            {
                customer.Initialise(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "; " + ex.InnerException);

                Console.ReadKey();
            }
        }
    }
}
