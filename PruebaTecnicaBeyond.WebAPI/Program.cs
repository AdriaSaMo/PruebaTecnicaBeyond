using PruebaTecnicaBeyond.Interfaces;
using PruebaTecnicaBeyond.Repositories;
using PruebaTecnicaBeyond.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ITodoListRepository, TodoListRepository>(); //para poder usarlos en memoria
builder.Services.AddSingleton<TodoListService>();


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost5500", policy =>
    {
        policy
            .WithOrigins("http://127.0.0.1:5500")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost5500");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
