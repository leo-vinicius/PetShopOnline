using PetShopOnline.Communication.Responses;
using PetShopOnline.Domain;
using PetShopOnline.Exceptions;

namespace PetShopOnline.Application.UseCases.Clientes
{
    public class GetClientesUseCase
    {
        private readonly IClienteRepository _clienteRepository;

        public GetClientesUseCase(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public List<ResponseGetClientesJson> Executar()
        {
            var clientes = _clienteRepository.GetClientes();

            if (clientes == null || !clientes.Any())
            {
                throw new NenhumClienteCadastradoException();
            }

            return clientes.Select(c => new ResponseGetClientesJson
            {
                Nome = c.Nome,
                Email = c.Email,
                Telefone = c.Telefone
            }).ToList();
        }
    }
}
