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
        List<customerModel> Model = new List<customerModel>();
        List<transfersModel> Trans = new List<transfersModel>();
        customerModel giver = new customerModel();
        customerModel beneficiary = new customerModel();

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
                        customerModel c = new customerModel()
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
            SearchCustomer(giverId, beneficiaryId);

            if ((giver.Type == "internal") && (beneficiary.Type == "internal"))
            {
                if (amount <= 100)
                    fees = amount;
                else
                    fees = amount + 5;
            }
            else if ((giver.Type == "internal") && (beneficiary.Type == "externo"))
                fees = amount + 10;

            return fees;
        }

        public long CountCustomers()
        {
            throw new NotImplementedException();
        }

        

        public Customer[] SearchCustomers(long? id = default(long?), string name = null)
        {
            throw new NotImplementedException();
        }

        public void Transfer(long giverId, long beneficiaryId, double amount)
        {
            SearchCustomer(giverId, beneficiaryId);

            if (giver.Type == "interno")
            {
                double monto_fees = CalculateFees(giverId, beneficiaryId, amount);

                if (monto_fees < giver.Balance)
                {
                    giver.Balance -= monto_fees;
                    beneficiary.Balance += amount;
                    transfersModel t = new transfersModel()
                    {
                        Id = Trans[Trans.Count].Id++,
                        GiverId = giverId,
                        BeneficiaryId = beneficiaryId,
                        GiverType = giver.Type,
                        BeneficiaryType = beneficiary.Type,
                        Amount = amount,
                        Fees = monto_fees - amount
                    };
                }
                else
                    Console.WriteLine("El cliente no posee fondos suficientes para realizar la transferencia");
            }
            else
                Console.WriteLine("El cliente no puede realizar la transferencia por ser cliente externo");
        }

        private void SearchCustomer(long giverId, long beneficiaryId)
        {
            foreach (var item in Model)
            {
                if (item.Id == giverId)
                    giver = item;

                if (item.Id == beneficiaryId)
                    beneficiary = item;
            }
        }
    }
}
