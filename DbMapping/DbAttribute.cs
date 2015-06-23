using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbApi.DbMapping
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DbAttribute : Attribute
    {
        private string name;
        private bool isMandatory;
        public double version = 1.0;

        public DbAttribute(string name)
        {
            this.name = name;
            this.isMandatory = false;
        }
        public DbAttribute(string name, bool mandatory)
        {
            this.name = name;
            this.isMandatory = mandatory;
        }
        public string Name { get { return name; } }
        public bool IsMandatory { get { return isMandatory; } }
    }
}
