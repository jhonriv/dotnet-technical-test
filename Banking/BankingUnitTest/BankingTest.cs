using Microsoft.VisualStudio.TestTools.UnitTesting;
using Banking;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BankingUnitTest
{
    [TestClass]
    public class BankingTest : Customer
    {
        private readonly Random _random = new Random();

        [TestMethod]
        public void BankingDoubleInitialization()
        {
            bool InstanceOpen;
            using (Mutex mutex = new Mutex(true, Process.GetCurrentProcess().ProcessName, out InstanceOpen))
            {
                if (InstanceOpen)
                {
                    string path = @"D:\Jhonatan Rivero\Documents\GitHub\dotnet-technical-test\customers.csv";
                    Initialise(path);
                }
                else
                {
                    Console.WriteLine("Solo es posible ejecutar una instancia de Banking.");
                    Console.ReadKey();
                }
            }
        }

        [TestMethod]
        public void SearchCustomerByName()
        {
            List<CustomerModel> customers = new List<CustomerModel>();
            Console.Write(" Nombre a buscar: ");
            Thread.Sleep(2000);
            Console.WriteLine("meaghan");
            Thread.Sleep(1000);
            string name = "meaghan"; // Console.ReadLine();
            customers = SearchCustomer(null, name);
            if (customers.Count > 0)
            {
                string option = "0";
                do
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("Los siguientes clientes han sido encontrados...");
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine("|  Id   |        Nombre Completo        |");
                    Console.WriteLine("-----------------------------------------");
                    for (int i = 0; i < customers.Count; i++)
                    {
                        Console.WriteLine("|" + em(7, customers[i].Id.ToString(), 'C') + "|" + em(31, customers[i].First_name + " " + customers[i].Last_name, 'C') + "|");
                    }
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine();

                    Console.Write("Escriba el ID del cliente que desea visualizar detalladamente. o 0 para salir: ");
                    option = Console.ReadLine();
                    if ((option != "0") && (option.All(Char.IsDigit)))
                    {
                        bool find = false;
                        foreach (var item in customers)
                        {
                            if (item.Id.ToString() == option)
                            {
                                find = true;
                                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                                Console.WriteLine("|  id   |        Nombre Completo        |  Telefono 1  |  Telefono 2  |   Tipo de Cliente |           Saldo          |");
                                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                                Console.WriteLine("|" + em(7, item.Id.ToString(), 'C') + "|" + em(31, item.First_name + " " + item.Last_name, 'C') + "|"
                                    + em(14, item.Phone1, 'C') + "|" + em(14, item.Phone2, 'C') + "|" + em(19, item.Type, 'C') + "|" 
                                    + em(26, String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", item.Balance), 'C') + "|");
                                Console.WriteLine("----------------------------------------------------------------------------------------------------------------------");
                                Console.WriteLine("Presione cualquier tecla para volver atras");
                                Console.ReadKey();
                            }

                            if (!find)
                            {
                                Console.WriteLine("El Id escrito no está visualizado.");
                                Thread.Sleep(3000);
                            }
                        }
                    }
                    else if (option != "0")
                    {
                        Console.WriteLine("Por favor escriba solo el número correspondiente al id.");
                        Thread.Sleep(3000);
                    }
                } while (option != "0");
            }
        }

        [TestMethod]
        public void FeeBothCustomerInternalAmount50()
        {
            List<CustomerModel> customers = Model.Where(x => x.Type == "internal").ToList();
            CustomerModel giver = customers[_random.Next(customers.Count)];
            CustomerModel beneficiary = customers[_random.Next(customers.Count)];
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(giver.First_name + " " + giver.Last_name + " envía a " + beneficiary.First_name + " " + beneficiary.Last_name + " la cantidad de: 50.00. ");
            double fees = CalculateFees((long)giver.Id, (long)beneficiary.Id, 50);
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("|    El Monto a transferir:" + em(41, String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", 50), 'D') + "  |");
            Console.WriteLine("|    El costo de comisión para esta transacción es:" + em(17, String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", fees), 'D') + "  |");
            Console.WriteLine("|    Monto total de transferencia:" + em(34, String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", (fees + 50)), 'D') + "  |");
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Presione cualquier tecla para volver atras");
            Console.ReadKey();
        }

        [TestMethod]
        public void FeeBothCustomerInternalAmount100()
        {
            List<CustomerModel> customers = Model.Where(x => x.Type == "internal").ToList();
            CustomerModel giver = customers[_random.Next(customers.Count)];
            CustomerModel beneficiary = customers[_random.Next(customers.Count)];
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(giver.First_name + " " + giver.Last_name + " envía a " + beneficiary.First_name + " " + beneficiary.Last_name + " la cantidad de: 100.00. ");
            double fees = CalculateFees((long)giver.Id, (long)beneficiary.Id, 100);
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("|    El Monto a transferir:" + em(41, String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", 100), 'D') + "  |");
            Console.WriteLine("|    El costo de comisión para esta transacción es:" + em(17, String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", fees), 'D') + "  |");
            Console.WriteLine("|    Monto total de transferencia:" + em(34, String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", (fees + 100)), 'D') + "  |");
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Presione cualquier tecla para volver atras");
            Console.ReadKey();
        }

        [TestMethod]
        public void FeeInternalGiverExternalBeneficiaryAmount75()
        {
            List<CustomerModel> customers = Model.Where(x => x.Type == "internal").ToList();
            CustomerModel giver = customers[_random.Next(customers.Count)];
            customers = Model.Where(x => x.Type == "external").ToList();
            CustomerModel beneficiary = customers[_random.Next(customers.Count)];
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine(giver.First_name + " " + giver.Last_name + " envía a " + beneficiary.First_name + " " + beneficiary.Last_name + " la cantidad de: 75.00. ");
            double fees = CalculateFees((long)giver.Id, (long)beneficiary.Id, 75);
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("|    El Monto a transferir:" + em(41, String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", 75), 'D') + "  |");
            Console.WriteLine("|    El costo de comisión para esta transacción es:" + em(17, String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", fees), 'D') + "  |");
            Console.WriteLine("|    Monto total de transferencia:" + em(34, String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", (fees + 75)), 'D') + "  |");
            Console.WriteLine("-----------------------------------------------------------------------");
            Console.WriteLine("Presione cualquier tecla para volver atras...");
            Console.ReadKey();
        }

        [TestMethod]
        public void TransferExternalGiver()
        {
            List<CustomerModel> customers = Model.Where(x => x.Type == "external").ToList();
            CustomerModel giver = customers[_random.Next(customers.Count)];
            CustomerModel beneficiary = customers[_random.Next(customers.Count)];
            Console.WriteLine("     " + giver.First_name + " " + giver.Last_name + " envía a " + beneficiary.First_name + " " + beneficiary.Last_name + " la cantidad de: 100.00. ");
            Transfer((long)giver.Id, (long)beneficiary.Id, 100);
            Console.ReadKey();
        }


        [TestMethod]
        public void TransferNegativeAmount()
        {
            List<CustomerModel> customers = Model.Where(x => x.Type == "internal").ToList();
            CustomerModel giver = customers[_random.Next(customers.Count)];
            CustomerModel beneficiary = customers[_random.Next(customers.Count)];
            Console.WriteLine("     " + giver.First_name + " " + giver.Last_name + " envía a " + beneficiary.First_name + " " + beneficiary.Last_name + " la cantidad de: -25.00. ");
            Transfer((long)giver.Id, (long)beneficiary.Id, -25);
            Console.ReadKey();
        }

        [TestMethod]
        public void TransferExceedBalance()
        {
            List<CustomerModel> customers = Model.Where(x => x.Type == "internal").ToList();
            CustomerModel giver = customers[_random.Next(customers.Count)];
            CustomerModel beneficiary = customers[_random.Next(customers.Count)];
            Console.WriteLine("     " + giver.First_name + " " + giver.Last_name + " envía a " + beneficiary.First_name + " " + beneficiary.Last_name + " la cantidad de: 1000.00. ");
            Transfer((long)giver.Id, (long)beneficiary.Id, 1000);
            Console.ReadKey();
        }


        [TestMethod]
        public void TransferInternalGiverExternalBeneficiary()
        {
            List<CustomerModel> customers = Model.Where(x => x.Type == "internal").ToList();
            CustomerModel giver = customers[_random.Next(customers.Count)];
            customers = Model.Where(x => x.Type == "external").ToList();
            CustomerModel beneficiary = customers[_random.Next(customers.Count)];
            Console.WriteLine("     " + giver.First_name + " " + giver.Last_name + " envía a " + beneficiary.First_name + " " + beneficiary.Last_name + " la cantidad de: 75.00. ");
            Transfer((long)giver.Id, (long)beneficiary.Id, 75);
            Console.ReadKey();
        }

        [TestMethod]
        public void TransferBothCustomerInternal()
        {
            List<CustomerModel> customers = Model.Where(x => x.Type == "internal").ToList();
            CustomerModel giver = customers[_random.Next(customers.Count)];
            CustomerModel beneficiary = customers[_random.Next(customers.Count)];
            Console.WriteLine("     " + giver.First_name + " " + giver.Last_name + " envía a " + beneficiary.First_name + " " + beneficiary.Last_name + " la cantidad de: 50.00. ");
            Transfer((long)giver.Id, (long)beneficiary.Id, 50);
            Console.ReadKey();
        }
    }
}
