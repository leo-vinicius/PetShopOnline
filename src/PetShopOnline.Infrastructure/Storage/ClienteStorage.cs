using PetShopOnline.Domain;

namespace PetShopOnline.Infrastructure.Storage
{
    public static class ClienteStorage
    {
        public static List<Cliente> Clientes { get; } = new();
    }
}
