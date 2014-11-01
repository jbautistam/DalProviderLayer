using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Bau.Libraries.LibDBProvidersBase.Providers.SQLServer;
using Bau.Libraries.LibDBProviderSQLServerCE;

namespace TestsLibDB
{
	/// <summary>
	///		Formulario principal para pruebas de la librería LibDBProvidersXXX
	/// </summary>
	public partial class frmMain : Form
	{
		public frmMain()
		{ InitializeComponent();
		}

		/// <summary>
		///		Prueba de SQL Server
		/// </summary>
		private void TestSQLServer()
		{ try
				{ using (SQLServerProvider objSQLServer = new SQLServerProvider(new SQLServerConnectionString(txtConnectionString.Text)))
						{ // Abre la conexión
								objSQLServer.Open();
							// Obtiene un DataReader
								objSQLServer.ExecuteReader(txtSQL.Text, null, System.Data.CommandType.Text);
							// Cierra la conexión
								objSQLServer.Close();
						}
					MessageBox.Show("Prueba ejecutada correctamente");
				}
			catch (Exception objException)
				{ AddLog("Error en la prueba SQL Server", objException);
				}
		}

		/// <summary>
		///		Prueba de SQL Server Compact Edition
		/// </summary>
		private void TestSQLServerCE()
		{ try
				{ using (SQLServerCompactProvider objSQLServer = new SQLServerCompactProvider(new SQLServerCompactConnectionString(txtConnectionStringSQLCe.Text)))
						{ // Abre la conexión
								objSQLServer.Open();
							// Obtiene un DataReader
								objSQLServer.ExecuteReader(txtSQL.Text, null, System.Data.CommandType.Text);
							// Cierra la conexión
								objSQLServer.Close();
						}
					MessageBox.Show("Prueba ejecutada correctamente");
				}
			catch (Exception objException)
				{ AddLog("Error en la prueba SQL Server", objException);
				}
		}

		/// <summary>
		///		Añade un mensaje al log
		/// </summary>
		private void AddLog(string strMessage, Exception objException)
		{ AddLog(strMessage + Environment.NewLine + objException.Message + Environment.NewLine + objException.StackTrace);
		}

		/// <summary>
		///		Añade un mensaje al log
		/// </summary>
		private void AddLog(string strMessage)
		{ txtLog.AppendText(string.Format("{0:HH:mm:ss}", DateTime.Now));
			txtLog.AppendText(strMessage);
		}

		private void cmdTestSQLServer_Click(object sender, EventArgs e)
		{ TestSQLServer();
		}

		private void cmdTestSQLServerCE_Click(object sender, EventArgs e)
		{ TestSQLServerCE();
		}
	}
}
