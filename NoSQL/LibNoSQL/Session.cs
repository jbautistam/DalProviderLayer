using System;

using Raven.Client;

namespace Bau.Libraries.LibNoSQL
{
	/// <summary>
	///		Sesión para bases de datos NoSQL
	/// </summary>
	public class Session : IDisposable
	{ // Variables privadas
			private IDocumentSession objSession;

		public Session(Connection objConnection)
		{ DataBaseConnection = objConnection;
		}

		/// <summary>
		///		Abre la sesión
		/// </summary>
		public void Open()
		{ if (objSession == null)
				objSession = DataBaseConnection.Store.OpenSession();
		}

		/// <summary>
		///		Graba un objeto
		/// </summary>
		public void Save<TypeData>(TypeData objData)
		{ objSession.Store(objData);
		}

		/// <summary>
		///		Carga un objeto
		/// </summary>
		public TypeData Load<TypeData>(string strID)
		{ return objSession.Load<TypeData>(strID);
		}

		/// <summary>
		///		Borra un objeto
		/// </summary>
		public void Delete<TypeData>(TypeData objData)
		{ objSession.Delete(objData);
		}

		/// <summary>
		///		Graba los datos pendientes
		/// </summary>
		public void Commit()
		{ if (!IsClosed)
				objSession.SaveChanges();
		}

		/// <summary>
		///		Cierra la sesión
		/// </summary>
		public void Close()
		{ if (!IsClosed)
				{ objSession.Dispose();
					objSession = null;
				}
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
	  ~Session() 
	  {	Dispose(false);
		}

		/// <summary>
		///		Conexión
		/// </summary>
		public Connection DataBaseConnection { get; private set; }

		/// <summary>
		///		Documento de la sesión
		/// </summary>
		public IDocumentSession DocumentSession 
		{ get { return objSession; } 
		}

		/// <summary>
		///		Indica si la sesión está cerrada
		/// </summary>
		public bool IsClosed
		{ get { return objSession == null; }
		}
	}
}
