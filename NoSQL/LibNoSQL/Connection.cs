using System;

using Raven.Storage.Esent;
using Raven.Client.Embedded;

namespace Bau.Libraries.LibNoSQL
{
	/// <summary>
	///		Clase con los datos de una conexión
	/// </summary>
	public class Connection : IDisposable
	{
		/// <summary>
		///		Abre una conexión
		/// </summary>
		public void Open(string strPath)
		{ // Crea el directorio
				MakePath(strPath);
			// Inicializa el almacén de datos
				Store = new EmbeddableDocumentStore { DataDirectory = strPath };
				Store.Initialize();
		}

		/// <summary>
		///		Crea un directorio
		/// </summary>
		private void MakePath(string strPath)
		{ try
				{ if (!System.IO.Directory.Exists(strPath))
						System.IO.Directory.CreateDirectory(strPath);
				}
			catch {}
		}

		/// <summary>
		///		Cierra la conexión
		/// </summary>
		public void Close()
		{ // Cierra la conexión
				if (Store != null && !Store.WasDisposed)
					Store.Dispose();
			// Libera el objeto
				Store = null;
		}

		/// <summary>
		///		Desconecta la conexión
		/// </summary>
		public void Dispose() 
		{	Dispose(true);
			GC.SuppressFinalize(this);
		}
	
		/// <summary>
		///		Desconecta la conexión
		/// </summary>
		private void Dispose(bool blnDispose) 
		{	Close();
		}

		/// <summary>
		///		Destruye el objeto
		/// </summary>
	  ~Connection() 
	  {	Dispose(false);
		}

		/// <summary>
		///		Almacén
		/// </summary>
		internal EmbeddableDocumentStore Store { get; private set; }
	}
}
