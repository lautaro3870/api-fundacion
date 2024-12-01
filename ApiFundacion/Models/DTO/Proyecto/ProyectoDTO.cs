using ApiFundacion.Models.DTO.Personal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiFundacion.Models.DTO
{
    public class ProyectoDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string PaisRegion { get; set; }
        public int? MesInicio { get; set; }
        public int? AnioInicio { get; set; }
        public int? MesFinalizacion { get; set; }
        public int? AnioFinalizacion { get; set; }
        public string MontoContrato { get; set; }
        public string Moneda { get; set; }
        //public string ConsultoresAsoc { get; set; }
        public bool? FichaLista { get; set; }
        public List<AreasDTO> ListaAreas { get; set; }
        public List<PersonalDto> ListaPersonal { get; set; }
        public string Departamentos { get; set; }
        public string Link { get; set; }
        public bool? Convenio { get; set; }
        public string Contratante { get; set; }
        public string Descripcion { get; set; }
        public string pdf { get; set; }
        public string ISBN { get; set; }
        public string Cita { get; set; }
        public string ISSN { get; set; }
        public string Revista { get; set; }
    }
}
