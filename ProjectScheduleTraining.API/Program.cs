using ProjectScheduleTraining.API;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Registra todos os serviços da aplicação no container de DI.
/// </summary>
builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

/// <summary>
/// Configura o pipeline de middlewares da aplicação.
/// </summary>
app.UseApi();

app.Run();