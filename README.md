# CsSql

Though not necessarily an ideal storage solution, it's not uncommon to need to store
some serialized document structures as JSON in a SQL database. SQL is pretty good at 
storing text, but doing a database migration involving JSON values can be a nightmare. 

CsSql is designed to make this process easier, allowing execution of scripts which 
combine SQL and c# code to modify json values.

## Example
Lets say we have a table with the following schema:

|primaryKey|json                |
|----------|--------------------|
|0         |{"x":true,"y":false}|
|1         |{"x":true,"y":true} |

We need to update the json to add a new field which indicates if both x and y are true.

CsSql uses a YAML document to define JSON data transformations
It also supports an XML structure for more complex transformations. 
We didn't want any of the serialization formats to feel left out. (In reality, it was originally
using XML, but that felt too klunky. YAML is concise, allows code blocks, and is easy to read.)

For this example, we could do the following:
```
for: json
in: SELECT primaryKey, json FROM Db.schema.Table
transform: |
  //This code is essentially the body of a method void(dynamic {columnName}, dynamic record).
  //It will be compiled to native c# code. It can be as complex as you want.
  json.xAndY = json.x == true && json.y == true;
then: UPDATE Db.schema.Table SET json = @json WHERE primaryKey = @primaryKey
```

You can use @columnName in the update query to reference the values from the original query. The
parameters are mapped using Dapper.NET.

We then run the executable with the connection string as the first argument, and the path to the xml file as the second.
Once it runs, we should see the following result:

|primaryKey|json                              |
|----------|----------------------------------|
|0         |{"x":true,"y":false,"xAndY":false}|
|1         |{"x":true,"y":true,"xAndY":true}  |

## Potentially Asked Questions

Will rename to "Frequently" if people actually ask them:
* Can I do joins, etc on the initial query? ANSWER: Yes. It's just SQL. Do whatever you want.
* Can I transform multiple columns? ANSWER: Yes, use the XML syntax (see unit tests. Will add documentation eventually).
* Can I combine data from multiple JSON columns? ANSWER: Yes, but currently, you can only automatically deserialize/update one. You could in code call JsonConvert.DeserializeObject on a member of the record object, and then write that back.
* This is pretty useful. I can fold complex relational data into single read model. Can I run this as a job? ANSWER: Maybe, but I wouldn't recommend it. It's designed as a tool to support simple data migrations.
* Can I hook this up to a UI and allow users to define custom queries? ANSWER: Please don't. The c# code is not sandboxed in any way.
* Can I write complex c# code? Can I define variables, use System classes, linq, etc? ANSWER: Yes. It gets compiled as regular c# code. It does not have to be a single statement. The following namespaces are included: System, System.Collections.Generic, System.Linq, System.Text, Microsoft.CSharp, Newtonsoft.Json, and Newtonsoft.Json.Linq.
* Can I use this just to do more complex operations in SQL on regular columns with c#, like regex replaces? ANSWER: I guess. You could just ignore the JSON object and do operations on the record parameter.


## Notes

The c# script section is the same as if you ran
```
void RunScript(dynamic someColumnName, dynamic record)
{
  //your code
}
```
the deserialized json is the result of Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(...)
this can have the occasional odd result when it comes to the implicit/explicit conversions between
JToken and primative values. I.E. in the above example json.x == true && json.y == true; works, but json.x && json.y;
does not, becuase the RuntimeBinder has trouble figuring out the conversions.

the record value is the result of a Dapper.net query.

## Warnings
The code inside the script block will be compiled executed as CLR code. Running this application is 
as potentially dangerous as the code which you run on it. Use caution and obviously don't execute a script that
you don't understand and trust.

## Ideas
Currently, this is a entierly a proof of concept, though should be generally useable.
Some ideas that come to mind for future improvement:
* Allow for multiple columns in a script block. I.E. copy values from one json column to another.
* Allow for using different CLR languages.
* Create at least a simple command line UI.
* Output errors
* A better name would be cool too