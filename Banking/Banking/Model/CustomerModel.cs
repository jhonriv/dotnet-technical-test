using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Model
{
    class CustomerModel
    {
        public int CustomerID { get; set; }
        public string  FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public TypeOfCustomer TypeCustomer { get; set; }
        public double Balance  { get; set; }

    }
}
