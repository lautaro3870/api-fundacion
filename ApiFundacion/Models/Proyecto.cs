﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ApiFundacion.Models
{
    public partial class Proyecto
    {
        public Proyecto()
        {
            Areasxproyectos = new HashSet<Areasxproyecto>();
            Equipoxproyectos = new HashSet<Equipoxproyecto>();
            Publicacionesxproyectos = new HashSet<Publicacionesxproyecto>();
        }

        public int Id { get; set; }
        public int? IdArea { get; set; }
        public string Titulo { get; set; }
        public string PaisRegion { get; set; }
        public string Contratante { get; set; }
        public string Dirección { get; set; }
        public string MontoContrato { get; set; }
        public string NroContrato { get; set; }
        public int? MesInicio { get; set; }
        public int? AnioInicio { get; set; }
        public int? MesFinalizacion { get; set; }
        public int? AnioFinalizacion { get; set; }
        public string ConsultoresAsoc { get; set; }
        public string Descripcion { get; set; }
        public string Resultados { get; set; }
        public bool? FichaLista { get; set; }
        public bool? EnCurso { get; set; }
        public string Departamento { get; set; }
        public string Moneda { get; set; }
        public bool? Certconformidad { get; set; }
        public int? Certificadopor { get; set; }
        public byte[] SsmaTimestamp { get; set; }
        public char? Trial118 { get; set; }
        public bool? Activo { get; set; }
        public string Link { get; set; }
        public bool? Convenio { get; set; }
        public string pdf { get; set; }
        public string ISBN { get; set; }
        public string Cita { get; set; }
        public string ISSN { get; set; }
        public string Revista { get; set; }
        public virtual ICollection<Areasxproyecto> Areasxproyectos { get; set; }
        public virtual ICollection<Equipoxproyecto> Equipoxproyectos { get; set; }
        public virtual ICollection<Publicacionesxproyecto> Publicacionesxproyectos { get; set; }
    }
}
