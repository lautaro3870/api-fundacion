﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiFundacion.Models.DTO
{
    public class UsuarioUpdate
    {
        public string Email { get; set; }
        public string PasswordVieja { get; set; }
        public string PasswordNueva { get; set; }

    }
}
