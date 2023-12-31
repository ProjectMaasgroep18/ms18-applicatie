﻿using Maasgroep.SharedKernel.ViewModels.Admin;

namespace Maasgroep.SharedKernel.ViewModels.Receipts
{
    public record ReceiptApprovalModel
    {
        public long Id { get; set; }
        public long ReceiptId { get; set; }
        public string? Note { get; set; }
        public bool Approved { get; set; }
        public bool Paid { get; set; }
        public MemberModel? MemberCreated { get; set; }
        public DateTime? DateTimeCreated { get; set; }
    }
}
