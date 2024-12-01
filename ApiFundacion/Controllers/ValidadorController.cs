using ApiFundacion.Models.DTO.Validadores;
using ApiFundacion.Repository.ValidadoresRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiFundacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("Prog3")]
    [Authorize]
    public class ValidadorController : ControllerBase
    {
        private readonly IValidadorRepository validadorRepository;

        public ValidadorController(IValidadorRepository validadorRepository)
        {
            this.validadorRepository = validadorRepository;
        }

        [HttpGet]
        public async Task<List<ValidadorDTO>> GetValidadores()
        {
            return await validadorRepository.GetValidadores();
        }

        [HttpGet("{id}")]
        public async Task<ValidadorDTO> GetValidador(int id)
        {
            return await validadorRepository.GetValidador(id);
        }

        [HttpPost]  
        public async Task<bool> Insert(ValidadorDTO insert)
        {
            return await validadorRepository.CreateValidador(insert);
        }

        [HttpPut]
        public async Task<bool> Update(ValidadorUpdate update)
        {
            return await validadorRepository.UpdateValidador(update);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await validadorRepository.DeleteValidador(id);
        }
    }
}
