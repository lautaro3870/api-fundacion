﻿using ApiFundacion.Models;
using ApiFundacion.Models.DTO;
using ApiFundacion.Repository.Proyectos;
using ApiFundacion.Repository.QueryFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize]
    public class ProyectoController : ControllerBase
    {

        private readonly IProyectoRepository proyectorepository;

        public ProyectoController(IProyectoRepository proyectorepository)
        {
            this.proyectorepository = proyectorepository;

        }

        [HttpGet]
        public async Task<ActionResult<List<ProyectoDTO>>> GetFilters([FromQuery] ProyectosQueryFilter filters)
        {
            return await proyectorepository.GetProyectosFilter(filters);
        }

        [HttpGet("tabla")]
        public async Task<ActionResult<List<ProyectoTablaDTO>>> GetProyectos()
        {
            return await proyectorepository.GetProyectos();
        }

        [HttpGet("texto")]
        public async Task<ActionResult<List<ProyectoDTO>>> GetProyetosSearchedText(string texto)
        {
            return await proyectorepository.GetSearchedText(texto);
        }

        // GET api/<ProyectoController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<ProyectoIdDTO>>> GetProyectoId(int id)
        {
            return await proyectorepository.GetProyectosId(id);
        }

        // POST api/<ProyectoController>
        [HttpPost]
        public async Task<bool> Post(ProyectoInsert proyecto)
        {
            return await proyectorepository.Create(proyecto);
        }

        // PUT api/<ProyectoController>/5
        [HttpPut]
        public async Task<bool> Put(ProyectoUpdate proyecto)
        {
            return await proyectorepository.Update(proyecto);
        }

        // DELETE api/<ProyectoController>/5
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await proyectorepository.Delete(id);
        }

        
    }
}
