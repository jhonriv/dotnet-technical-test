using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using System.Threading;

namespace Banking
{
    public class Customer : IBanking
    {
        public string Name;
        public long? Id;
        public List<CustomerModel> Model = new List<CustomerModel>();
        List<CustomerModel> Persons = new List<CustomerModel>();
        List<transfersModel> Trans = new List<transfersModel>();
        CustomerModel giver = new CustomerModel();
        CustomerModel beneficiary = new CustomerModel();
        

        public void Initialise(string path)
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
                {
                    var result = reader.AsDataSet();
                    DataTable data = result.Tables[0];
                    for (int i = 1; i < data.Rows.Count; i++)
                    {
                        CustomerModel c = new CustomerModel()
                        {
                            Id = Convert.ToInt32(data.Rows[i][0]),
                            First_name = data.Rows[i][1].ToString(),
                            Last_name = data.Rows[i][2].ToString(),
                            Company_name = data.Rows[i][3].ToString(),
                            Address = data.Rows[i][4].ToString(),
                            City = data.Rows[i][5].ToString(),
                            County = data.Rows[i][6].ToString(),
                            State = data.Rows[i][7].ToString(),
                            Zip = data.Rows[i][8].ToString(),
                            Phone1 = data.Rows[i][9].ToString(),
                            Phone2 = data.Rows[i][10].ToString(),
                            Email = data.Rows[i][11].ToString(),
                            Web = data.Rows[i][12].ToString(),
                            Type = data.Rows[i][13].ToString(),
                            Balance = Convert.ToDouble(data.Rows[i][14])
                        };
                        Model.Add(c);
                    }
                }
            }
        }

        public double CalculateFees(long giverId, long beneficiaryId, double amount)
        {
            double fees = 0;
            giver = SearchCustomer(giverId)[0];
            beneficiary = SearchCustomer(beneficiaryId)[0];

            if ((giver.Type == "internal") && (beneficiary.Type == "internal"))
            {
                if (amount < 100)
                    fees = 0;
                else
                    fees = 5;
            }
            else if ((giver.Type == "internal") && (beneficiary.Type == "external"))
                fees = 10;

            return fees;
        }

        public long CountCustomers()
        {
            return Model.Count;
        }
                
        public void Transfer(long giverId, long beneficiaryId, double amount)
        {
            try
            {
                Thread.Sleep(4000);
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("----------------------------------------------------------------------------");
                Console.WriteLine("|                   Resultados de la Transacción:                          |");
                Console.WriteLine("----------------------------------------------------------------------------");
                if (amount > 0)
                {
                    giver = SearchCustomer(giverId).Count > 0 ? SearchCustomer(giverId)[0] : null;
                    beneficiary = SearchCustomer(beneficiaryId).Count > 0 ? SearchCustomer(beneficiaryId)[0] : null;
                    if (giver.Type == "internal")
                    {
                        double monto_fees = CalculateFees(giverId, beneficiaryId, amount);
                        long? id;

                        if ((monto_fees + amount) < giver.Balance)
                        {
                            double saldo = giver.Balance;
                            giver.Balance -= (monto_fees + amount);
                            beneficiary.Balance += amount;
                            if (Trans.Count == 0)
                                id = 1;
                            else
                                id = Trans.Count + 1;
                            transfersModel t = new transfersModel()
                            {
                                Id = id,
                                GiverId = giverId,
                                BeneficiaryId = beneficiaryId,
                                GiverType = giver.Type,
                                BeneficiaryType = beneficiary.Type,
                                Amount = amount,
                                Fees = monto_fees
                            };
                            Trans.Add(t);
                            Console.WriteLine("|    La Transferencia fue realizada correctamente...                       |");
                            Console.WriteLine("|" + em(75, "|", 'D'));
                            Console.WriteLine("|    Depositante: " + em(27, giver.First_name + " " + giver.Last_name, 'I')
                            + em(29, " Saldo: " + String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", saldo), 'D') + " |");
                            Console.WriteLine("|" + em(75, "|", 'D'));
                            Console.WriteLine("|    Beneficiario: " + em(25, beneficiary.First_name + " " + beneficiary.Last_name, 'I')
                                + em(30, " Transferido: +" + String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", amount), 'D') + " |");
                            Console.WriteLine("|" + em(75, "|", 'D'));
                            Console.WriteLine("|    Comisión Bancaria: "
                                + em(50, String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", monto_fees), 'D') + " |");
                            Console.WriteLine("|" + em(75, "----------------- |",'D'));
                            Console.WriteLine("|" + em(75, "|", 'D'));
                            Console.WriteLine("|    Saldo del depositante al finalizar la transacción: "
                                + em(18, String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", giver.Balance), 'D') + " |");
                        }
                        else
                            Console.WriteLine("| El cliente no posee fondos suficientes para realizar la transferencia    |");
                    }
                    else
                        Console.WriteLine("|    El cliente no puede realizar la transferencia por ser cliente externo.|");
                }
                else
                    Console.WriteLine("|    No se puede realizar operaciones con monto menor o igual que cero.    |");

                Console.WriteLine("----------------------------------------------------------------------------");
                Console.WriteLine("Presione cualquier tecla para volver atras");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al realizar transferencia: " + ex.Message + " " + ex.InnerException);
            }
        }

        public List<CustomerModel> SearchCustomer(long? id = default, string name = null)
        {
            Persons = new List<CustomerModel>();
            foreach (var item in Model)
            {
                if ((id != default(long?)) && (item.Id == id))
                    Persons.Add(item);
                else if (name != null)
                    if ((item.First_name.ToLower().IndexOf(name.ToLower()) >= 0) |
                        (item.Last_name.ToLower().IndexOf(name.ToLower()) >= 0))
                        Persons.Add(item);
            }
            return Persons;
        }

        public Customer[] SearchCustomers(long? id = null, string name = null)
        {
            throw new NotImplementedException();
        }

        public string em(int large, string var, char ori)
        {
            if (var.Length > large)
            {
                var = var.Substring(0, large - 2);
                var += "  ";
            }
            else
            {
                int space = large - var.Length;
                int centrar; bool plus = false;
                if (space % 2 != 0)
                    plus = true;

                centrar = space / 2;
                string sp = "";

                if (ori == 'C')
                {
                    for (int i = 0; i < centrar; i++)
                    {
                        sp += " ";
                    }
                    var = sp + var + sp + (plus ? " " : "");
                }
                else
                {
                    for (int i = 0; i < space; i++)
                    {
                        sp += " ";
                    }
                    if (ori == 'I')
                        var += sp;
                    else
                        var = sp + var;
                }
            }

            return var;
        }
    }
}

