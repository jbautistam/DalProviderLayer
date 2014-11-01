using System;

namespace Bau.Libraries.LibNoSQL
{
	/// <summary>
	///		Interface para las conexiones NoSQL
	/// </summary>
	public interface IConnectionNoSQL : IDisposable
	{
		void Open(string strPath);

		void Close();
	}
}
