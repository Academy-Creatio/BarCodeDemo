using BarCodeDemo.Api;
using BarCodeDemo.Api.BarCode;
using BarCodeDemo.Api.DataModel;
using Common.Logging;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web.SessionState;
using Terrasoft.Common;
using Terrasoft.Core;
using Terrasoft.Core.Factories;
using Terrasoft.Web.Common;
using Terrasoft.Web.Http.Abstractions;

namespace BarCodeDemo
{
	[ServiceContract]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
#if NET472
	public class GenerateBarCode : BaseService, IReadOnlySessionState
#elif NETSTANDARD2_0
	public class GenerateBarCode : BaseService
#endif
	{
		#region Properties
		private HttpContext CurrentContext => HttpContextAccessor.GetInstance();
		private SystemUserConnection _systemUserConnection;



		private SystemUserConnection SystemUserConnection
		{
			get
			{
				return _systemUserConnection ?? (_systemUserConnection = (SystemUserConnection)AppConnection.SystemUserConnection);
			}
		}
		#endregion

		/// <summary>
		/// Generates BarCode
		/// </summary>
		/// <param name="contactId">
		/// </param>
		/// <returns>File with barcode</returns>
		/// <remarks>
		/// Generates BarCode by following the url<seealso href="http://localhost:8020/0/rest/GenerateBarCode/Generate/410006e1-ca4e-4502-a9ec-e54d922d2c00"/>
		/// </remarks>
		#region Methods : REST
		[OperationContract]
		[WebInvoke(Method = "GET", UriTemplate = "Generate/{contactid}",
			RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
		public Stream Generate(string contactId)
		{
			SessionHelper.SpecifyWebOperationIdentity(CurrentContext, UserConnection.CurrentUser);
			IApplication application = ClassFactory.Get<IBuilder<IApplication>>()
				.ConfigureUserConnection(UserConnection)
				.ConfigureLogger(LogManager.GetLogger("SalesLoftLogger"))
				.Build();

			IDataOperations _dataOperations = application.GetService<IDataOperations>();
			IContactDataModel contact = _dataOperations.GetContactById(Guid.Parse(contactId));

			IBarCodeGenerator barCodeGenerator = application.GetService<IBarCodeGenerator>();
			Stream businessCard = barCodeGenerator.GenerateBusinesscard(contact);

			SetHeaders(businessCard.Length);
			businessCard.Flush();
			businessCard.Seek(0, SeekOrigin.Begin);
			return businessCard;
		}

		/// <summary>
		/// Generates BarCode
		/// </summary>
		/// <param name="contactId">
		/// </param>
		/// <returns>File with barcode</returns>
		/// <remarks>
		/// Generates BarCode by following the url
		/// <seealso href="http://localhost:8020/0/rest/GenerateBarCode/GenerateSwissQrCode?contactId=410006e1-ca4e-4502-a9ec-e54d922d2c00"/>
		/// </remarks>
		[OperationContract]
		[WebInvoke(Method = "GET", 
			RequestFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
		public Stream GenerateSwissQrCode(Guid contactId)
		{
			SessionHelper.SpecifyWebOperationIdentity(CurrentContext, UserConnection.CurrentUser);
			IApplication application = ClassFactory.Get<IBuilder<IApplication>>()
				.ConfigureUserConnection(UserConnection)
				.ConfigureLogger(LogManager.GetLogger("SalesLoftLogger"))
				.Build();

			IBarCodeGenerator barCodeGenerator = application.GetService<IBarCodeGenerator>();
			
			var bill = barCodeGenerator.GenerateSissQrCode(GenerateBillModel(application, contactId));
			SetHeaders(bill.Length, "BarCode.png");
			bill.Flush();
			bill.Seek(0, SeekOrigin.Begin);
			return bill;			
		}
		#endregion

		#region METHODS: Private
		private void SetHeaders(long length, string filename = "BarCode.png")
		{

			CurrentContext.Response.ContentType = "application/octet-stream";
			CurrentContext.Response.Headers["Content-Length"] =  length.ToString();
			CurrentContext.Response.AddHeader("Content-Disposition", $"attachment; filename*=UTF-8''{filename}");
			MimeTypeResult mimeTypeResult = MimeTypeDetector.GetMimeType(filename);

#if NETFRAMEWORK
			OperationContext context = OperationContext.Current;
			if (context != null)
			{
				System.ServiceModel.Channels.HttpResponseMessageProperty responseMessageProperty =
					new System.ServiceModel.Channels.HttpResponseMessageProperty
					{
						Headers = {
							["Content-Type"] = CurrentContext.Response.ContentType
						}
					};
				string httpResponsePropertyKey = System.ServiceModel.Channels.HttpResponseMessageProperty.Name;
				context.OutgoingMessageProperties[httpResponsePropertyKey] = responseMessageProperty;
			}
#endif
		}

		private IBillDataModel GenerateBillModel(IApplication application, Guid contactId)
		{
			IDataOperations _dataOperations = application.GetService<IDataOperations>();
			IContactDataModel contact = _dataOperations.GetContactById(contactId);

			return new BillDataModel()
			{
				GraphicsFormat = GraphicsFormat.PNG,
				Language = Language.EN,
				OutputSize = OutputSize.QrBillOnly,
				IBAN = "CH4431999123000889012",
				Amount = 100,
				Currency = "EUR",
				Reference = "210000000003139471430009017",
				UnstructuredMessage = "Message would go here",
				Payor = new BillParticipant
				{
					Name = contact.Name,
					AddressLine1 = contact.Address,
					AddressLine2 = string.IsNullOrEmpty(contact.Zip) ?$"{contact.RegionName}": $"{contact.RegionName}, {contact.Zip}",
					CountryCode = "CY"
				},
				Payee = new BillParticipant
				{
					Name = contact.Name,
					AddressLine1 = contact.Address,
					AddressLine2 = string.IsNullOrEmpty(contact.Zip) ?$"{contact.RegionName}": $"{contact.RegionName}, {contact.Zip}",
					CountryCode = "CY"
				}
			};
		}
		#endregion
	}
}



