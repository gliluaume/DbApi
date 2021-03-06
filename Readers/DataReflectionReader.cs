using DbApi.DbMapping;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DbApi.Readers
{
    public class DataReflectionReader
    {
        private Dictionary<string, PropertyInfo> propertyDescriptor;

        public DataReflectionReader()
        {
            this.propertyDescriptor = new Dictionary<string, PropertyInfo>();
        }

        public void AddInfo(string name, PropertyInfo propertyInfo)
        {
            this.propertyDescriptor.Add(name, propertyInfo);
        }
        public PropertyInfo this[string name]
        {
            get
            {
                return this.propertyDescriptor[name];
            }
        }
        public bool IsKnown(string name)
        {
            return this.propertyDescriptor.ContainsKey(name);
        }
        public List<T> ExtractListByReflection<T>(IDataReader reader) where T : new()
        {
            List<T> ret = new List<T>();
            T obj;

            if (0 < reader.FieldCount)
            {
                while (reader.Read())
                {
                    
                    obj = new T();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string strVal = reader.GetValue(i).ToString();
                        string columnName = reader.GetName(i);

                        PropertyInfo pi = GetCachedObjectAttributeName<T>(columnName);
                        if (null != pi)
                        {
                            pi.SetValue(obj, ParseString(pi.PropertyType, strVal));    
                        }
                        
                    }
                    ret.Add((T)obj);
                }
            }

            return ret;
        }

        private object ParseString(Type type, string strValue)
        {
            object ret = null;
            if (type == typeof(int))
            {
                ret = int.Parse(strValue);
            }
            else if (type == typeof(bool))
            {
                ret = (int.Parse(strValue) == 1);
            }
            else if (type == typeof(String))
            {
                ret = strValue;
            }
            return ret;
        }

        private PropertyInfo GetCachedObjectAttributeName<T>(string columnName)
        {
            PropertyInfo ret;
            // On regarde si la colonne est connue du cache
            if (!this.IsKnown(columnName))
            {
                this.AddInfo(columnName, GetObjectAttributeName<T>(columnName));
            }

            // On prend la propriété dans le cache.
            ret = this[columnName];

            return ret;
        }
        private PropertyInfo GetObjectAttributeName<T>(string attributeDbName)
        {
            PropertyInfo ret = null;
            PropertyInfo[] propsInfo = typeof(T).GetProperties();
            Attribute[] attrs;

            foreach (PropertyInfo propInfo in propsInfo)
            {
                attrs = Attribute.GetCustomAttributes(propInfo);
                foreach (Attribute attr in attrs)
                {
                    if (attr is DbAttribute)
                    {
                        DbAttribute a = (DbAttribute)attr;
                        if (string.Compare(a.Name, attributeDbName) == 0)
                        {
                            ret = propInfo;
                        }
                    }
                }
            }

            return ret;
        }
    }
}
