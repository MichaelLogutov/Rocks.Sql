# Rocks.Sql
This is a sql generation helper tool. Meaning if you need to build some complex SQL - it can help you by lifting up cumbersome management of string conctatination for correct "where" clauses, prevent dublicate joins and so on.

Example. Lets say you have this filter model:

```csharp
public class Filter
{
    public string UserName { get; set; }
    public string UserEmail { get; set; }
    public int? MaxRecords { get; set; }
}
```

Then for generating query SQL based on presence of some or all values in this filter you can use this:

```csharp
private SqlClause CreateSql (Filter filter)
{
    var sql = SqlBuilder.SelectFrom ("Orders as o")
                        .Columns ("o.Id", "o.Date");

    if (filter.MaxRecords != null)
    {
        sql.Top (new SqlParameter
                    {
                        ParameterName = "@top",
                        SqlDbType = SqlDbType.Int,
                        Value = filter.MaxRecords
                    });
    }

    if (!string.IsNullOrEmpty (filter.UserName))
    {
        sql.From.Add ("u", "inner join Users as u on (o.UserId = u.Id)");
        sql.Where.AddEquals ("u.Name",
                                new SqlParameter
                                {
                                    ParameterName = "@userName",
                                    SqlDbType = SqlDbType.VarChar,
                                    Value = filter.UserName
                                });
    }

    if (!string.IsNullOrEmpty (filter.UserEmail))
    {
        sql.From.Add ("u", "inner join Users as u on (o.UserId = u.Id)");
        sql.Where.AddEquals ("u.Email",
                                new SqlParameter
                                {
                                    ParameterName = "@userEmail",
                                    SqlDbType = SqlDbType.VarChar,
                                    Value = filter.UserEmail
                                });
    }

    sql.OrderBy.Add ("o.Date");

    return sql.Build ();
}
```

And then you can simply use it in ADO:

```csharp
using (var connection = this.CreateConnection ())
{
    var command = connection.CreateCommand ();
    command.CommandText = sql.GetSql ();
    command.Parameters.AddRange (sql.GetParameters ().ToArray ());

    connection.Open ();
    // ... command.ExecuteReader () 
}
```
Or Dapper.NET or some other tool.

It will gets you this sql nicely formatted:

```sql
select top(@top)
	o.Id,
	o.Date
from
	Orders as o
	inner join Users as u on (o.UserId = u.Id)
where
	(u.Name = @userName)
	and (u.Email = @userEmail)
order by
	o.Date
```

For debug purposes you can use SqlClause.ToString which will inline all parameters for quickly check in sql studio:

```sql
select top(10)
	o.Id,
	o.Date
from
	Orders as o
	inner join Users as u on (o.UserId = u.Id)
where
	(u.Name = 'aaa')
	and (u.Email = 'user@email.com')
order by
	o.Date
```

This tool is provider agnostic so you can use it with MySql, Oracle and so on.
