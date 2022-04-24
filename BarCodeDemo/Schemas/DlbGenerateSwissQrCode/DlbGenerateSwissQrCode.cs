namespace Terrasoft.Core.Process.Configuration
{
	using BarCodeDemo.Api;
	using BarCodeDemo.Api.BarCode;
	using BarCodeDemo.Api.DataModel;
	using global::Common.Logging;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Globalization;
	using Terrasoft.Common;
	using Terrasoft.Core;
	using Terrasoft.Core.Configuration;
	using Terrasoft.Core.DB;
	using Terrasoft.Core.Entities;
	using Terrasoft.Core.Factories;
	using Terrasoft.Core.Process;
	using Terrasoft.UI.WebControls.Controls;

	#region Class: DlbGenerateSwissQrCode

	/// <exclude/>
	public partial class DlbGenerateSwissQrCode
	{

		#region Methods: Protected

		protected override bool InternalExecute(ProcessExecutingContext context) {


			bool isIdLoaded = false;
			if(EntityUnderLoad.Schema.PrimaryColumn is object)
			{
				string primaryColumnName = EntityUnderLoad.Schema.PrimaryColumn?.Name;
				isIdLoaded = EntityUnderLoad.IsColumnValueLoaded(primaryColumnName);
			}

			if (!isIdLoaded || EntityUnderLoad.Schema.Columns.FindByName("DlbSwissQrCode") == null)
			{
				return true;
			}

			IApplication application = ClassFactory.Get<IBuilder<IApplication>>()
				.ConfigureUserConnection(UserConnection)
				.ConfigureLogger(LogManager.GetLogger("SalesLoftLogger"))
				.Build();
			IBarCodeGenerator barCodeGenerator = application.GetService<IBarCodeGenerator>();

			var model = GenerateBillModel(application, EntityUnderLoad.PrimaryColumnValue);
			var barCode = barCodeGenerator.GenerateSissQrCode(model);
			EntityUnderLoad.SetBytesValue("DlbSwissQrCode", barCode.ReadAllBytes());
			
			return true;
		}

		protected IBillDataModel GenerateBillModel(IApplication application, Guid contactId)
		{
			IDataOperations _dataOperations = application.GetService<IDataOperations>();
			IContactDataModel contact = _dataOperations.GetContactById(contactId);

			return new BillDataModel()
			{
				GraphicsFormat = GraphicsFormat.PNG,
				Language = Language.EN,
				OutputSize = OutputSize.QrBillOnly,
								
				//TODO: Probably need to validate IBAN, can only start with CH and LI
				IBAN = "CH4431999123000889012",

				//TODO: AMOUNT Probably take from SwicoBillInformation is available
				Amount = 100,
				
				//TODO: I dont know whats allowed, but probably need to have a list of allowed values
				Currency = "EUR",


				UnstructuredMessage = "Message would go here",
				Payor = new BillParticipant
				{
					Name = contact.Name,
					AddressLine1 = contact.Address,
					AddressLine2 = string.IsNullOrEmpty(contact.Zip) ? $"{contact.RegionName}" : $"{contact.RegionName}, {contact.Zip}",
					CountryCode = "CY"
				},
				Payee = new BillParticipant
				{
					Name = contact.Name,
					AddressLine1 = contact.Address,
					AddressLine2 = string.IsNullOrEmpty(contact.Zip) ? $"{contact.RegionName}" : $"{contact.RegionName}, {contact.Zip}",
					CountryCode = "CY"
				}
			};
		}
		#endregion

		#region Methods: Public

		public override bool CompleteExecuting(params object[] parameters) {
			return base.CompleteExecuting(parameters);
		}

		public override void CancelExecuting(params object[] parameters) {
			base.CancelExecuting(parameters);
		}

		public override string GetExecutionData() {
			return string.Empty;
		}

		public override ProcessElementNotification GetNotificationData() {
			return base.GetNotificationData();
		}

		#endregion

	}

	#endregion

}

