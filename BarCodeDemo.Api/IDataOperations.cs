using BarCodeDemo.Api.DataModel;
using System;

namespace BarCodeDemo.Api
{
	public interface IDataOperations
	{
		IContactDataModel GetContactById(Guid id);
	}
}
