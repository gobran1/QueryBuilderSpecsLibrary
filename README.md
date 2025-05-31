# QueryBuilderSpecsLibrary 🚀

![NuGet](https://www.nuget.org/packages/QueryBuilderSpecs/)
![License](https://raw.githubusercontent.com/gobran1/QueryBuilderSpecsLibrary/refs/heads/main/License)

A powerful and clean .NET library for building query specifications using the **Specification Pattern**, **Generic Repository**, and **Unit of Work**.

> ✅ Fully supports filtering via DTOs  
> ✅ Works with both EF Core DbContext and Unit of Work  
> ✅ Highly customizable and testable  

---

## 📦 Installation

Install via NuGet:

```bash
dotnet add package QueryBuilderSpecsLibrary
```

---

## 🧰 Features

- 🔄 **Unit of Work**: Abstracts multiple repositories with transactional support
- 🔍 **Specification Pattern**: Enables clean, reusable, and composable query logic
- 🧪 **Filter Builder**: Apply filtering rules based on DTOs dynamically
- 💡 **Generic Repository**: Simplifies CRUD operations

---

## 🛠️ Setup

### 1. Register Library Services in `Program.cs`

```csharp
builder.Services.UseQueryBuilderSpecs(); // Registers UoW and repository
```

### 2. Register Filter Builder for a specific entity

```csharp
builder.Services.AddFilterBuilder<User, UserFilter, UserFilterBuilder>();
```

---

## 🧪 Usage

### ✅ Using DbContext with Filter Builder

```csharp
app.MapGet("/api/users/from-dbcontext", async (
    UserFilter filter,
    [FromServices] AppDbContext dbContext,
    [FromServices] IGenericFiltersBuilder<User, UserFilter> filterBuilder
) =>
{
    var users = await dbContext.Users
        .ApplyFilter(filter, filterBuilder)
        .ToListAsync();

    return Results.Ok(users);
});
```

### ✅ Using Unit of Work

```csharp
app.MapGet("/api/users/from-uow", async (
    UserFilter filter,
    [FromServices] IUnitOfWork<AppDbContext> uow,
    [FromServices] IGenericFiltersBuilder<User, UserFilter> filterBuilder
) =>
{
    var spec = uow.Repository<User>()
        .InjectSpecification(filterBuilder, filter);

    var users = await uow.Repository<User>().GetAllAsync(extendQuery: spec);

    return Results.Ok(users);
});
```

---

## 📁 Folder Structure

```
📦 QueryBuilderSpecsLibrary
 ┣ 📂 src
 ┃ ┣ 📜 QueryBuilderSpecs.csproj
 ┃ ┣ 📜 Interfaces/
 ┃ ┣ 📜 Extensions/
 ┃ ┗ 📜 Specifications/
 ┗ 📂 SampleWebApp
    ┗ 📜 Program.cs
```

---

## 📄 Sample DTO and Filter Builder

### `UserFilter.cs`

```csharp
public class UserFilter
{
    public string? Name { get; set; }
    public bool? IsActive { get; set; }
}
```


### `FilterByIsActiveSpecification.cs`

```csharp
public class FilterByIsActiveSpecification : BaseSpecification<User>, ISpecification<User>
{
    public FilterByIsActiveSpecification(bool? isActive)
    {
        if (isActive == null)
            return;

        SetCriteria(u => u.IsActive == isActive);
    }
}
```


### `UserFilterBuilder.cs`

```csharp

public class UserFilterBuilder: GenericFiltersBuilder<User,UserFilter>,IGenericFiltersBuilder<User,UserFilter> 
{
    public override void InitItemSpecifications()
    {
        AddSpecification(
            nameof(UserFilter.Name),
            filter => new FilterByNameSpecification(filter.Name)
        );
        
        
        AddSpecification(
            nameof(UserFilter.IsActive),
            filter => new FilterByIsActiveSpecification(filter.IsActive)
        );
    }
}
```

---

## 🧪 Testing

Use the sample project to test and verify functionality:

```bash
dotnet run --project SampleWebApp
```

Navigate to:

```
https://localhost:5001/swagger
```

Test your endpoints with filters such as:

- `/api/users/from-dbcontext?name=John`
- `/api/users/from-uow?isActive=true`

---

## 🧾 Versioning

We follow [Semantic Versioning](https://semver.org/):

- `1.0.0` → Initial release
- `1.1.0` → Minor feature additions (non-breaking)


Update the version in `.csproj` and tag it:

```xml
<Version>1.1.0</Version>
```

---

## 📝 License

This project is licensed under the [MIT License](LICENSE).

---

## 🙋‍♂️ Contributing

We welcome contributions! Feel free to open issues, PRs, or suggest improvements.

---

## 🌐 Author

Developed with ❤️ by **GibranFahed**
