using System.Reflection.Metadata;
using System.Runtime.Intrinsics.Arm;
using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using FluentValidation;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=app.db"));

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();


builder.Services.AddValidatorsFromAssemblyContaining<UsuarioCreateDtoValidator>();


var app = builder.Build();

//Listar Usuarios
app.MapGet("/Usuarios", async (IUsuarioService service, CancellationToken ct) =>
{
    var Usuarios = await service.ListarAsync(ct);
    return Results.Ok(Usuarios);
});
// Buscar por ID
app.MapGet("/Usuarios/{id}", async (int id, IUsuarioService service, CancellationToken ct) =>
{
    var Usuario = await service.ObterAsync(id, ct);
    return Usuario != null ? Results.Ok(Usuario) : Results.NotFound();
});
//Criando Usuario
app.MapPost("/Usuarios", async (
    UsuarioCreateDto dto,
    IUsuarioService service,
    CancellationToken ct,
    IValidator<UsuarioCreateDto> validator) =>
{
   
    var ResultValidation = await validator.ValidateAsync(dto, ct);

    if (!ResultValidation.IsValid)
    {
        return Results.ValidationProblem(ResultValidation.ToDictionary());
    }
    
    var Usuario = await service.CriarAsync(dto, ct);
    return Results.Created($"/Usuarios/{Usuario.Id}", Usuario);
});

// Atualização do Usuario
app.MapPut("/Usuarios/{id}", async (int id, UsuarioUpdateDto dto, IUsuarioService service, CancellationToken ct, IValidator<UsuarioUpdateDto> validator) =>
{
    var validationResult = await validator.ValidateAsync(dto, ct);
    if (!validationResult.IsValid) return Results.ValidationProblem(validationResult.ToDictionary());
    var UsuarioAtualizado = await service.AtualizarAsync(id, dto, ct);
    return Results.Ok(UsuarioAtualizado);

});

// Deletar Usuario
app.MapDelete("/Usuarios/{id}", async (int id, IUsuarioService service, CancellationToken ct) =>
{
    var removido = await service.RemoverAsync(id, ct);
    if (!removido)
        return Results.NotFound();

    return Results.NoContent();
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();


app.Run();
