using ApiFundacion.Models;
using ApiFundacion.Models.DTO;
using ApiFundacion.Models.DTO.Areas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiFundacion.Repository.IRepository
{
    public interface IAreaRepository
    {
        Task<List<Area>> GetAreas();
        Area GetArea(int id);
        Task<bool> CreateArea(AreaInsert area);
        Task<bool> UpdateArea(AreaUpdate areaUpdate);
        Task<bool> DeleteArea(int id);
        bool Save();
        Task<List<AreasxDepto>> GetAreasxDepto(string depto);

    }
}
