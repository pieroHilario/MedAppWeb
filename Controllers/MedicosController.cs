using Microsoft.AspNetCore.Mvc;
using MedAppWeb.Models;
using MedAppWeb.Services;

namespace MedAppWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicosController : ControllerBase
    {
        private readonly FirebaseService _firebase;
        public MedicosController(FirebaseService firebase) { _firebase = firebase; }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _firebase.GetMedicosAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var medico = await _firebase.GetMedicoAsync(id);
            return medico is null ? NotFound(new { message = "Médico no encontrado" }) : Ok(medico);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Medico medico)
        {
            var id = await _firebase.AddMedicoAsync(medico);
            return Ok(new { message = "Médico agregado", id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Medico medico)
        {
            await _firebase.UpdateMedicoAsync(id, medico);
            return Ok(new { message = "Médico actualizado" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _firebase.DeleteMedicoAsync(id);
            return Ok(new { message = "Médico eliminado" });
        }
    }
}
