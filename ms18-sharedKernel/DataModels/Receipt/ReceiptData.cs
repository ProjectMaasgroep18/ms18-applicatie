﻿
namespace Maasgroep.SharedKernel.DataModels.Receipts
{
    public record ReceiptData
    {
        public decimal? Amount { get; set; }
        public long? CostCentreId { get; set; }
        public string? Note { get; set; }
    }
}
