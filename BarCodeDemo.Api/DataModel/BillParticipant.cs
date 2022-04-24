namespace BarCodeDemo.Api.DataModel
{
	/// <summary>
	/// Bill participant, either payor or payee
	/// </summary>
	public interface IBillParticipant
	{
		/// <summary>
		/// Participant AddressLine1
		/// </summary>
		string AddressLine1 { get; set; }

		/// <summary>
		/// Participant AddressLine2
		/// </summary>
		string AddressLine2 { get; set; }

		/// <summary>
		/// Participant country code
		/// </summary>
		string CountryCode { get; set; }

		/// <summary>
		/// Participant Name
		/// </summary>
		string Name { get; set; }
	}
	

	/// <inheritdoc cref="IBillParticipant"/>
	public class BillParticipant : IBillParticipant
	{

		/// <inheritdoc cref="IBillParticipant.Name"/>
		public string Name { get; set; }

		/// <inheritdoc cref="IBillParticipant.AddressLine1"/>
		public string AddressLine1 { get; set; }

		/// <inheritdoc cref="IBillParticipant.AddressLine2"/>
		public string AddressLine2 { get; set; }

		/// <inheritdoc cref="IBillParticipant.CountryCode"/>
		//TODO: Country Code needs to implement conversion from whatever to a TWO-LETTER country code
		public string CountryCode { get; set; }
	}
}
