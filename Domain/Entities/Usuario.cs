using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public  class  Usuario

{
 [Key]
public  int Id { get; set; }

[Required]
public  string Nome { get; set; } = string.Empty;

[Required]
public  string Email { get; set; } = string.Empty;

[Required]
public  string Senha { get; set; } = string.Empty;

[Required]
public  DateTime DataNascimento { get; set; }  

public  string Telefone { get; set; } 

public  bool Ativo { get; set; } = true;

public  DateTime DataCriacao { get; set; } 

public  DateTime? DataAtualizacao { get; set; } 

}
