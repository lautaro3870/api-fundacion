using ApiFundacion.Models;
using ApiFundacion.Models.DTO;
using ApiFundacion.Repository.Usuarios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiFundacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Prog3")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [Authorize]
        [HttpGet("Usuarios")]
        public async Task<List<Usuario>> GetUsuariosLogin()
        {
            return await _usuarioRepository.GetUsuarioLogin();
        }

        [HttpPost("Login")]
        public async Task<ResultadosApi> Login(UsuarioDTO usuario)
        {
            var u = await _usuarioRepository.Login(usuario);
            return u;

        }

        [Authorize]
        [HttpPut("UpdatePassword")]
        public bool Update(UsuarioUpdate usuario)
        {
            return _usuarioRepository.UpdatePass(usuario);
        }

        //// POST api/<PersonalController>
        [HttpPost]
        public async Task<ResultadosApi> Singup(UsuarioDTO usuario)
        {
            return await _usuarioRepository.Signup(usuario);
        }
    }
}
