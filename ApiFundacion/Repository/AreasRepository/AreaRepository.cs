using ApiFundacion.Models;
using ApiFundacion.Models.DTO;
using ApiFundacion.Models.DTO.Areas;
using ApiFundacion.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiFundacion.Repository
{
    public class AreaRepository : IAreaRepository
    {
        private readonly dena66utud3alcContext context;

        public AreaRepository(dena66utud3alcContext context)
        {
            this.context = context;
        }

        public async Task<bool> CreateArea(AreaInsert area)
        {
            var areaDto = new Area
            {
                Area1 = area.Area,
                Activo = true
            };

            await context.Areas.AddAsync(areaDto);
            var valor = await context.SaveChangesAsync();

            if (valor == 0)
                return false;

            return true;
        }

        public async Task<bool> DeleteArea(int id)
        {
            var area = await context.Areas.FirstOrDefaultAsync(x => x.Id == id);

            if (area != null)
            {
                area.Activo = false;
                context.Areas.Update(area);
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public Area GetArea(int id)
        {
            return context.Areas.FirstOrDefault(x => x.Id.Equals(id));
        }

        public async Task<List<Area>> GetAreas()
        {
            return await context.Areas.Where(x => x.Id != 189 && x.Activo == true).OrderBy(x => x.Area1).ToListAsync();
        }

        public async Task<List<AreasxDepto>> GetAreasxDepto(string depto)
        {
            var lista = new List<AreasxDepto>();
            var proyectos = await context.Proyectos.Where(x => x.Activo == true).ToListAsync();

            var areaxProyetoBD = await context.Areasxproyectos.ToListAsync();
            var areaBD = await context.Areas.Where(x => x.Activo == true).ToListAsync();

            foreach (var i in proyectos)
            {
                var areaxproyecto = areaxProyetoBD.Where(x => x.Idproyecto == i.Id).ToList();

                foreach (var j in areaxproyecto)
                {
                    var area = areaBD.SingleOrDefault(x => x.Id == j.Idarea);
                    if (area != null)
                    {
                        var areaDto = new AreasxDepto
                        {
                            Area = area.Area1,
                            Id = area.Id,
                            Depto = i.Departamento
                        };
                        lista.Add(areaDto);
                    }
                }
            }
            if (depto != null)
            {
                lista = lista.Where(x => x.Depto == depto).ToList();
            }

            var listaSinDulicados = lista.OrderBy(x => x.Area).
                GroupBy(x => x.Id).Select(y => y.First()).ToList();

            return listaSinDulicados;
        }

        public bool Save()
        {
            return context.SaveChanges() > 0 ? true : false;
        }

        public async Task<bool> UpdateArea(AreaUpdate areaUpdate)
        {
            var area = await context.Areas.FirstOrDefaultAsync(x => x.Id == areaUpdate.Id);

            if (area != null)
            {
                area.Area1 = areaUpdate.Area ?? area.Area1;
                context.Areas.Update(area);
                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
