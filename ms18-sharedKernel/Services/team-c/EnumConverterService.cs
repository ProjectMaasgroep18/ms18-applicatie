using Maasgroep.SharedKernel.ViewModels.Receipts;

namespace Maasgroep.SharedKernel.Services
{
	public static class EnumConverterService
	{
		public static string ConvertEnumToString(ReceiptStatus receiptStatus)
		{
			return receiptStatus.ToString();
		}

		public static ReceiptStatus ConvertStringToEnum(string receiptStatus)
		{
			ReceiptStatus result;
			if (Enum.TryParse(receiptStatus, out result))
				return result;
			else
				return ReceiptStatus.Onbekend;
		}

	}
}
