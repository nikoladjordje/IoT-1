using GrpcClientIoT.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

var mongoClient = new MongoClient("mongodb://127.0.0.1:27017");
var database = mongoClient.GetDatabase("Beehive");

try
{
    mongoClient.ListDatabaseNames();
    Console.WriteLine("Successfully connected to MongoDB server.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error connecting to MongoDB server: {ex.Message}");
}

builder.Services.AddSingleton<IMongoClient>(mongoClient);
builder.Services.AddSingleton<IMongoDatabase>(database);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<Microservices>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
