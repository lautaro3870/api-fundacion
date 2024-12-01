using ApiFundacion.Models;
using ApiFundacion.Models.DTO;
using ApiFundacion.Models.DTO.Usuarios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Resultados;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;



namespace ApiFundacion.Repository.Usuarios
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly dena66utud3alcContext context;
        private readonly IConfiguration _configuration;

        public UsuarioRepository(dena66utud3alcContext context, IConfiguration configuration)
        {
            this.context = context;
            _configuration = configuration;
        }

        //public UsuarioRepository(string key)
        //{
        //    this.key = key;
        //}

        //public string Authenticate(UsuarioDTO usuario)
        //{
        //    if (!context.Usuarios.Any(u => u.Email == null && u.Password == null))
        //    {
        //        return null;
        //    }
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var tokenKey = Encoding.ASCII.GetBytes(key);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[] {
        //            new Claim(ClaimTypes.Email, usuario.Email)
        //        }),
        //        Expires = DateTime.UtcNow.AddHours(1),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
        //            SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}

        //public List<Personal> GetPersonal()
        //{
        //    return  context.Personals.Where(x => x.Activo == true && x.Nombre != null && x.Id != 33).OrderBy(x => x.Nombre).ToList();
        //}

        private string CrearToken(Usuario u)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, u.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Settings:key").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(50),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public List<Usuario> GetUsuario()
        {
            return context.Usuarios.Where(x => x.Activo == true).ToList();
        }

        public async Task<List<Usuario>> GetUsuarioLogin()
        {
            return await context.Usuarios.Where(x => x.Activo == true).ToListAsync();
        }

        //public Usuario Login(Usuario oPersonal)
        //{
        //    var r = new ResultadosApi();
        //    var personal = context.Usuarios.SingleOrDefault(x => x.Email == oPersonal.Email);
        //    bool isValidPassword = BCrypt.Net.BCrypt.Verify(oPersonal.Password, personal.Password);

        //    if (isValidPassword)
        //    {
        //        return personal;
        //    }
        //    return null;
        //}

        //public async Task<Personal> Login(Personal oPersonal)
        //{

        //    var personal = await context.Personals.SingleOrDefaultAsync(x => x.Email == oPersonal.Email);
        //    el metodo verify de la clase Bcrypt es estatico, por ende no se debe instanciar la clase, solo se implementa
        //        bool isValidPassword = BCrypt.Net.BCrypt.Verify(oPersonal.Password, personal.Password);

        //    if (!isValidPassword)
        //    {
        //        throw new Exception("Error logueo");
        //        return null;
        //    }

        //    return personal;

        //}

        public async Task<ResultadosApi> Signup(UsuarioDTO oPersonal)
        {
            oPersonal.Password = BCrypt.Net.BCrypt.HashPassword(oPersonal.Password);

            var r = new ResultadosApi();

            Usuario u = new Usuario
            {
                Email = oPersonal.Email,
                Password = oPersonal.Password,
                Activo = true
            };

            if (u != null)
            {
                var repetido = await context.Usuarios.FirstOrDefaultAsync(x => x.Email == oPersonal.Email);
                if (repetido == null)
                {
                    r.Ok = true;
                    r.Return = u;
                    context.Usuarios.Add(u);
                    await context.SaveChangesAsync();
                    return r;
                }
                else
                {
                    r.Ok = false;
                    r.Error = "Usuario ya registrado con ese mail";
                    r.Return = null;
                    return r;
                }
            }
            else
            {
                throw new Exception("No se pudo registrar el usuario");
            }

        }

        public bool UpdatePass(UsuarioUpdate usuario)
        {
            UsuarioUpdate u = new UsuarioUpdate
            {
                Email = usuario.Email,
                PasswordVieja = usuario.PasswordVieja,
                PasswordNueva = usuario.PasswordNueva
            };

            var usu = context.Usuarios.SingleOrDefault(x => x.Email == usuario.Email);
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(usuario.PasswordVieja, usu.Password);

            if (isValidPassword)
            {
                usu.Password = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordNueva);
                context.Usuarios.Update(usu);
                context.SaveChanges();
                return true;
            }
            return false;

        }

        public async Task<ResultadosApi> Login(UsuarioDTO oPersonal)
        {
            var r = new ResultadosApi();
            var personal = await context.Usuarios.SingleOrDefaultAsync(x => x.Email == oPersonal.Email && x.Activo == true);
            var contra = BCrypt.Net.BCrypt.HashPassword(oPersonal.Password);

            if (personal != null)
            {
                bool isValidPassword = BCrypt.Net.BCrypt.Verify(oPersonal.Password, personal.Password);
                if (isValidPassword)
                {
                    var usuarioSesion = new UsuarioSesionDto
                    {
                        IdUsuario = personal.Id,
                        Email = personal.Email,
                        Password = personal.Password,
                        Token = CrearToken(personal),
                    };
                    r.Ok = true;
                    r.Return = usuarioSesion;
                    return r;
                }
                else
                {
                    r.Ok = false;
                    r.Error = "Usuario y/o contraseña incorrectos";
                    r.Return = null;
                    return r;
                }

            }
            else
            {
                r.Ok = false;
                r.Error = "El Usuario Ingresado no se encuentra Registrado";
                r.Return = null;
                return r;
            }

        }

        //public Personal Signup(Personal oPersonal)
        //{
        //    oPersonal.Password = BCrypt.Net.BCrypt.HashPassword(oPersonal.Password);
        //    context.Personals.Add(oPersonal);
        //    context.SaveChanges();
        //    return oPersonal;
        //}

        //public List<Personal> GetUsuario()
        //{
        //    return context.Personals.ToList();
        //}
    }
}
