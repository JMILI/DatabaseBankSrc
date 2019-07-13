using System;
using System.Collections.Generic;

namespace BankDeposit.Model.SqlBank
{
    public partial class Transferrecords
    {
        public int Tid { get; set; }
        public int TpartyAcid { get; set; }
        public string TpartyAname { get; set; }
        public int TpartyBcid { get; set; }
        public string TpartyBname { get; set; }
        public double? TtransferBalance { get; set; }
        public DateTime? TtransferTime { get; set; }
        public int? Tmid { get; set; }

        public Cards TpartyAc { get; set; }
        public Cards TpartyBc { get; set; }
    }
}
