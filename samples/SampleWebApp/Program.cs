using QueryBuilderSpecs.Extensions;
using SampleWebApp.Application.Users.Filters;
using SampleWebApp.Domain.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//configuration for the library
builder.Services.UseQuerySpecificationBuilder();
builder.Services.AddFilterBuilder<User,UserFilter,UserFilterBuilder>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/test-user-uow", () =>
    {
        
        
        return 200;
    })
    .WithOpenApi();

app.Run();