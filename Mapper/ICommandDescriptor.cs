using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBdd.Mapper
{
    public interface ICommandDescriptor
    {
        IEnumerable<IDataParameter> CommandParameters { get; }
        string CommandText { get; }
        CommandType CommType { get; }
    }
}
