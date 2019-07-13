using System;
using System.Collections.Generic;

namespace BankDeposit.Model.SqlBank
{
    public partial class Idcard
    {
        public Idcard()
        {
            Cards = new HashSet<Cards>();
            Records = new HashSet<Records>();
        }

        public int Icid { get; set; }
        public string Icsex { get; set; }
        public string Icname { get; set; }
        public string Icaddress { get; set; }

        public ICollection<Cards> Cards { get; set; }
        public ICollection<Records> Records { get; set; }
    }
}
