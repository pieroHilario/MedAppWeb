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
        public CitasController() { _firebase = new FirebaseService(); }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _firebase.GetCitasAsync());

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cita cita)
        {
            await _firebase.AddCitaAsync(cita);
            return Ok(new { message = "Cita agregada" });
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