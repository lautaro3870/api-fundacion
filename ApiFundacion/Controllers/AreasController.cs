using ApiFundacion.Models;
using ApiFundacion.Models.DTO;
using ApiFundacion.Models.DTO.Areas;
using ApiFundacion.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiFundacion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AreasController : ControllerBase
    {
        private readonly IAreaRepository areaRepository;
        private readonly IMapper mapper;

        public AreasController(IAreaRepository areaRepository, IMapper mapper)
        {
            this.areaRepository = areaRepository;
            this.mapper = mapper;
        }

        [HttpGet("AreasxDepto")]
        public async Task<List<AreasxDepto>> GetAreasxDepto([FromQuery] string depto)
        {
            return await areaRepository.GetAreasxDepto(depto);
        }
        
        [HttpGet]
        public async Task<List<Area>> GetAreas()
        {
            return await areaRepository.GetAreas();
        }

        //// GET api/<AreasController>/5
        [HttpGet("{id}")]
        public IActionResult GetId(int id)
        {
            var area = areaRepository.GetArea(id);
            return Ok(area);
        }

        [HttpPost]
        public async Task<bool> Post(AreaInsert area)
        {
            return await areaRepository.CreateArea(area);
        }

        [HttpPut]
        public async Task<bool> Put(AreaUpdate areaUpdate)
        {
            return await areaRepository.UpdateArea(areaUpdate);
        }

        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await areaRepository.DeleteArea(id);
        }
    }
}
