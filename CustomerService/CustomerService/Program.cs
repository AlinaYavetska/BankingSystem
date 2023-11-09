using CustomerService.AsyncDataServices;
using CustomerService.Data;
using CustomerService.SyncDataServices.Grps;
using CustomerService.SyncDataServices.Http;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var env = builder.Environment;        

// Add services to the container.
if (env.IsProduction())
{
    Console.WriteLine("--> Using SqlServer Db");
    var connectionString = configuration.GetConnectionString("CustomersConnection");
    Console.WriteLine($"Connection string: {connectionString}");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseSqlServer(connectionString));
}
else
{
    Console.WriteLine("--> Using InMemory Db");
    builder.Services.AddDbContext<AppDbContext>(opt =>
        opt.UseInMemoryDatabase("InMemory"));
}    

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddHttpClient<ITransactionDataClient, HttpTransactionDataClient>();

builder.Services.AddSingleton<IMessageBusClient, MessageBusClient>();

builder.Services.AddGrpc();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Console.WriteLine($"--> TransactionService Endpoint {configuration["TransactionService"]}");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<GrpcCustomerService>();

app.MapGet("/protos/customers.proto", async context =>
{
    await context.Response.WriteAsync(File.ReadAllText("Protos/customers.proto"));
});

PrepDb.PrepPopulation(app, env.IsProduction());

app.Run();
