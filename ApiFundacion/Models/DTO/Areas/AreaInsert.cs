using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiFundacion.Models.DTO
{
    public class AreaInsert
    {
        //mensaje
        public int Id { get; set; }
        public string Area { get; set; }
        public bool Activo { get; set; }
    }
}
