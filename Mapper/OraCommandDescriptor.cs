using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbApi.Mapper
{
    public class OraCommandDescriptor : ICommandDescriptor
    {
        public IEnumerable<IDataParameter> CommandParameters { get; set; }
        public string CommandText { get; set; }
        public CommandType CommType { get; set; }
        public OraCommandDescriptor()
        {
            this.CommandParameters = new List<OracleParameter>();
            this.CommandText = string.Empty;
            this.CommType = CommandType.StoredProcedure;
        }
    }
}
