﻿using ApiFundacion.Models;
using ApiFundacion.Models.DTO;
using ApiFundacion.Models.DTO.Personal;
using ApiFundacion.Repository.QueryFilters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ApiFundacion.Repository.Proyectos
{
    public class ProyectoRepository : IProyectoRepository
    {

        public readonly dena66utud3alcContext context;

        public ProyectoRepository(dena66utud3alcContext context)
        {
            this.context = context;
        }
        public async Task<bool> Create(ProyectoInsert proyecto)
        {
            Proyecto pro = new Proyecto();

            pro.Activo = proyecto.Activo;
            pro.AnioFinalizacion = proyecto.AnioFinalizacion;
            pro.AnioInicio = proyecto.AnioInicio;
            pro.SsmaTimestamp = new byte[5];
            pro.MontoContrato = proyecto.MontoContrato;
            pro.NroContrato = proyecto.NroContrato;
            pro.PaisRegion = proyecto.PaisRegion;
            pro.Titulo = proyecto.Titulo;
            pro.Certconformidad = proyecto.Certconformidad;
            pro.Certificadopor = proyecto.Certificadopor;
            pro.Moneda = proyecto.Moneda;
            pro.EnCurso = proyecto.EnCurso;
            pro.Descripcion = proyecto.Descripcion;
            pro.Departamento = proyecto.Departamento;
            pro.ConsultoresAsoc = proyecto.ConsultoresAsoc;
            pro.Resultados = proyecto.Resultados;
            pro.MesInicio = proyecto.MesInicio;
            pro.MesFinalizacion = proyecto.MesFinalizacion;
            pro.Contratante = proyecto.Contratante;
            pro.Dirección = proyecto.Dirección;
            pro.FichaLista = proyecto.FichaLista;
            pro.Link = proyecto.Link;
            pro.Convenio = proyecto.Convenio;
            pro.Cita = proyecto.Cita;
            pro.ISBN = proyecto.ISBN;
            pro.pdf = proyecto.pdf;
            pro.ISSN = proyecto.ISSN;
            pro.Revista = proyecto.Revista;

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    await context.Proyectos.AddAsync(pro);
                    var valor = await context.SaveChangesAsync();

                    if (valor == 0)
                    {
                        throw new Exception("No se pudo insertar el proyecto");
                    }

                    foreach (var i in proyecto.Areas)
                    {
                        Areasxproyecto area = new Areasxproyecto();
                        area.Idarea = i.Id;
                        area.Idproyecto = pro.Id;

                        await context.Areasxproyectos.AddAsync(area);

                    }

                    foreach (var j in proyecto.Personal)
                    {
                        Equipoxproyecto equipo = new Equipoxproyecto();
                        equipo.IdPersonal = j.Id;
                        equipo.IdProyecto = pro.Id;
                        equipo.Coordinador = j.Coordinador;
                        equipo.SubCoordinador = j.SubCoordinador;
                        equipo.ConsultorAsociado = j.ConsultorAsociado;
                        equipo.Investigador = j.Investigador;
                        equipo.SsmaTimestamp = new byte[5];

                        await context.Equipoxproyectos.AddAsync(equipo);
                    }

                    foreach (var t in proyecto.Publicaciones)
                    {
                        Publicacionesxproyecto publi = new Publicacionesxproyecto();
                        publi.IdProyecto = pro.Id;
                        publi.Publicacion = t.Publicacion;
                        publi.Año = t.Año;
                        publi.Codigobcs = t.Codigobcs;

                        await context.Publicacionesxproyectos.AddAsync(publi);
                        await context.SaveChangesAsync();
                    }

                    await context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;

                } catch (Exception ex) {
                    await transaction.RollbackAsync();
                    throw new Exception("No se pudo insertar el proyecto con área, personal o publicaciones", ex);
                }
            }
        }

        public async Task<List<ProyectoDTO>> GetProyectosFilter(ProyectosQueryFilter filters)
        {
            var proyectos = await context.Proyectos.Where(i => i.Activo.Equals(true)).OrderByDescending(x => x.Id).ToListAsync();
            var areasxproyectosBD = await context.Areasxproyectos.ToListAsync();
            var equipoxproyectoBD = context.Equipoxproyectos.ToList();
            
            var areaBD = await context.Areas.ToListAsync();
            var equipoBD = await context.Personals.ToListAsync();

            var listProyectoDto = new List<ProyectoDTO>();
            var equipoxproyecto = new List<Equipoxproyecto>();

            foreach (var i in proyectos)
            {
                var areaxProyecto = areasxproyectosBD.Where(x => x.Idproyecto == i.Id).ToList();
                var listaAreaDto = new List<AreasDTO>();
                foreach (var j in areaxProyecto)
                {
                    var area = areaBD.FirstOrDefault(x => x.Id == j.Idarea);

                    if (area != null)
                    {
                        var areaDto = new AreasDTO
                        {
                            Id = area.Id,
                            Area1 = area.Area1
                        };
                        listaAreaDto.Add(areaDto);
                    }
                }

                equipoxproyecto = equipoxproyectoBD.Where(x => x.IdProyecto == i.Id).ToList();
                var listaPersonal = new List<PersonalDto>();

                foreach (var item in equipoxproyecto)
                {
                    var persona = equipoBD.FirstOrDefault(x => x.Id == item.IdPersonal);
                    if(persona != null)
                    {
                        var personalDto = new PersonalDto
                        {
                            Nombre = persona.Nombre,
                            Coordinador = item.Coordinador,
                            SubCoordinador = item.SubCoordinador,
                            ConsultorAsociado = item.ConsultorAsociado,
                            Investigador = item.Investigador,
                        };
                        listaPersonal.Add(personalDto);
                    }
                }

                var proyectoDto = new ProyectoDTO
                {
                    Id = i.Id,
                    Titulo = i.Titulo,
                    AnioFinalizacion = i.AnioFinalizacion,
                    AnioInicio = i.AnioInicio,
                    Departamentos = i.Departamento,
                    ListaAreas = listaAreaDto,
                    FichaLista = i.FichaLista,
                    MesFinalizacion = i.MesFinalizacion,
                    MesInicio = i.MesInicio,
                    PaisRegion = i.PaisRegion,
                    MontoContrato = i.MontoContrato,
                    Moneda = i.Moneda,
                    Link = i.Link,
                    Convenio = i.Convenio,
                    Contratante = i.Contratante,
                    ListaPersonal = listaPersonal,
                    Descripcion = i.Descripcion,
                    Cita = i.Cita,
                    ISBN = i.ISBN,
                    ISSN = i.ISSN,
                    pdf = i.pdf,
                    Revista = i.Revista
                };

                listProyectoDto.Add(proyectoDto);
            }

            if (filters.Area != null)
            {
                areasxproyectosBD = areasxproyectosBD.Where(x => x.Idarea == filters.Area).ToList();

                var listaProyectoDtoFilter = new List<ProyectoDTO>();

                Proyecto p = null;

                foreach (var j in areasxproyectosBD)
                {
                    var listAreaDtoFilter = new List<AreasDTO>();
                    var areas = new List<Area>();
                    var personales = new List<Personal>();
                    p = await context.Proyectos.FirstOrDefaultAsync(x => x.Id == j.Idproyecto);

                    foreach (var k in p.Areasxproyectos)
                    {
                        var area = await context.Areas.FirstOrDefaultAsync(x => x.Id == k.Idarea);
                        areas.Add(area);
                    }

                    foreach (var l in areas)
                    {
                        var areaDTO = new AreasDTO
                        {
                            Id = l.Id,
                            Area1 = l.Area1
                        };
                        listAreaDtoFilter.Add(areaDTO);

                    }

                    var pDto = new ProyectoDTO
                    {
                        Id = p.Id,
                        Titulo = p.Titulo,
                        AnioFinalizacion = p.AnioFinalizacion,
                        AnioInicio = p.AnioInicio,
                        Departamentos = p.Departamento,
                        ListaAreas = listAreaDtoFilter,
                        FichaLista = p.FichaLista,
                        MesFinalizacion = p.MesFinalizacion,
                        MesInicio = p.MesInicio,
                        PaisRegion = p.PaisRegion,
                        MontoContrato = p.MontoContrato,
                        Moneda = p.Moneda,
                        Link = p.Link,
                        Convenio = p.Convenio,
                        Contratante = p.Contratante,
                        Descripcion = p.Descripcion,
                        Cita = p.Cita,
                        ISBN = p.ISBN,
                        ISSN = p.ISSN,
                        pdf = p.pdf,
                        Revista = p.Revista
                    };
                    
                    listaProyectoDtoFilter.Add(pDto);
                    areas.Clear();

                    equipoxproyecto = equipoxproyectoBD.Where(x => x.IdProyecto == p.Id).ToList();
                    var listaPersonalFilter = new List<PersonalDto>();

                    foreach (var l in equipoxproyecto)
                    {
                        var personal2 = equipoBD.FirstOrDefault(x => x.Id == l.IdPersonal);
                        if(personal2 != null)
                        {
                            personales.Add(personal2);

                            foreach (var item2 in personales)
                            {
                                var personalDto = new PersonalDto
                                {
                                    Nombre = personal2.Nombre,
                                    Coordinador = l.Coordinador,
                                    Investigador = l.Investigador,
                                    ConsultorAsociado = l.ConsultorAsociado,
                                    SubCoordinador = l.SubCoordinador,
                                };
                                listaPersonalFilter.Add(personalDto);
                            }
                            personales.Clear();
                        }
                    }
                    pDto.ListaPersonal = listaPersonalFilter;
                }
                
                listProyectoDto = listaProyectoDtoFilter;
            }

            if (filters.Id != null)
            {
                listProyectoDto = listProyectoDto.Where(x => x.Id == filters.Id).ToList();
            }
            if (filters.AnioInicio != null && filters.AnioInicio != null && filters.AnioInicio <= filters.AnioFin)
            {
                listProyectoDto = listProyectoDto.Where(x => x.AnioInicio >= filters.AnioInicio && x.AnioFinalizacion <= filters.AnioFin).ToList();
            }
            if (filters.MesInicio != null)
            {
                listProyectoDto = listProyectoDto.Where(x => x.MesInicio == filters.MesInicio).ToList();
            }
            if (filters.MesFin != null)
            {
                listProyectoDto = listProyectoDto.Where(x => x.MesFinalizacion == filters.MesFin).ToList();
            }
            if (filters.Titulo != null)
            {
                listProyectoDto = listProyectoDto.Where(x => x.Titulo == filters.Titulo).ToList();
            }
            if (filters.Pais != null)
            {
                listProyectoDto = listProyectoDto.Where(x => x.PaisRegion.ToLower().Contains(filters.Pais)).ToList();
            }
            if (filters.Departamento != null)
            {
                listProyectoDto = listProyectoDto.Where(x => x.Departamentos == filters.Departamento).ToList();
            }
            if (filters.pdf != null)
            {
                listProyectoDto = listProyectoDto.Where(x => x.pdf == filters.pdf).ToList();
            }
            if (filters.ISSN != null)
            {
                listProyectoDto = listProyectoDto.Where(x => x.ISSN == filters.ISSN).ToList();
            }
            if (filters.ISBN != null)
            {
                listProyectoDto = listProyectoDto.Where(x => x.ISBN == filters.ISBN).ToList();
            }
            if (filters.Cita != null)
            {
                listProyectoDto = listProyectoDto.Where(x => x.Cita == filters.Cita).ToList();
            }
            if (filters.Link != null)
            {
                listProyectoDto = (bool)filters.Link ? listProyectoDto.Where(x => !string.IsNullOrEmpty(x.Link)).ToList() : listProyectoDto.Where(x => string.IsNullOrEmpty(x.Link)).ToList();
            }

            return listProyectoDto;
        }

        public async Task<List<ProyectoTablaDTO>> GetProyectos()
        {
            //return await context.Proyectos.ToListAsync();
            var proyectos = await context.Proyectos.OrderBy(x => x.Id).Where(j => j.Activo == true).ToListAsync();
            var areaxProyectoDB = await context.Areasxproyectos.ToListAsync();
            var areaDB = await context.Areas.ToListAsync();

            var listProyectoDto = new List<ProyectoTablaDTO>();
            
            foreach (var i in proyectos)
            {
                var areaxProyecto = areaxProyectoDB.Where(x => x.Idproyecto == i.Id).ToList();
                var listaAreaDto = new List<AreaTablaDTO>();
                foreach (var j in areaxProyecto)
                {
                    var area = areaDB.FirstOrDefault(x => x.Id == j.Idarea);

                    if (area != null)
                    {
                        var areaDto = new AreaTablaDTO
                        {
                            Area1 = area.Area1
                        };
                        listaAreaDto.Add(areaDto);
                    }
                }
                var proyectoDto = new ProyectoTablaDTO
                {
                    Id = i.Id,
                    Titulo = i.Titulo,
                    AnioFinalizacion = i.AnioFinalizacion,
                    AnioInicio = i.AnioInicio,
                    Departamentos = i.Departamento,
                    ListaAreas = listaAreaDto,
                    FichaLista = i.FichaLista,
                    MesFinalizacion = i.MesFinalizacion,
                    MesInicio = i.MesInicio,
                    PaisRegion = i.PaisRegion,  
                };

                listProyectoDto.Add(proyectoDto);
            }
            return listProyectoDto;

        }

        public async Task<bool> Update(ProyectoUpdate proyecto)
        {
            var pro = context.Proyectos.FirstOrDefault(x => x.Id == proyecto.Id);

            if (pro == null)
            {
                throw new Exception("Proyecto no encotrado");
            }
            else
            {
                pro.Activo = proyecto.Activo ?? pro.Activo;
                pro.AnioFinalizacion = proyecto.AnioFinalizacion ?? pro.AnioFinalizacion;
                pro.AnioInicio = proyecto.AnioInicio ?? pro.AnioInicio;
                pro.SsmaTimestamp = new byte[5];
                pro.MontoContrato = proyecto.MontoContrato ?? pro.MontoContrato;
                pro.NroContrato = proyecto.NroContrato ?? pro.NroContrato;
                pro.PaisRegion = proyecto.PaisRegion ?? pro.PaisRegion;
                pro.Titulo = proyecto.Titulo ?? pro.Titulo;
                pro.Certconformidad = proyecto.Certconformidad ?? pro.Certconformidad;
                pro.Certificadopor = proyecto.Certificadopor ?? pro.Certificadopor;
                pro.Moneda = proyecto.Moneda ?? pro.Moneda;
                pro.EnCurso = proyecto.EnCurso ?? pro.EnCurso;
                pro.Descripcion = proyecto.Descripcion ?? pro.Descripcion;
                pro.Departamento = proyecto.Departamento ?? pro.Departamento;
                pro.ConsultoresAsoc = proyecto.ConsultoresAsoc ?? pro.ConsultoresAsoc;
                pro.Resultados = proyecto.Resultados ?? pro.Resultados;
                pro.MesInicio = proyecto.MesInicio ?? pro.MesInicio;
                pro.MesFinalizacion = proyecto.MesFinalizacion ?? pro.MesFinalizacion;
                pro.Contratante = proyecto.Contratante ?? pro.Contratante;
                pro.Dirección = proyecto.Dirección ?? pro.Dirección;
                pro.FichaLista = proyecto.FichaLista ?? pro.FichaLista;
                pro.Link = proyecto.Link ?? pro.Link;
                pro.Convenio = proyecto.Convenio ?? pro.Convenio;
                pro.pdf = proyecto.pdf ?? pro.pdf;
                pro.ISSN = proyecto.ISSN ?? pro.ISSN;
                pro.ISBN = proyecto.ISBN ?? pro.ISBN;
                pro.Cita = proyecto.Cita ?? pro.Cita;
                pro.Revista = proyecto.Revista ?? pro.Revista;

                var areaProyecto = await context.Areasxproyectos.Where(x => x.Idproyecto == proyecto.Id).ToListAsync();
                foreach(var j in areaProyecto)
                {
                    context.Areasxproyectos.Remove(j);
                    await context.SaveChangesAsync();
                }

                foreach (var i in proyecto.Areas)
                {
                    Areasxproyecto area = new Areasxproyecto();
                    area.Idarea = i.Id;
                    area.Idproyecto = pro.Id;
                    await context.Areasxproyectos.AddAsync(area);
                }

                var personalProyecto = await context.Equipoxproyectos.Where(x => x.IdProyecto == proyecto.Id).ToListAsync();
                foreach(var i in personalProyecto)
                {
                    context.Equipoxproyectos.Remove(i);
                    await context.SaveChangesAsync();
                }

                foreach (var j in proyecto.Personal)
                {
                    Equipoxproyecto equipo = new Equipoxproyecto();
                    equipo.IdPersonal = j.Id;
                    equipo.IdProyecto = pro.Id;
                    equipo.Coordinador = j.Coordinador;
                    equipo.SubCoordinador = j.SubCoordinador;
                    equipo.ConsultorAsociado = j.ConsultorAsociado;
                    equipo.Investigador = j.Investigador;
                    equipo.SsmaTimestamp = new byte[5];
                    await context.Equipoxproyectos.AddAsync(equipo);
                }

                var publiProyecto = await context.Publicacionesxproyectos.Where(x => x.IdProyecto == proyecto.Id).ToListAsync();

                foreach (var item in publiProyecto)
                {
                    context.Publicacionesxproyectos.Remove(item);
                    await context.SaveChangesAsync();
                }

                foreach (var t in proyecto.Publicaciones)
                {
                    Publicacionesxproyecto publi = new Publicacionesxproyecto();
                    publi.IdPublicacion = t.IdPublicacion;
                    publi.IdProyecto = pro.Id;
                    publi.Publicacion = t.Publicacion;
                    publi.Año = t.Año;
                    publi.Codigobcs = t.Codigobcs;

                    await context.Publicacionesxproyectos.AddAsync(publi);
                    await context.SaveChangesAsync();
                }

                context.Proyectos.Update(pro);
                var valor = await context.SaveChangesAsync();

                if (valor == 0)
                {
                    throw new Exception("Proyecto no se pudo actualizar");
                }
                return true;

            }

        }

        public async Task<bool> Delete(int id)
        {
            //var proyectos = await context.Proyectos.Where(i => i.Activo.Equals(true)).OrderBy(x => x.Id).ToListAsync();
            var proyecto = await context.Proyectos.FirstOrDefaultAsync(x => x.Id == id);

            if (proyecto == null)
            {
                throw new Exception("Proyecto no encontrado");
            }
            else
            {
                proyecto.Activo = false;
                //context.Proyectos.Update(proyecto);
                await context.SaveChangesAsync();
                return true;
            }
        }

        private string QuitarTildes(string texto)
        {
            if (texto != null)
            {
                var textoNormalizado = texto.Normalize(NormalizationForm.FormD);
                var stringBuilder = new StringBuilder();

                foreach (var c in textoNormalizado)
                {
                    var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                    if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                    {
                        stringBuilder.Append(c);
                    }
                }

                return stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLower();
            }
            return "";
        }

        public async Task<List<ProyectoDTO>> GetSearchedText(string texto)
        {
            var textoNormalizado = QuitarTildes(texto);
            var proyectos = await context.Proyectos.Where(e => e.Activo == true).ToListAsync();
            var proyectosFiltrados = proyectos
                .Where(p => QuitarTildes(p.Titulo).Contains(textoNormalizado))
                .ToList();

            var areasxproyectosBD = await context.Areasxproyectos.ToListAsync();
            var equipoxproyectoBD = context.Equipoxproyectos.ToList();

            var areaBD = await context.Areas.ToListAsync();
            var equipoBD = await context.Personals.ToListAsync();

            var listProyectoDto = new List<ProyectoDTO>();
            var equipoxproyecto = new List<Equipoxproyecto>();

            foreach (var i in proyectosFiltrados)
            {
                var areaxProyecto = areasxproyectosBD.Where(x => x.Idproyecto == i.Id).ToList();
                var listaAreaDto = new List<AreasDTO>();
                foreach (var j in areaxProyecto)
                {
                    var area = areaBD.FirstOrDefault(x => x.Id == j.Idarea);

                    if (area != null)
                    {
                        var areaDto = new AreasDTO
                        {
                            Id = area.Id,
                            Area1 = area.Area1
                        };
                        listaAreaDto.Add(areaDto);
                    }
                }

                equipoxproyecto = equipoxproyectoBD.Where(x => x.IdProyecto == i.Id).ToList();
                var listaPersonal = new List<PersonalDto>();

                foreach (var item in equipoxproyecto)
                {
                    var persona = equipoBD.FirstOrDefault(x => x.Id == item.IdPersonal);
                    if (persona != null)
                    {
                        var personalDto = new PersonalDto
                        {
                            Nombre = persona.Nombre,
                            Coordinador = item.Coordinador,
                            SubCoordinador = item.SubCoordinador,
                            ConsultorAsociado = item.ConsultorAsociado,
                            Investigador = item.Investigador,
                        };
                        listaPersonal.Add(personalDto);
                    }
                }

                var proyectoDto = new ProyectoDTO
                {
                    Id = i.Id,
                    Titulo = i.Titulo,
                    AnioFinalizacion = i.AnioFinalizacion,
                    AnioInicio = i.AnioInicio,
                    Departamentos = i.Departamento,
                    ListaAreas = listaAreaDto,
                    FichaLista = i.FichaLista,
                    MesFinalizacion = i.MesFinalizacion,
                    MesInicio = i.MesInicio,
                    PaisRegion = i.PaisRegion,
                    MontoContrato = i.MontoContrato,
                    Moneda = i.Moneda,
                    Link = i.Link,
                    Convenio = i.Convenio,
                    Contratante = i.Contratante,
                    ListaPersonal = listaPersonal,
                    Descripcion = i.Descripcion,
                    Cita = i.Cita,
                    ISBN = i.ISBN,
                    ISSN = i.ISSN,
                    pdf = i.pdf,
                    Revista = i.Revista
                };

                listProyectoDto.Add(proyectoDto);
            }
            return listProyectoDto;
        }

        public async Task<List<ProyectoIdDTO>> GetProyectosId(int id)
        {
            var proyectos = await context.Proyectos.Where(i => i.Activo.Equals(true) && i.Id == id).OrderBy(x => x.Id).ToListAsync();
            var areasxproyectosBD = await context.Areasxproyectos.ToListAsync();
            var personalxproyectoBD = await context.Equipoxproyectos.ToListAsync();
            var publicacionxProyectoBD = await context.Publicacionesxproyectos.ToListAsync();
            var areaBD = await context.Areas.ToListAsync();
            var personalBD = await context.Personals.ToListAsync();
            var publiBD = await context.Publicacionesxproyectos.ToListAsync();

            var listProyectoDto = new List<ProyectoIdDTO>();

            foreach (var i in proyectos)
            {
                //var areaxProyecto = await context.Areasxproyectos.Where(x => x.Idproyecto == i.Id).ToListAsync();
                var areaxProyecto = areasxproyectosBD.Where(x => x.Idproyecto == i.Id).ToList();
                var equipoxProyecto = personalxproyectoBD.Where(x => x.IdProyecto == i.Id).ToList();
                var publicacionxProyecto = publicacionxProyectoBD.Where(x => x.IdProyecto == i.Id).ToList();
                var listaAreaDto = new List<AreasDTO>();
                var listaEquipoDto = new List<UsuarioIdDTO>();
                var listaPublicacionesDto = new List<PublicacionesDTO>();

                foreach (var j in areaxProyecto)
                {
                    //var area = await context.Areas.FirstOrDefaultAsync(x => x.Id == j.Idarea);
                    var area = areaBD.FirstOrDefault(x => x.Id == j.Idarea);

                    if (area != null)
                    {
                        var areaDto = new AreasDTO
                        {
                            Id = area.Id,
                            Area1 = area.Area1
                        };
                        listaAreaDto.Add(areaDto);
                    }
                }

                foreach (var k in equipoxProyecto)
                {
                    //var area = await context.Areas.FirstOrDefaultAsync(x => x.Id == j.Idarea);
                    var personal = personalBD.FirstOrDefault(x => x.Id == k.IdPersonal);

                    var equipo = equipoxProyecto.FirstOrDefault(j => j.IdPersonal == k.IdPersonal);

                    if (personal != null)
                    {
                        var personalDto = new UsuarioIdDTO
                        {
                            Id = personal.Id,
                            Nombre = personal.Nombre,
                            Titulo = personal.Titulo,
                            Coordinador = equipo.Coordinador,
                            SubCoordinador = equipo.SubCoordinador,
                            Investigador = equipo.Investigador,
                            ConsultorAsociado = equipo.ConsultorAsociado,
                        };
                        
                        listaEquipoDto.Add(personalDto);
                    }
                }

                foreach (var item in publicacionxProyecto)
                {
                    var publicacion = publiBD.FirstOrDefault(x => x.IdPublicacion == item.IdPublicacion);
                    if (publicacion != null)
                    {
                        var publiDTO = new PublicacionesDTO
                        {
                            IdPublicacion = item.IdPublicacion,
                            IdProyecto = item.IdProyecto,
                            Año = item.Año,
                            Codigobcs = item.Codigobcs,
                            Publicacion = item.Publicacion
                        };
                        listaPublicacionesDto.Add(publiDTO);
                    }
                    
                }

                var proyectoDto = new ProyectoIdDTO
                {
                    Id = i.Id,
                    Titulo = i.Titulo,
                    AnioFinalizacion = i.AnioFinalizacion,
                    AnioInicio = i.AnioInicio,
                    Departamento = i.Departamento,
                    ListaAreas = listaAreaDto,
                    ListaPersonal = listaEquipoDto,
                    FichaLista = i.FichaLista,
                    MesFinalizacion = i.MesFinalizacion,
                    MesInicio = i.MesInicio,
                    PaisRegion = i.PaisRegion,
                    Activo = i.Activo,
                    Certificadopor = i.Certificadopor,
                    Certconformidad = i.Certconformidad,
                    Moneda = i.Moneda,
                    EnCurso = i.EnCurso,
                    Resultados = i.Resultados,
                    Descripcion = i.Descripcion,
                    MontoContrato = i.MontoContrato,
                    ConsultoresAsoc = i.ConsultoresAsoc,
                    NroContrato = i.NroContrato,
                    Dirección = i.Dirección,
                    Contratante = i.Contratante,
                    Link = i.Link,
                    ListaPublicaciones = listaPublicacionesDto,
                    Convenio = i.Convenio,
                    Cita = i.Cita,
                    ISBN = i.ISBN,
                    ISSN = i.ISSN,
                    pdf = i.pdf,
                    Revista = i.Revista,
                };

                listProyectoDto.Add(proyectoDto);
            }

            return listProyectoDto;
        }
    }
}
