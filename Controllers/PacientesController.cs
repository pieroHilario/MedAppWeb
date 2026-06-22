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
        public PacientesController() { _firebase = new FirebaseService(); }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _firebase.GetPacientesAsync());

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Paciente paciente)
        {
            await _firebase.AddPacienteAsync(paciente);
            return Ok(new { message = "Paciente agregado" });
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