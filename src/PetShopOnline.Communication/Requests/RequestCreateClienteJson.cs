namespace PetShopOnline.Communication.Requests
{
    public class RequestCreateClienteJson
    {
        public string Nome { get; set; } = string.Empty;
        public string Email {  get; set; } = string.Empty;
        public string Telefone {  get; set; } = string.Empty;
        public string Senha {  get; set; } = string.Empty;
    }
}
