namespace Application.Services;

public static class UsuarioFactory
{
    public static Usuario Criar(string nome, string email, string telefone, DateTime dataNascimento)
    {
          if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("O nome é inválido...", nameof(nome));
        }
        
        if (string.IsNullOrWhiteSpace(email))
        {
               throw new ArgumentException("O Email é inválido...", nameof(email));
        }

        if (telefone.Length < 9)
        {
             throw new ArgumentException("O Telefone é inválido...", nameof(telefone));
        }
            if (dataNascimento > DateTime.Now.AddYears(-18))
        {
            throw new ArgumentException("Data de nascimento inválida.", nameof(dataNascimento));
        }

        Console.WriteLine("Usuario validado com sucesso! Criando...");
        
        Usuario usuarioValido = new Usuario();
        usuarioValido.Nome = nome;
        usuarioValido.Telefone = telefone;
        usuarioValido.Email = email;
        usuarioValido.DataNascimento = dataNascimento;
        usuarioValido.DataCriacao = DateTime.Now;
        return usuarioValido;
    }
}
