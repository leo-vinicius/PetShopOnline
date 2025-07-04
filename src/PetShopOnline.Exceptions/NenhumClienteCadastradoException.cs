namespace PetShopOnline.Exceptions
{
    public class NenhumClienteCadastradoException : Exception
    {
        public NenhumClienteCadastradoException()
       : base("Nenhum cliente foi cadastrado ainda.") { }
    }
}
