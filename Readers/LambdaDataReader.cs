using ApiBdd.Mapper;
using ApiBdd.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbApi.Readers
{
    public class LambdaDataReader : IDisposable
    {
        protected IDataReader dataReader;
        protected IDbCommand oracleCommand;
        protected IDbConnection connection;

        public string RootName { get; set; }
        public string ObjectName { get; set; }
        public string ConnectionString { get; set; }

        public LambdaDataReader(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        ~LambdaDataReader()
        {
            this.DisposeStuffs();
        }

        public IEnumerable<S> GetData<S, T, U>(string commandText)
            where S : new()
            where T : DbConnection, new()
            where U : DbCommand, new()
        {
            return GetData<S, T, U>(commandText, null, CommandType.Text);
        }
        public IEnumerable<S> GetData<S, T, U>(ICommandDescriptor commandDescriptor)
            where S : new()
            where T : DbConnection, new()
            where U : DbCommand, new()
        {
            return GetData<S, T, U>(commandDescriptor.CommandText, commandDescriptor.CommandParameters, commandDescriptor.CommType);
        }
        protected IEnumerable<S> GetData<S, T, U>(string commandText, IEnumerable<IDataParameter> dbParameters, CommandType type = CommandType.StoredProcedure)
            where S : new()
            where T : DbConnection, new()
            where U : DbCommand, new()
        {
            IEnumerable<S> ret;

            IDataReader dataReader = GetReader<T, U>(commandText, dbParameters, type);
            DataReflectionReader dataReflectionReader = new DataReflectionReader();
            ret = dataReflectionReader.ExtractListByReflection<S>(dataReader);
            DisposeStuffs();

            return ret;
        }

        private IDataReader GetReader<T, U>(string commandText, IEnumerable<IDataParameter> oraParameters, CommandType type = CommandType.StoredProcedure)
            where T : DbConnection, new()
            where U : DbCommand, new()
        {
            CheckCommand(commandText);

            // Se connecte
            this.connection = new T();
            this.connection.ConnectionString = this.ConnectionString;
            connection.Open();

            // Construit la commande
            this.oracleCommand = new U();
            this.oracleCommand.CommandText = commandText;
            this.oracleCommand.Connection = this.connection;

            if (null != oraParameters)
            {
                foreach (IDataParameter oraParameter in oraParameters)
                {
                    oracleCommand.Parameters.Add(oraParameter);
                }
            }
            oracleCommand.CommandType = type;

            DateTime start = DateTime.Now;
            Logger.Log(ELogLevel.INFO, "Execute oracle command");
            this.dataReader = oracleCommand.ExecuteReader();
            Logger.Log(ELogLevel.INFO, "Execute oracle command took {0}", DateTime.Now.Subtract(start));

            return dataReader;
        }

        public void Dispose()
        {
            DisposeStuffs();
        }
        private void DisposeStuffs()
        {
            // On fait le ménage
            if (null != this.dataReader)
            {
                this.dataReader.Close();
                this.dataReader.Dispose();
            }
            if (null != this.oracleCommand)
            {
                this.oracleCommand.Dispose();
            }

            if (null != this.connection)
            {
                this.connection.Close();
                this.connection.Dispose();
            }
        }

        protected void CheckCommand(string commandText)
        {
            // Test de la commande
            string cmTest = commandText.ToUpper();
            if (cmTest.Contains("DROP") || cmTest.Contains("TRUNCATE"))
            {
                throw new Exception("Forbiden command : " + commandText);
            }
        }
    }
}
