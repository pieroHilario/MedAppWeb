using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using MedAppWeb.Models;

namespace MedAppWeb.Services
{
    public class FirebaseService
    {
        private readonly FirestoreDb _db;

        public FirebaseService()
        {
            var credentialsJson = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS_JSON");
            if (!string.IsNullOrEmpty(credentialsJson))
            {
                var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(credentialsJson));
                var credential = GoogleCredential.FromStream(stream).CreateScoped(
                    "https://www.googleapis.com/auth/cloud-platform",
                    "https://www.googleapis.com/auth/datastore");
                var client = new FirestoreClientBuilder { Credential = credential }.Build();
                _db = FirestoreDb.Create("medappweb-b6899", client);
            }
            else
            {
                Environment.SetEnvironmentVariable(
                    "GOOGLE_APPLICATION_CREDENTIALS",
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "firebase-credentials.json"));
                _db = FirestoreDb.Create("medappweb-b6899");
            }
        }

        // ========== MEDICOS ==========
        public async Task<List<Medico>> GetMedicosAsync()
        {
            var snapshot = await _db.Collection("medicos").GetSnapshotAsync();
            return snapshot.Documents.Select(d => new Medico
            {
                Id = d.Id,
                Nombre = d.GetValue<string>("Nombre"),
                Dni = d.GetValue<string>("Dni"),
                Especialidad = d.GetValue<string>("Especialidad"),
                UsuarioId = d.GetValue<string>("UsuarioId")
            }).ToList();
        }

        public async Task AddMedicoAsync(Medico medico)
        {
            await _db.Collection("medicos").AddAsync(new Dictionary<string, object>
            {
                { "Nombre", medico.Nombre },
                { "Dni", medico.Dni },
                { "Especialidad", medico.Especialidad },
                { "UsuarioId", medico.UsuarioId }
            });
        }

        public async Task UpdateMedicoAsync(string id, Medico medico)
        {
            await _db.Collection("medicos").Document(id).SetAsync(new Dictionary<string, object>
            {
                { "Nombre", medico.Nombre },
                { "Dni", medico.Dni },
                { "Especialidad", medico.Especialidad },
                { "UsuarioId", medico.UsuarioId }
            });
        }

        public async Task DeleteMedicoAsync(string id)
        {
            await _db.Collection("medicos").Document(id).DeleteAsync();
        }

        // ========== PACIENTES ==========
        public async Task<List<Paciente>> GetPacientesAsync()
        {
            var snapshot = await _db.Collection("pacientes").GetSnapshotAsync();
            return snapshot.Documents.Select(d => new Paciente
            {
                Id = d.Id,
                Nombre = d.GetValue<string>("Nombre"),
                Dni = d.GetValue<string>("Dni"),
                Telefono = d.GetValue<string>("Telefono"),
                UsuarioId = d.GetValue<string>("UsuarioId")
            }).ToList();
        }

        public async Task AddPacienteAsync(Paciente paciente)
        {
            await _db.Collection("pacientes").AddAsync(new Dictionary<string, object>
            {
                { "Nombre", paciente.Nombre },
                { "Dni", paciente.Dni },
                { "Telefono", paciente.Telefono },
                { "UsuarioId", paciente.UsuarioId }
            });
        }

        public async Task UpdatePacienteAsync(string id, Paciente paciente)
        {
            await _db.Collection("pacientes").Document(id).SetAsync(new Dictionary<string, object>
            {
                { "Nombre", paciente.Nombre },
                { "Dni", paciente.Dni },
                { "Telefono", paciente.Telefono },
                { "UsuarioId", paciente.UsuarioId }
            });
        }

        public async Task DeletePacienteAsync(string id)
        {
            await _db.Collection("pacientes").Document(id).DeleteAsync();
        }

        // ========== CITAS ==========
        public async Task<List<Cita>> GetCitasAsync()
        {
            var snapshot = await _db.Collection("citas").GetSnapshotAsync();
            return snapshot.Documents.Select(d => new Cita
            {
                Id = d.Id,
                PacienteId = d.GetValue<string>("PacienteId"),
                MedicoId = d.GetValue<string>("MedicoId"),
                Fecha = d.GetValue<string>("Fecha"),
                Hora = d.GetValue<string>("Hora"),
                Estado = d.GetValue<string>("Estado")
            }).ToList();
        }

        public async Task AddCitaAsync(Cita cita)
        {
            await _db.Collection("citas").AddAsync(new Dictionary<string, object>
            {
                { "PacienteId", cita.PacienteId },
                { "MedicoId", cita.MedicoId },
                { "Fecha", cita.Fecha },
                { "Hora", cita.Hora },
                { "Estado", cita.Estado }
            });
        }

        public async Task UpdateCitaAsync(string id, Cita cita)
        {
            await _db.Collection("citas").Document(id).SetAsync(new Dictionary<string, object>
            {
                { "PacienteId", cita.PacienteId },
                { "MedicoId", cita.MedicoId },
                { "Fecha", cita.Fecha },
                { "Hora", cita.Hora },
                { "Estado", cita.Estado }
            });
        }

        public async Task DeleteCitaAsync(string id)
        {
            await _db.Collection("citas").Document(id).DeleteAsync();
        }
    }
}