using ApiFundacion.Models;
using ApiFundacion.Models.DTO.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiFundacion.Repository.PersonalRepository
{
    public interface IPersonalRepository
    {
        Task<List<PersonalDto>> GetPersonal();
        Task<bool> Create(PersonalInsert personalInsert);
        Task<bool> Update(PersonalUpdate personalUpdate);
        Task<bool> Delete(int id);
        Task<List<Personal>> GetUnPersonal(int id);
    }
}
