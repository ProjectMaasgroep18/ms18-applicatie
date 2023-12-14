﻿namespace Maasgroep.SharedKernel.ViewModels.Receipts
{
	public class ReceiptPhotoModel
	{
		public ReceiptPhotoModel() { }

		public long Id { get; set; }
		public string Base64Image { get; set; }
		public string FileExtension { get; set; }
		public string FileName { get; set; }
		public long ReceiptId { get; set; }
		public long? MemberCreatedId { get; set; }
	}
}
