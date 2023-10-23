﻿using Maasgroep.Database.Interfaces;

namespace Maasgroep.Database.Models
{
    public class Receipt : IReceipt
    {
        public long Id { get; set; }
        public decimal? Amount { get; set; }
        public ICostCentre? CostCentre { get; set; }
        public IReceiptStatus ReceiptStatus { get; set; }
        public string? Location { get; set; }
        public string? Note { get; set; }
    }
}
