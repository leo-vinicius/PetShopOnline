using PetShopOnline.Domain;

namespace PetShopOnline.Infrastructure
{
    public class ClienteRepositoryEmMemoria : IClienteRepository
    {
        private static readonly List<Cliente> _clientes = new();

        public bool ExistePorEmail(string email)
        {
            return _clientes.Any(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public void Adicionar (Cliente cliente)
        {
            _clientes.Add(cliente);
        }
    }
}
