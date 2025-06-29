using PetShopOnline.Communication.Requests;
using PetShopOnline.Communication.Responses;

namespace PetShopOnline.Application.UseCases.Clientes.Create
{
    public class CreateClienteUseCase
    {
        public ResponseCreateClienteJson Executar(RequestCreateClienteJson request)
        {
            Validar(request);
            return new ResponseCreateClienteJson();
        }

        private void Validar(RequestCreateClienteJson request)
        {
            if (string.IsNullOrWhiteSpace(request.Nome))
                throw new ArgumentException("O nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("O email é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.Telefone))
                throw new ArgumentException("O telefone é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.Senha))
                throw new ArgumentException("A senha é obrigatória.");
        }
    }
}
