namespace PetShopOnline.Exceptions
{
    public class ClienteJaCadastradoException : Exception
    {
        public ClienteJaCadastradoException()
            : base("Já existe um cliente com esse e-mail.") { }
    }
}
