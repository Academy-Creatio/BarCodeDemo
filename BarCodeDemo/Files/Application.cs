using BarCodeDemo.Api;
using BarCodeDemo.Api.BarCode;
using BarCodeDemo.BarCode;
using Common.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using Terrasoft.Core;

namespace BarCodeDemo.Files
{
	internal sealed class Application : IApplication
	{
		private readonly IServiceScope _scope;
		private readonly ILog _logger;

		internal Application(UserConnection userConnection, ILog logger)
		{
			ServiceCollection services = new ServiceCollection();

			//ADD YOUR SERVICES HERE
			services.AddSingleton<UserConnection>(userConnection);
			services.AddSingleton<ILog>(logger);
			services.AddSingleton<IDataOperations, DataOperations>();
			services.AddSingleton<IBarCodeGenerator, BarCodeGenerator>();
			
			ServiceProvider container = services.BuildServiceProvider(true);
			_scope = container.CreateScope();
			_logger = logger;
		}
		public T GetService<T>()
		{
			try
			{
				return _scope.ServiceProvider.GetService<T>();
			}
			catch (Exception ex)
			{
				_logger.ErrorFormat("Error {0} while resolving service {3}\n{1}\n{2}", ex.GetType(), ex.Message, ex.StackTrace, typeof(T).FullName);
				throw;
			}

		}
	}
}
