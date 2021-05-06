using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking
{
    class transfersModel
    {
        public long? Id { get; set; }
        public long? GiverId { get; set; }
        public long? BeneficiaryId { get; set; }
        public string GiverType { get; set; }
        public string BeneficiaryType { get; set; }
        public double Amount { get; set; }
        public double Fees { get; set; }
    }
}
