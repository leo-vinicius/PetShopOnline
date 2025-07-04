using FluentValidation;
using PetShopOnline.Communication.Requests;

namespace PetShopOnline.Application.UseCases.Clientes.Create
{
    public class CreateClienteValidator : AbstractValidator<RequestCreateClienteJson>
    {
        public CreateClienteValidator()
        {
            RuleFor(cliente => cliente.Nome).NotEmpty().WithMessage("O nome é obrigatório");
            RuleFor(cliente => cliente.Email).NotEmpty().WithMessage("O email é obrigatório");
            RuleFor(cliente => cliente.Telefone).NotEmpty().WithMessage("O telefone é obrigatório");
            RuleFor(cliente => cliente.Senha).NotEmpty().WithMessage("A senha é obrigatória");
        }
    }
}
