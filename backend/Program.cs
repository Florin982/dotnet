// =====================================================================
// ESERCITAZIONE 9.1: API CRUD di piloti con il modello CSR
//   client -> Controller (Endpoints) -> Service -> Repository -> dati
// =====================================================================

var builder = WebApplication.CreateBuilder(args);

// Render passa la porta nella variabile d'ambiente PORT
string? port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS: permette al frontend Vue (che sta su un altro indirizzo) di chiamare l'API
builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Swagger attivo anche in produzione, così si può provare l'API su Render
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.MapPilotiEndpoints();

app.Run();
