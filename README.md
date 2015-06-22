# DbApi
Apply on relational DBMS respecting some interfaces from .Net's System.Data assembly. It's a tiny set of classes helping object instaciation from a query result. It uses reflection to instanciate objects. It may not depend explicitly on DBMS.

## DbMapping
Defines two classes used for properties tagging.

## Mapper
Wraps DBMS commands.

## Readers
Provides main classes. 

### LambdaDataReader 
Allows retrieving generic collection from a query using IDataReader and DataReflectionReader.

### DataReflectionReader
Instanciate object mapping public properties witch DbAttribute value equals query field name.

### OracleWrapper
Is an implementation of a LambdaDataReader specifying connection and command for oracle. Provides an easy way to get data from an oracle database.

# example
Giving a query returning a cursor like:
'''sql
ID    Number
NAME  Varchar2
AGE   Number
'''

Build a class like :
'''c#
    [DbObject("Kitty")]
    public class Kitty
    {
        [DbAttribute("ID")]
        public int Id { get; set; }
        [DbAttribute("NAME")]
        public string Name { get; set; }
        [DbAttribute("AGE")]
        public int Age { get; set; }
        public string DummyProp;
    }
'''

Instanciating a Kitty collection will simply be:
'''c#
        static public Cat[] ReadAllKitties(bool cached)
        {
            const string connectionString = "Data Source=service;User Id=user;Password=pwd;";

            OracleWrapper oracleWrapper = new OracleWrapper(connectionString);
            SrgAgentVrs[] ret = oracleWrapper.GetData<SrgAgentVrs>("SELECT * FROM KITTIES").ToArray();

            return ret;
        }
'''
