namespace BarCodeDemo.Api.DataModel
{
	public interface IBillDataModel
	{
		decimal? Amount { get; set; }
		string Currency { get; set; }
		string IBAN { get; set; }


		/// <inheritdoc cref="Format"/>
		GraphicsFormat GraphicsFormat { get; set; }
		
		/// <inheritdoc cref="Language"/>
		Language Language { get; set; }

		/// <inheritdoc cref="OutputSize"/>
		OutputSize OutputSize { get; set; }


		/// <inheritdoc cref="IBillParticipant"/>
		IBillParticipant Payee { get; set; }

		/// <inheritdoc cref="IBillParticipant"/>
		IBillParticipant Payor { get; set; }
		
		
		string Reference { get; set; }
		
		
		string UnstructuredMessage { get; set; }
	}

	public class BillDataModel : IBillDataModel
	{
		/// <inheritdoc cref="Language"/>
		public Language Language { get; set; }

		/// <inheritdoc cref="GraphicsFormat"/>
		public GraphicsFormat GraphicsFormat { get; set; }

		/// <inheritdoc cref="OutputSize"/>
		public OutputSize OutputSize { get; set; }

		public decimal? Amount { get; set; }
		public string Currency { get; set; }

		public string IBAN { get; set; }

		public string Reference { get; set; }
		public string UnstructuredMessage { get; set; }

		public IBillParticipant Payee { get; set; }
		public IBillParticipant Payor { get; set; }

	}
}
