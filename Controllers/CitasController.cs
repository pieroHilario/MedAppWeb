using Microsoft.AspNetCore.Mvc;
using MedAppWeb.Models;
using MedAppWeb.Services;

namespace MedAppWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitasController : ControllerBase
    {
        private readonly FirebaseService _firebase;
        public CitasController(FirebaseService firebase) { _firebase = firebase; }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _firebase.GetCitasAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var cita = await _firebase.GetCitaAsync(id);
            return cita is null ? NotFound(new { message = "Cita no encontrada" }) : Ok(cita);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cita cita)
        {
            var id = await _firebase.AddCitaAsync(cita);
            return Ok(new { message = "Cita agregada", id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Cita cita)
        {
            await _firebase.UpdateCitaAsync(id, cita);
            return Ok(new { message = "Cita actualizada" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _firebase.DeleteCitaAsync(id);
            return Ok(new { message = "Cita eliminada" });
        }
    }
}
