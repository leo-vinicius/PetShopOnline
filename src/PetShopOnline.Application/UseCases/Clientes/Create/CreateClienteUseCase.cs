using PetShopOnline.Communication.Requests;
using PetShopOnline.Communication.Responses;
using PetShopOnline.Domain;
using PetShopOnline.Exceptions;

namespace PetShopOnline.Application.UseCases.Clientes.Create
{
    public class CreateClienteUseCase
    {
        private readonly IClienteRepository _clienteRepository;

        public CreateClienteUseCase(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public ResponseCreateClienteJson Executar(RequestCreateClienteJson request)
        {
            Validar(request);

            Cliente cliente = new Cliente(request.Nome, request.Email, request.Telefone, request.Senha);
            _clienteRepository.Adicionar(cliente);

            return new ResponseCreateClienteJson
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Telefone = cliente.Telefone
            };
        }

        private void Validar(RequestCreateClienteJson request)
        {
            var validator = new CreateClienteValidator();
            var result = validator.Validate(request);
            if (_clienteRepository.ExistePorEmail(request.Email))
            {
                throw new ClienteJaCadastradoException();
            }
        }
    }
}
