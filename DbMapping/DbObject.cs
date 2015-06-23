using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbApi.DbMapping
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class DbObject : Attribute
    {
        string name;
        public double version;

        public DbObject(string name)
        {
            this.name = name;

            // Default value.
            version = 1.0;
        }

        public string Name { get { return name; } }
    }
}
