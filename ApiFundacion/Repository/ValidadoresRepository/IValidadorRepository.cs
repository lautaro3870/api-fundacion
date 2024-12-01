using ApiFundacion.Models.DTO.Validadores;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiFundacion.Repository.ValidadoresRepository
{
    public interface IValidadorRepository
    {
        Task<List<ValidadorDTO>> GetValidadores();
        Task<bool> CreateValidador(ValidadorDTO insert);
        Task<bool> UpdateValidador(ValidadorUpdate update);
        Task<bool> DeleteValidador(int id);
        Task<ValidadorDTO> GetValidador(int id);
    }
}
