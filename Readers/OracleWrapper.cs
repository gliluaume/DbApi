using ApiBdd.DbMapping;
using ApiBdd.Mapper;
using ApiBdd.Readers;
using ApiBdd.Tools;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ApiBdd.Readers
{
    public class OracleWrapper : LambdaDataReader
    {
        public OracleWrapper(string connectionString)
            : base(connectionString){}
        public IEnumerable<S> GetData<S>(ICommandDescriptor commandDescriptor) where S : new()
        {
            return this.GetData<S, OracleConnection, OracleCommand>(commandDescriptor);
        }
        public IEnumerable<S> GetData<S>(string commandText) where S : new()
        {
            return this.GetData<S, OracleConnection, OracleCommand>(commandText);
        }
        public IEnumerable<S> GetData<S>(string commandText, IEnumerable<IDataParameter> dbParameters, CommandType type = CommandType.StoredProcedure) where S : new()
        {
            return this.GetData<S, OracleConnection, OracleCommand>(commandText, dbParameters, type);
        }
    }
}