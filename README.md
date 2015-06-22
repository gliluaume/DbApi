# DbApi
Apply on relational DBMS respecting some interfaces from .Net's System.Data assembly. It's a tiny set of classes helping object instaciation from a query result. It uses reflection to instanciate objects. It may not depend explicitly on DBMS.

# Example
Giving a query returning a cursor like:
```
ID    Number
NAME  Varchar2
AGE   Number
```

Build a class like (it just need to have a no parameter constructor) :
```csharp
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
```

Instanciating a Kitty collection will simply be :
For a simple select query from KITTIES table :
```csharp
public Kitty[] ReadAllKitties()
{
    const string connectionString = "Data Source=service;User Id=user;Password=pwd;";

    OracleWrapper oracleWrapper = new OracleWrapper(connectionString);
    Kitty[] ret = oracleWrapper.GetData<Kitty>("SELECT * FROM KITTIES").ToArray();

    return ret;
}
```
For a stored procedure PKG_READ.GET_KITTIES which returns described result above :
```csharp
public Kitty[] ReadAllKitties()
{
    const string connectionString = "Data Source=service;User Id=user;Password=pwd;";
    
    List<OracleParameter> oraParameters = new List<OracleParameter>();
    oraParameters.Add(new OracleParameter("p_return_code", OracleDbType.Int32, ParameterDirection.Output));
    oraParameters.Add(new OracleParameter("p_sql_code", OracleDbType.Int32, ParameterDirection.Output));
    oraParameters.Add(new OracleParameter("p_error_text", OracleDbType.Varchar2, ParameterDirection.Output));
    oraParameters.Add(new OracleParameter("cur_out", OracleDbType.RefCursor, ParameterDirection.Output));
    
    OracleWrapper oracleWrapper = new OracleWrapper(connectionString);
    Kitty[] ret = oracleWrapper.GetData<Kitty>("PKG_READ.GET_KITTIES", oraParameters).ToArray();

    return ret;
}
```


## Details
Defines two classes used for properties tagging.

### Mapper
Wraps DBMS commands.

### Readers
Provides main classes. 

#### LambdaDataReader 
Allows retrieving generic collection from a query using IDataReader and DataReflectionReader.

#### DataReflectionReader
Instanciate object mapping public properties witch DbAttribute value equals query field name. Uses a ```csharp Dictionary<string, PropertyInfo>``` as a cache for performance reasons (reflection may be dramatically slow).

#### OracleWrapper
Is an implementation of a LambdaDataReader specifying connection and command for oracle. Provides an easy way to get data from an oracle database.


