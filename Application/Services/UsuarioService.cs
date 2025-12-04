using Application.Interfaces;
using Application.DTOs;

namespace Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repo;

    public UsuarioService(IUsuarioRepository repository)
    {
        _repo = repository;
    }

    public async Task<IEnumerable<UsuarioReadDto>> ListarAsync(CancellationToken ct)
    {
        var usuarios = await _repo.GetAllAsync(ct);
        return usuarios.Select(MappingExtensions.ToReadDto);
    }

    public async Task<UsuarioReadDto?> ObterAsync(int id, CancellationToken ct)
    {
        var usuario = await _repo.GetByIdAsync(id, ct);
        if (usuario == null || !usuario.Ativo)
            return null;

        return MappingExtensions.ToReadDto(usuario);
    }

    public async Task<UsuarioReadDto> CriarAsync(UsuarioCreateDto dto, CancellationToken ct)
    {
        if (await _repo.EmailExistsAsync(dto.Email, ct))
            throw new Exception("Email já cadastrado");

        var usuario = UsuarioFactory.Criar(dto.Nome, dto.Email, dto.Telefone, dto.DataNascimento);
        usuario.Ativo = true;

        await _repo.AddAsync(usuario, ct);
        await _repo.SaveChangesAsync(ct);

        return MappingExtensions.ToReadDto(usuario);
    }

    public async Task<UsuarioReadDto> AtualizarAsync(int id, UsuarioUpdateDto dto, CancellationToken ct)
    {
        var usuario = await _repo.GetByIdAsync(id, ct);

        if (usuario == null)
            throw new Exception("Usuário não encontrado");

       
        if (dto.Email != usuario.Email)
        {
            var emailExiste = await _repo.EmailExistsAsync(dto.Email, ct);
            if (emailExiste)
                throw new Exception("Email já cadastrado");
        }

        usuario.Nome = dto.Nome;
        usuario.Email = dto.Email;
        usuario.Telefone = dto.Telefone;
        usuario.DataNascimento = dto.DataNascimento;

        await _repo.UpdateAsync(usuario, ct);
        await _repo.SaveChangesAsync(ct);

        return MappingExtensions.ToReadDto(usuario);
    }

    public async Task<bool> RemoverAsync(int id, CancellationToken ct = default)
    {
        var usuario = await _repo.GetByIdAsync(id, ct);
        if (usuario == null)
            return false;

        usuario.Ativo = false;

        await _repo.UpdateAsync(usuario, ct);
        await _repo.SaveChangesAsync(ct);

        return true;
    }

    public Task<bool> EmailJaCadastradoAsync(string email, CancellationToken ct)
    {
        return _repo.EmailExistsAsync(email, ct);
    }

    public Task<object?> AtualizarParcialAsync(int id, Usuario usuario, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
