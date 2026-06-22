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
        public MedicosController() { _firebase = new FirebaseService(); }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _firebase.GetMedicosAsync());

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Medico medico)
        {
            await _firebase.AddMedicoAsync(medico);
            return Ok(new { message = "Médico agregado" });
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