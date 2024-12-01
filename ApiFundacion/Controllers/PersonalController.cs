using ApiFundacion.Models;
using ApiFundacion.Models.DTO;
using ApiFundacion.Models.DTO.Personal;
using ApiFundacion.Repository.PersonalRepository;
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
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Prog3")]
    [Authorize]
    public class PersonalController : ControllerBase
    {
        private readonly IPersonalRepository personalRepository;

        public PersonalController(IPersonalRepository personalRepository)
        {
            this.personalRepository = personalRepository;
        }

        // GET: api/<PersonalController>
        [HttpGet]
        public async Task<List<PersonalDto>> GetPersonal()
        {
            return await personalRepository.GetPersonal();
        }

        [HttpGet("{id}")]
        public async Task<List<Personal>> GetUnPersonal(int id)
        {
            return await personalRepository.GetUnPersonal(id);
        }

        [HttpPost]
        public async Task<bool> Create(PersonalInsert personalInsert)
        {
            return await personalRepository.Create(personalInsert);
        }

        [HttpPut]
        public async Task<bool> Update(PersonalUpdate personalUpdate)
        {
            return await personalRepository.Update(personalUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await personalRepository.Delete(id);
        }

    }
}
