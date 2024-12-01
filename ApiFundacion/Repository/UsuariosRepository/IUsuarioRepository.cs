using ApiFundacion.Models;
using ApiFundacion.Models.DTO;
using Resultados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiFundacion.Repository.Usuarios
{
    public interface IUsuarioRepository
    {
        Task<ResultadosApi> Signup(UsuarioDTO oPersonal);
        Task<ResultadosApi> Login(UsuarioDTO oPersonal);
        List<Usuario> GetUsuario();
        bool UpdatePass(UsuarioUpdate usuario);
        //List<Personal> GetPersonal();

        //Task<Personal> Login(Personal oPersonal);
        Task<List<Usuario>> GetUsuarioLogin();
        

        //string Authenticate(UsuarioDTO usuario);

    }
}
