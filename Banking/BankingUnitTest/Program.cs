using System;
using System.Threading;

namespace BankingUnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                BankingTest Bank = new BankingTest();
                Bank.BankingDoubleInitialization();
                int option = 0;
                do
                {
                    Console.Clear();
                    Console.WriteLine("----------------------------------------------------------------------");
                    Console.WriteLine("|            Sistema Bancario Banking APP (Testing)                  |");
                    Console.WriteLine("----------------------------------------------------------------------");
                    Console.WriteLine();
                    Console.WriteLine("----------------------------------------------------------------------");
                    Console.WriteLine("|    ¿Que desea realizar.?                                           |");
                    Console.WriteLine("|        1.- Testing SearchCustomerByName                            |");
                    Console.WriteLine("|        2.- Testing FeeBothCustomerInternalAmount50                 |");
                    Console.WriteLine("|        3.- Testing FeeBothCustomerInternalAmount100                |");
                    Console.WriteLine("|        4.- Testing FeeInternalGiverExternalBeneficiaryAmount75     |");
                    Console.WriteLine("|        5.- Testing TransferExternalGiver                           |");
                    Console.WriteLine("|        6.- Testing TransferNegativeAmount                          |");
                    Console.WriteLine("|        7.- Testing TransferExceedBalance                           |");
                    Console.WriteLine("|        8.- Testing TransferInternalGiverExternalBeneficiary        |");
                    Console.WriteLine("|        9.- Testing TransferBothCustomerInternal                    |");
                    Console.WriteLine("|        0.- Salir del Sistema.                                      |");
                    Console.WriteLine("----------------------------------------------------------------------");
                    Console.Write("     ");
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (char.IsDigit(key.KeyChar))
                    {
                        option = int.Parse(key.KeyChar.ToString());
                        switch (option)
                        {
                            case 0:
                                Console.WriteLine("     Cerrando Sistema...");
                                Thread.Sleep(1000);
                                break;
                            case 1:
                                Bank.SearchCustomerByName();
                                break;
                            case 2:
                                Bank.FeeBothCustomerInternalAmount50();
                                break;
                            case 3:
                                Bank.FeeBothCustomerInternalAmount100();
                                break;
                            case 4:
                                Bank.FeeInternalGiverExternalBeneficiaryAmount75();
                                break;
                            case 5:
                                Bank.TransferExternalGiver();
                                break;
                            case 6:
                                Bank.TransferNegativeAmount();
                                break;
                            case 7:
                                Bank.TransferExceedBalance();
                                break;
                            case 8:
                                Bank.TransferInternalGiverExternalBeneficiary();
                                break;
                            case 9:
                                Bank.TransferBothCustomerInternal();
                                break;
                            default:
                                Console.WriteLine("     Por favor seleccione las opciones mostradas: 0 al 9.");
                                Thread.Sleep(3000);
                                option = 10;
                                break;
                        }
                    }
                } while (option != 0);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "; " + ex.InnerException);

                Console.ReadKey();
            }
        }
    }

}
