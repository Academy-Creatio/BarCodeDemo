using BarCodeDemo.Api.DataModel;
using System.IO;

namespace BarCodeDemo.Api.BarCode
{
	public interface IBarCodeGenerator
	{

		/// <summary>
		/// Generates bar code with <paramref name="contact"/> information
		/// </summary>
		/// <param name="contact"></param>
		/// <returns> Stream of bardcore image</returns>
		Stream GenerateBusinesscard(IContactDataModel contact);


		/// <summary>
		/// Generates SwissQR code
		/// </summary>
		/// <param name="billDataModel"></param>
		/// <returns></returns>
		Stream GenerateSissQrCode(IBillDataModel billDataModel);
	}
}
