# DbReader
It applies to relational DBMS respecting some interfaces from ```System.Data``` assembly of ```.Net```. It's a tiny set of classes helping object instanciation from a query result. Object instanciation is done by reflection.

# Example
Giving a query returning a cursor like:
```
ID    Number
NAME  Varchar2
AGE   Number
```

Build a class like (it just needs to have a no parameter constructor and tagged properties to match query result fields) :
```csharp
[DbObject("Person")]
public class Person
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

Instanciating a Person collection will simply be (for a simple select query from PERSONS table) :
```csharp
public Person[] ReadAllPersons()
{
    const string connectionString = "Data Source=service;User Id=user;Password=pwd;";

    OracleWrapper oracleWrapper = new OracleWrapper(connectionString);
    Person[] ret = oracleWrapper.GetData<Person>("SELECT * FROM PERSONS").ToArray();

    return ret;
}
```
Or for a stored procedure ```PKG_READ.GET_PERSONS``` which returns described result above (can also be done with an OraCommandDescriptor) :
```csharp
public Person[] ReadAllPersonsFromProc()
{
    const string connectionString = "Data Source=service;User Id=user;Password=pwd;";
    
    List<OracleParameter> oraParameters = new List<OracleParameter>();
    oraParameters.Add(new OracleParameter("p_return_code", OracleDbType.Int32, ParameterDirection.Output));
    oraParameters.Add(new OracleParameter("p_sql_code", OracleDbType.Int32, ParameterDirection.Output));
    oraParameters.Add(new OracleParameter("p_error_text", OracleDbType.Varchar2, ParameterDirection.Output));
    oraParameters.Add(new OracleParameter("cur_out", OracleDbType.RefCursor, ParameterDirection.Output));
    
    OracleWrapper oracleWrapper = new OracleWrapper(connectionString);
    Person[] ret = oracleWrapper.GetData<Person>("PKG_READ.GET_PERSONS", oraParameters).ToArray();

    return ret;
}
```


## Details
It defines two classes used for properties tagging.

### Mapper
It wraps DBMS commands.

#### ICommandDescriptor
It is an interface packing command parameters.

#### OraCommandDescriptor
An implementation of ```ICommandDescriptor``` for Oracle.

### Readers
Provides main classes. 

#### LambdaDataReader 
Allows retrieving generic collection from a query using IDataReader and DataReflectionReader. Is ``` IDisposable```, manages DBMS connection and resources disposing.

#### DataReflectionReader
Instanciate object mapping public properties witch DbAttribute value equals query field name. Uses a ``` Dictionary<string, PropertyInfo>``` as a cache for performance reasons (reflection may be dramatically slow).

#### OracleWrapper
Is an implementation of a ```LambdaDataReader``` specifying connection and command for Oracle&reg;. Provides an easy way to get data from an oracle database.


# Note
## Compatibility
Given ```csproj``` has been generated with Visual Studio 2013&reg;.

## On perfs
More tests have to be done. Treatments seem to be about as fast as explicit instanciation on a 1200 rows and 20 columns table.
