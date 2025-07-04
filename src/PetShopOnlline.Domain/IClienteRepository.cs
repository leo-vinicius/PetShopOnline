namespace PetShopOnline.Domain
{
    public interface IClienteRepository
    {
        bool ExistePorEmail(string email);
        void Adicionar(Cliente cliente);
        List<Cliente> GetClientes();
    }
}
