using System;

namespace BarCodeDemo.Api.DataModel
{
	/// <summary>
	/// Contact Model
	/// </summary>
	public interface IContactDataModel
	{

		/// <summary>
		/// Address CityName
		/// </summary>
		string CityName { get; set; }
		
		/// <summary>
		/// Address Country name
		/// </summary>
		string CountryName { get; set; }

		/// <summary>
		/// Address Resion name
		/// </summary>
		string RegionName { get; set; }
		
		/// <summary>
		/// Contact name
		/// </summary>
		string Name { get; set; }
		
		
		/// <summary>
		/// Contact phone
		/// </summary>
		string Phone { get; set; }

		/// <summary>
		/// Contact cell phone
		/// </summary>
		string MobilePhone { get; set; }


		/// <summary>
		/// Contact email
		/// </summary>
		string Email { get; set; }


		/// <summary>
		/// Contact birth date
		/// </summary>
		DateTime? BirthDate { get; set; }

		/// <summary>
		/// Contact address
		/// </summary>
		string Address { get; set; }
		
		/// <summary>
		/// Contact Zip
		/// </summary>
		string Zip { get; set; }

		/// <summary>
		/// Contact Zip
		/// </summary>
		string AccountName { get; set; }
	}

	/// <inheritdoc cref="IContactDataModel"/>
	public class ContactDataModel : IContactDataModel
	{
		/// <inheritdoc cref="IContactDataModel.Name"/>
		public string Name { get; set; }

		/// <inheritdoc cref="IContactDataModel.Email"/>
		public string Email { get; set; }

		/// <inheritdoc cref="IContactDataModel.MobilePhone"/>
		public string MobilePhone { get; set; }

		/// <inheritdoc cref="IContactDataModel.Phone"/>
		public string Phone { get; set; }

		/// <inheritdoc cref="IContactDataModel.CountryName"/>
		public string CountryName { get; set; }

		/// <inheritdoc cref="IContactDataModel.CityName"/>
		public string CityName { get; set; }

		/// <inheritdoc cref="IContactDataModel.RegionName"/>
		public string RegionName { get; set; }

		/// <inheritdoc cref="IContactDataModel.BirthDate"/>
		public DateTime? BirthDate { get; set; }

		/// <inheritdoc cref="IContactDataModel.Address"/>
		public string Address { get; set; }

		/// <inheritdoc cref="IContactDataModel.Zip"/>
		public string Zip { get; set; }

		/// <inheritdoc cref="IContactDataModel.AccountName"/>
		public string AccountName { get; set; }
	}
}
