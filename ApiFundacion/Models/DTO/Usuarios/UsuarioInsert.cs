using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiFundacion.Models.DTO
{
    public class UsuarioInsert
    {
        public int Id { get; set; }
        public bool Coordinador { get; set; }
        public bool? SubCoordinador { get; set; }
        public bool? Investigador { get; set; }
        public bool? ConsultorAsociado { get; set; }
    }
}
