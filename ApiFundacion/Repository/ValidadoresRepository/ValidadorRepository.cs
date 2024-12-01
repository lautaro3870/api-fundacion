using ApiFundacion.Models;
using ApiFundacion.Models.DTO.Validadores;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiFundacion.Repository.ValidadoresRepository
{
    public class ValidadorRepository : IValidadorRepository
    {
        private readonly dena66utud3alcContext context;

        public ValidadorRepository(dena66utud3alcContext context)
        {
            this.context = context;
        }

        public async Task<bool> CreateValidador(ValidadorDTO insert)
        {
            var validador = new Validadore();

            validador.Nombre = insert.Nombre;

            if (validador != null)
            {
                await context.Validadores.AddAsync(validador);
                var valor = await context.SaveChangesAsync();
                
                if (valor == 1)
                    return true;
                return false;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteValidador(int id)
        {
            var validador = await context.Validadores.FirstOrDefaultAsync(x => x.Id == id);

            if (validador != null)
            {
                
                context.Validadores.Remove(validador);
                var valor = await context.SaveChangesAsync();

                if (valor == 1)
                    return true;
                return false;
            }
            else
            {
                return false;
            }
        }

        public async Task<ValidadorDTO> GetValidador(int id)
        {
            var validador = await context.Validadores.FindAsync(id);

            if (validador != null)
            {
                var vDto = new ValidadorDTO
                {
                    Id = validador.Id,
                    Nombre = validador.Nombre
                };
                return vDto;
            }
            else
            {
                throw new Exception("Validador no encontrado");
            }
        }

        public async Task<List<ValidadorDTO>> GetValidadores()
        {
            var validadores = await context.Validadores.ToListAsync();

            var lista = new List<ValidadorDTO>();

            foreach (var i in validadores)
            {
                var validadorDto = new ValidadorDTO
                {
                    Id = i.Id,
                    Nombre = i.Nombre
                };
                lista.Add(validadorDto);
            }

            return lista;
        }

        public async Task<bool> UpdateValidador(ValidadorUpdate update)
        {
            var validador = await context.Validadores.FirstOrDefaultAsync(x => x.Id == update.Id);

            if (validador != null)
            {
                validador.Nombre = update.Nombre ?? validador.Nombre;

                context.Validadores.Update(validador);
                var valor = await context.SaveChangesAsync();

                if (valor == 1)
                    return true;
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
