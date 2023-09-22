using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TareasMVC.Entidades;
using TareasMVC.Models;
using TareasMVC.Servicios;

namespace TareasMVC.Controllers
{
    [Route("api/tareas")]
    public class TareasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IServicioUsuarios servicioUsuarios;

        public TareasController(ApplicationDbContext context, IMapper mapper,
            IServicioUsuarios servicioUsuarios)
        {
            this.context = context;
            this.mapper = mapper;
            this.servicioUsuarios = servicioUsuarios;
        }
        [HttpGet]
        public async Task<ActionResult<List<TareaDTO>>> Get()
        {

            var usuarioId = servicioUsuarios.ObtenerUsuarioId();
            var tareas = await context.Tareas.Where(t => t.UsuariosCreacionId == usuarioId)
                .OrderBy(t => t.Orden)
                .ProjectTo<TareaDTO>(mapper.ConfigurationProvider).ToListAsync();
            return tareas;
        }

        [HttpPost]
        public async Task<ActionResult<Tarea>> Post([FromBody] string titulo)
        {
            var usuarioID = servicioUsuarios.ObtenerUsuarioId();

            var existenTareas = await context.Tareas.AnyAsync(t => t.UsuariosCreacionId == usuarioID);

            var ordenMayor = 0;
            if (existenTareas)
            {
                ordenMayor = await context.Tareas.Where(t => t.UsuariosCreacionId == usuarioID)
                    .Select(t => t.Orden).MaxAsync();
            }

            var tarea = new Tarea
            {
                Titulo = titulo,
                UsuariosCreacionId = usuarioID,
                FechaCreacion = DateTime.UtcNow,
                Orden = ordenMayor + 1,
            };

            context.Add(tarea);
            await context.SaveChangesAsync();

            return tarea;
        }
        [HttpPost("ordenar")]
        public async Task<IActionResult> Ordenar([FromBody] int[] ids)
        {
            
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();

            var tareas = await context.Tareas.Where(t => t.UsuariosCreacionId == usuarioId).ToListAsync();
           
            var tareasId = tareas.Select(t => t.Id);
           
            var idsTareasNoPertenecenAlUsuario = ids.Except(tareasId).ToList();

            if (idsTareasNoPertenecenAlUsuario.Any())
            {
                return Forbid();
            }

            var tareasDiccionario = tareas.ToDictionary(x => x.Id);
            
            for (int i = 0; i < ids.Length; i++)
            {
                var id = ids[i];
                var tarea = tareasDiccionario[id];
                tarea.Orden = i + 1;
            }
            
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
