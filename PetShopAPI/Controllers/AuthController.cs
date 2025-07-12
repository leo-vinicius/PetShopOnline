using Microsoft.AspNetCore.Mvc;
using PetShopAPI.DTOs;
using PetShopAPI.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace PetShopAPI.Controllers
{
    /// <summary>
    /// Controller para autenticação de usuários (clientes e administradores)
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Autenticação e gerenciamento de sessões")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Realiza login de cliente no sistema
        /// </summary>
        /// <param name="loginDto">Dados de login (email e senha)</param>
        /// <returns>Token de autenticação e informações do usuário</returns>
        /// <response code="200">Login realizado com sucesso</response>
        /// <response code="401">Email ou senha incorretos</response>
        /// <response code="400">Dados de entrada inválidos</response>
        [HttpPost("cliente/login")]
        [SwaggerOperation(
            Summary = "Login de cliente",
            Description = "Autentica um cliente no sistema usando email e senha. Retorna token para uso em requisições autenticadas.",
            OperationId = "LoginCliente",
            Tags = new[] { "Autenticação" }
        )]
        [SwaggerResponse(200, "Login realizado com sucesso", typeof(object))]
        [SwaggerResponse(401, "Email ou senha incorretos")]
        [SwaggerResponse(400, "Dados de entrada inválidos")]
        public async Task<IActionResult> LoginCliente(LoginDto loginDto)
        {
            var result = await _authService.LoginClienteAsync(loginDto.Email, loginDto.Senha);

            if (!result.Success)
                return Unauthorized(new { message = result.Message });

            return Ok(new
            {
                message = result.Message,
                token = result.Token,
                userId = result.UserId,
                userType = result.UserType
            });
        }

        /// <summary>
        /// Login de administrador
        /// </summary>
        [HttpPost("admin/login")]
        public async Task<IActionResult> LoginAdministrador(LoginDto loginDto)
        {
            var result = await _authService.LoginAdministradorAsync(loginDto.Email, loginDto.Senha);

            if (!result.Success)
                return Unauthorized(new { message = result.Message });

            return Ok(new
            {
                message = result.Message,
                token = result.Token,
                userId = result.UserId,
                userType = result.UserType
            });
        }

        /// <summary>
        /// Registro de novo cliente
        /// </summary>
        [HttpPost("cliente/registro")]
        public async Task<IActionResult> RegistrarCliente(ClienteRegistroDto registroDto)
        {
            var result = await _authService.RegistrarClienteAsync(
                registroDto.Nome,
                registroDto.Email,
                registroDto.Senha,
                registroDto.Telefone);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new
            {
                message = result.Message,
                userId = result.UserId
            });
        }

        /// <summary>
        /// Logout (invalida o token)
        /// </summary>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = HttpContext.Request.Headers.Authorization
                .FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token não fornecido." });

            await _authService.LogoutAsync(token);

            return Ok(new { message = "Logout realizado com sucesso." });
        }

        /// <summary>
        /// Valida se o token ainda é válido
        /// </summary>
        [HttpGet("validate")]
        public async Task<IActionResult> ValidateToken()
        {
            var token = HttpContext.Request.Headers.Authorization
                .FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { message = "Token não fornecido." });

            var isValid = await _authService.ValidateTokenAsync(token);

            if (!isValid)
                return Unauthorized(new { message = "Token inválido ou expirado." });

            return Ok(new { message = "Token válido.", valid = true });
        }

        /// <summary>
        /// Registro de novo administrador (apenas para super admin)
        /// </summary>
        [HttpPost("administrador/registro")]
        public async Task<IActionResult> RegistrarAdministrador(AdministradorRegistroDto registroDto)
        {
            var result = await _authService.RegistrarAdministradorAsync(
                registroDto.Nome,
                registroDto.Email,
                registroDto.Senha);

            if (!result.Success)
                return BadRequest(new { message = result.Message });

            return Ok(new
            {
                message = result.Message,
                userId = result.UserId
            });
        }
    }
}
