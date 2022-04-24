using BarCodeDemo.Api;
using BarCodeDemo.Api.DataModel;
using Common.Logging;
using System;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace BarCodeDemo
{
	public class DataOperations : IDataOperations
	{
		private readonly UserConnection _userConnection;
		private readonly ILog _logger;

		public DataOperations(UserConnection userConnection, ILog logger)
		{
			_userConnection = userConnection;
			_logger = logger;
		}


		public IContactDataModel GetContactById(Guid id)
		{
			const string sourceSchemaName = "Contact";
			EntitySchemaQuery esq = new EntitySchemaQuery(_userConnection.EntitySchemaManager, sourceSchemaName);
			
			esq.PrimaryQueryColumn.IsVisible = true;
			esq.AddColumn("Name");
			esq.AddColumn("Email");
			esq.AddColumn("MobilePhone");
			esq.AddColumn("Phone");
			esq.AddColumn("Address");
			esq.AddColumn("Zip");
			
			esq.AddColumn("Address");
			esq.AddColumn("Zip");
			esq.AddColumn("BirthDate");

			EntitySchemaQueryColumn cityName = esq.AddColumn("City.Name");
			cityName.Name = "CityName";

			EntitySchemaQueryColumn countryName = esq.AddColumn("Country.Name");
			countryName.Name = "CountryName";

			EntitySchemaQueryColumn regionName = esq.AddColumn("Region.Name");
			regionName.Name = "RegionName";

			EntitySchemaQueryColumn accountName = esq.AddColumn("Account.Name");
			accountName.Name = "AccountName";


			IEntitySchemaQueryFilterItem filterById = esq.CreateFilterWithParameters(FilterComparisonType.Equal, esq.PrimaryQueryColumn.Name, id);
			esq.Filters.Add(filterById);
			EntityCollection collection = esq.GetEntityCollection(_userConnection);


			ContactDataModel result = default(ContactDataModel);
			if (collection.Count == 0) return result;

			Entity entity = collection[0];
			result = new ContactDataModel()
			{
				Name = entity.IsColumnValueLoaded("Name") ? entity.GetTypedColumnValue<string>("Name") : string.Empty,
				Email = entity.IsColumnValueLoaded("Email") ? entity.GetTypedColumnValue<string>("Email") : string.Empty,
				Phone = entity.IsColumnValueLoaded("Phone") ? entity.GetTypedColumnValue<string>("Phone") : string.Empty,
				MobilePhone = entity.IsColumnValueLoaded("MobilePhone") ? entity.GetTypedColumnValue<string>("MobilePhone") : string.Empty,
				BirthDate = entity.IsColumnValueLoaded("BirthDate") ? entity.GetTypedColumnValue<DateTime>("BirthDate") : default,
				Address = entity.IsColumnValueLoaded("Address") ? entity.GetTypedColumnValue<string>("Address") : string.Empty,
				Zip = entity.IsColumnValueLoaded("Zip") ? entity.GetTypedColumnValue<string>("Zip") : string.Empty,
				CityName = entity.IsColumnValueLoaded(cityName.Name) ? entity.GetTypedColumnValue<string>(cityName.Name) : string.Empty,
				CountryName = entity.IsColumnValueLoaded(countryName.Name) ? entity.GetTypedColumnValue<string>(countryName.Name) : string.Empty,
				RegionName = entity.IsColumnValueLoaded(regionName.Name) ? entity.GetTypedColumnValue<string>(regionName.Name) : string.Empty,
				AccountName = entity.IsColumnValueLoaded(accountName.Name) ? entity.GetTypedColumnValue<string>(accountName.Name) : string.Empty
			};
			return result;
		}

	}
}
