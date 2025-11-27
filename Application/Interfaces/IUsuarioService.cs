using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


public interface IusuarioService
{
Task<IEnumerable<UsuarioReadDto>> ListarAsync(CancellationToken  ct);

Task<UsuarioReadDto?> ObterAsync(int  id, CancellationToken  ct);

Task<UsuarioReadDto> CriarAsync(UsuarioCreateDto  dto, CancellationToken  ct);

Task<UsuarioReadDto> AtualizarAsync(int  id, UsuarioUpdateDto  dto, CancellationToken  ct);

Task<bool> RemoverAsync(int  id, CancellationToken  ct);

Task<bool> EmailJaCadastradoAsync(string  email, CancellationToken  ct);
}