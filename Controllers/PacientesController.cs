using Microsoft.AspNetCore.Mvc;
using MedAppWeb.Models;
using MedAppWeb.Services;

namespace MedAppWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly FirebaseService _firebase;
        public PacientesController(FirebaseService firebase) { _firebase = firebase; }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _firebase.GetPacientesAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var paciente = await _firebase.GetPacienteAsync(id);
            return paciente is null ? NotFound(new { message = "Paciente no encontrado" }) : Ok(paciente);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Paciente paciente)
        {
            var id = await _firebase.AddPacienteAsync(paciente);
            return Ok(new { message = "Paciente agregado", id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Paciente paciente)
        {
            await _firebase.UpdatePacienteAsync(id, paciente);
            return Ok(new { message = "Paciente actualizado" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _firebase.DeletePacienteAsync(id);
            return Ok(new { message = "Paciente eliminado" });
        }
    }
}
