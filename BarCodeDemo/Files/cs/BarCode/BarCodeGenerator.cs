//using Aspose.BarCode.ComplexBarcode;
using Aspose.BarCode.Generation;
using BarCodeDemo.Api.BarCode;
using BarCodeDemo.Api.DataModel;
using Codecrete.SwissQRBill.Generator;
using System;
using System.IO;

namespace BarCodeDemo.BarCode
{
	public class BarCodeGenerator : IBarCodeGenerator
	{
		public Stream GenerateBusinesscard(IContactDataModel contact)
		{
			string meCard = $"BEGIN:VCARD\n" +
				$"VERSION:3.0\n"+
				$"FN;CHARSET=UTF-8:{contact.Name}\n" +
				$"N;CHARSET=UTF-8:{contact.Name}\n" +
				$"ORG;CHARSET=UTF-8:{contact.AccountName}\n" +
				$"TEL;TYPE=HOME:{contact.Phone}\n" +
				$"TEL;TYPE=CELL:{contact.MobilePhone}\n" +
				$"EMAIL:{contact.Email}\n" +
				$"ADR;TYPE=HOME:;;{contact.Address};{contact.CityName};{contact.RegionName};{contact.Zip};{contact.CountryName}\n" +
				$"BDAY:{contact.BirthDate:yyyyMMdd}\n" +
				$"URL:https://creatio.com\n" +
				$"END:VCARD";
			BarcodeGenerator generator = new BarcodeGenerator(EncodeTypes.QR, meCard);
			generator.Parameters.Resolution = 600;


            //var swissQRCodetext = new SwissQRCodetext();
            MemoryStream stream = new MemoryStream();
			generator.Save(stream, BarCodeImageFormat.Png);
			//generator.Dispose();
			stream.Flush();
			stream.Seek(0, SeekOrigin.Begin);
			
			return stream;
		}

        /// <summary>
        /// https://github.com/manuelbl/SwissQRBill.NET
        /// Validate with <seealso href="https://www.swiss-qr-invoice.org/validator/?lang=de/>
        /// </summary>
        /// <param name="billDataModel"></param>
        /// <returns></returns>
		public Stream GenerateSissQrCode(IBillDataModel billDataModel)
		{
            //TOD: SwicoBillInformation needs to be dynamically querried
            SwicoBillInformation billDetails = new SwicoBillInformation()
            {
                InvoiceNumber = "INV-1",
                InvoiceDate = System.DateTime.Now,
                CustomerReference = "PONUMBER_HERE",
                PaymentConditions = new System.Collections.Generic.List<(decimal, int)>()
                {
                    { new ValueTuple<decimal, int>(2, 10) },
                    { new ValueTuple<decimal, int>(0, 30) }
                },
                VatRate = 20,
                VatNumber = "MY_VAT_HERE"
            };
            Bill bill = new Bill
            {
                Account = billDataModel.IBAN,
                Creditor = new Address
                {
                    Name = billDataModel.Payee.Name,
					AddressLine1 = billDataModel.Payee.AddressLine1,
                    AddressLine2 = billDataModel.Payee.AddressLine2,
                    CountryCode = billDataModel.Payee.CountryCode,
                },

                // payment data
                Amount = billDataModel.Amount,
                Currency = billDataModel.Currency,

                // debtor data
                Debtor = new Address
                {
                    Name = billDataModel.Payor.Name,
                    AddressLine1 = billDataModel.Payor.AddressLine1,
                    AddressLine2 = billDataModel.Payor.AddressLine2,
                    CountryCode = billDataModel.Payor.CountryCode,
                },

                // more payment data
                //Reference = billDataModel.Reference,
                BillInformation = billDetails.EncodeAsText(),
                UnstructuredMessage = billDataModel.UnstructuredMessage,


                // output format
                Format = new BillFormat
                {
                    Language = (Codecrete.SwissQRBill.Generator.Language)billDataModel.Language,
                    GraphicsFormat = (Codecrete.SwissQRBill.Generator.GraphicsFormat)billDataModel.GraphicsFormat,
                    OutputSize = (Codecrete.SwissQRBill.Generator.OutputSize)billDataModel.OutputSize
                }
            };

            bill.CreateAndSetQRReference("5390 0754 7034");

			// Generate QR bill
			byte[] image = QRBill.Generate(bill);
			return new MemoryStream(image);
		}
	}
}
