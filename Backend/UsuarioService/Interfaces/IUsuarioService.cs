namespace Backend.UsuarioService.Interfaces
{
    public interface IUsuarioService
    {
        Task<object> CadastrarAsync(object dto);
        Task<object> LoginAsync(object loginDto);
    }
}