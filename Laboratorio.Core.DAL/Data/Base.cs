using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorio.Core.DAL.Data
{
    public class Base : IDisposable
    {
        //private bool disposed;
        private SqlConnection sqlConnection = null;
        private string stringConnection { get; set; }
        private SqlTransaction ActiveTransaction { get; set; }

        public Base()
        {
            try
            {
                //stringConnection = "Data Source=[servidor]; User Id= [usuario]; Password=[contraseña]; Initial Catalog=[base de datos]";
                stringConnection = "";
            }
            catch (ConfigurationException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        public void SetStringConnection(string connection)
        {
            try
            {
                stringConnection = connection;
            }
            catch (ConfigurationException ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }

        protected bool Connect()
        {
            var boolResult = false;
            if (this.sqlConnection != null && this.sqlConnection.State.Equals(ConnectionState.Open))
            {
                return true;
            }
            try
            {
                if (this.sqlConnection == null)
                {
                    this.sqlConnection = new SqlConnection();
                    this.sqlConnection.ConnectionString = this.stringConnection;
                }
                this.sqlConnection.Open();
                boolResult = true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                boolResult = false;
            }
            return boolResult;
        }


        protected SqlCommand CreateCommand()
        {
            var command = this.sqlConnection.CreateCommand();
            if (this.ActiveTransaction != null)
            {
                command.Transaction = this.ActiveTransaction;
            }
            return command;
        }

        public void Dispose()
        {
            if (this.sqlConnection != null)
            {
                if (this.ActiveTransaction != null)
                {
                    this.ActiveTransaction.Dispose();
                }
                this.sqlConnection.Close();
                this.sqlConnection.Dispose();
            }
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }


    }
}
