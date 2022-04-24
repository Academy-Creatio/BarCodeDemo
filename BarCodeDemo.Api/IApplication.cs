namespace BarCodeDemo.Api
{
	/// <summary>
	/// Main Application
	/// </summary>
	public interface IApplication
	{
		/// <summary>
		/// Gets requested service from the application
		/// </summary>
		/// <typeparam name="T">Service to get from the application</typeparam>
		/// <returns>Resulved service</returns>
		T GetService<T>();
	}
}
