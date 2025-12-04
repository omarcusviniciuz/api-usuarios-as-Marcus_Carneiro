using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using FluentValidation;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddValidatorsFromAssemblyContaining<UsuarioCreateDtoValidator>();

var app = builder.Build();


// LISTAR
app.MapGet("/Usuarios", async (IUsuarioService service, CancellationToken ct) =>
{
    var usuarios = await service.ListarAsync(ct);
    return Results.Ok(usuarios);
});

// BUSCAR POR ID
app.MapGet("/Usuarios/{id}", async (int id, IUsuarioService service, CancellationToken ct) =>
{
    var usuario = await service.ObterAsync(id, ct);
    return usuario is not null ? Results.Ok(usuario) : Results.NotFound();
});

// CRIAR
app.MapPost("/Usuarios", async (
    UsuarioCreateDto dto,
    IUsuarioService service,
    CancellationToken ct,
    IValidator<UsuarioCreateDto> validator) =>
{
    var validation = await validator.ValidateAsync(dto, ct);
    if (!validation.IsValid)
        return Results.ValidationProblem(validation.ToDictionary());

    var usuario = await service.CriarAsync(dto, ct);
    return Results.Created($"/Usuarios/{usuario.Id}", usuario);
});

// ATUALIZAR
app.MapPut("/Usuarios/{id}", async (
    int id,
    UsuarioUpdateDto dto,
    IUsuarioService service,
    CancellationToken ct,
    IValidator<UsuarioUpdateDto> validator) =>
{
    var validation = await validator.ValidateAsync(dto, ct);
    if (!validation.IsValid)
        return Results.ValidationProblem(validation.ToDictionary());

    var atualizado = await service.AtualizarAsync(id, dto, ct);
    return Results.Ok(atualizado);
});

// REMOVER
app.MapDelete("/Usuarios/{id}", async (int id, IUsuarioService service, CancellationToken ct) =>
{
    var removido = await service.RemoverAsync(id, ct);
    return removido ? Results.NoContent() : Results.NotFound();
});

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseHttpsRedirection();
app.Run();
