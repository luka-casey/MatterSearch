using MatterSearchApi.Interfaces;
using MatterSearchApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddScoped<IEmbeddingService, EmbeddingService>();
builder.Services.AddScoped<IPineconeService, PineconeService>();

// log if API key missing
var openAiKey = builder.Configuration["OpenAI:ApiKey"];
if (string.IsNullOrWhiteSpace(openAiKey))
{
    throw new Exception("OpenAI API key is missing or invalid in appsettings.json or environment variables.");
}

var pineconeApiKey = builder.Configuration["Pinecone:ApiKey"];
if (string.IsNullOrWhiteSpace(pineconeApiKey))
{
    throw new Exception("Pinecone API key is missing or invalid in appsettings.json or environment variables.");
}

// Build app
var app = builder.Build();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// Controllers
app.MapControllers();

// Run
app.Run();