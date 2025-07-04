using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopOnline.Domain
{
    public class Cliente
    {
        public Guid Id { get; set; }    
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;   
        public string Telefone { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;

        public Cliente(string nome, string email, string telefone, string senha)
        {
            Nome = nome;
            Email = email;
            Telefone = telefone;
            Senha = senha;
        }
    }
}
