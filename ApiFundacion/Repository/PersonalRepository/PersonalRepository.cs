using ApiFundacion.Models;
using ApiFundacion.Models.DTO.Personal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiFundacion.Repository.PersonalRepository
{
    public class PersonalRepository : IPersonalRepository
    {
        private readonly dena66utud3alcContext context;
        public PersonalRepository(dena66utud3alcContext context)
        {
            this.context = context;
        }

        public async Task<bool> Create(PersonalInsert personalInsert)
        {
            var personal = new Personal
            {
                Nombre = personalInsert.Nombre,
                Activo = true
            };

            if (personal != null)
            {
                context.Personals.Add(personal);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var personal = await context.Personals.FirstOrDefaultAsync(x => x.Id == id);
            if (personal != null)
            {
                personal.Activo = false;
                context.Personals.Update(personal);
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<PersonalDto>> GetPersonal()
        {
            var personal = await context.Personals.Where(x => x.Activo == true && x.Id != 33).OrderBy(x => x.Nombre).ToListAsync();
            var lista = new List<PersonalDto>();
            foreach (var i in personal)
            {
                var personalDto = new PersonalDto
                {
                    Id = i.Id,
                    Nombre = i.Nombre
                };
                lista.Add(personalDto);
            }
            return lista;
        }

        public async Task<List<Personal>> GetUnPersonal(int id)
        {
            var lista = new List<Personal>();
            var personal = await context.Personals.FirstOrDefaultAsync(x => x.Id == id && x.Activo == true);
            if (personal != null)
            {
                lista.Add(personal);
                return lista;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> Update(PersonalUpdate personalUpdate)
        {
            var personal = await context.Personals.FirstOrDefaultAsync(x => x.Id == personalUpdate.Id);
            if (personal != null)
            {
                personal.Nombre = personalUpdate.Nombre ?? personal.Nombre;
                
                context.Personals.Update(personal);
                var valor = await context.SaveChangesAsync();

                if (valor == 0)
                    return false;

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
