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
            return snapshot.Documents.Select(MapMedico).ToList();
        }

        public async Task<Medico?> GetMedicoAsync(string id)
        {
            var snapshot = await _db.Collection("medicos").Document(id).GetSnapshotAsync();
            return snapshot.Exists ? MapMedico(snapshot) : null;
        }

        public async Task<string> AddMedicoAsync(Medico medico)
        {
            var document = await _db.Collection("medicos").AddAsync(new Dictionary<string, object>
            {
                { "Nombre", medico.Nombre },
                { "Dni", medico.Dni },
                { "Especialidad", medico.Especialidad },
                { "UsuarioId", medico.UsuarioId }
            });
            return document.Id;
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
            return snapshot.Documents.Select(MapPaciente).ToList();
        }

        public async Task<Paciente?> GetPacienteAsync(string id)
        {
            var snapshot = await _db.Collection("pacientes").Document(id).GetSnapshotAsync();
            return snapshot.Exists ? MapPaciente(snapshot) : null;
        }

        public async Task<string> AddPacienteAsync(Paciente paciente)
        {
            var document = await _db.Collection("pacientes").AddAsync(new Dictionary<string, object>
            {
                { "Nombre", paciente.Nombre },
                { "Dni", paciente.Dni },
                { "Telefono", paciente.Telefono },
                { "UsuarioId", paciente.UsuarioId }
            });
            return document.Id;
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
            return snapshot.Documents.Select(MapCita).ToList();
        }

        public async Task<Cita?> GetCitaAsync(string id)
        {
            var snapshot = await _db.Collection("citas").Document(id).GetSnapshotAsync();
            return snapshot.Exists ? MapCita(snapshot) : null;
        }

        public async Task<string> AddCitaAsync(Cita cita)
        {
            var document = await _db.Collection("citas").AddAsync(new Dictionary<string, object>
            {
                { "PacienteId", cita.PacienteId },
                { "MedicoId", cita.MedicoId },
                { "Fecha", cita.Fecha },
                { "Hora", cita.Hora },
                { "Estado", cita.Estado }
            });
            return document.Id;
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

        private static Paciente MapPaciente(DocumentSnapshot d)
        {
            return new Paciente
            {
                Id = d.Id,
                Nombre = GetString(d, "Nombre", "nombre"),
                Dni = GetString(d, "Dni", "dni"),
                Telefono = GetString(d, "Telefono", "telefono"),
                UsuarioId = GetString(d, "UsuarioId", "usuarioId")
            };
        }

        private static Medico MapMedico(DocumentSnapshot d)
        {
            return new Medico
            {
                Id = d.Id,
                Nombre = GetString(d, "Nombre", "nombre"),
                Dni = GetString(d, "Dni", "dni"),
                Especialidad = GetString(d, "Especialidad", "especialidad"),
                UsuarioId = GetString(d, "UsuarioId", "usuarioId")
            };
        }

        private static Cita MapCita(DocumentSnapshot d)
        {
            return new Cita
            {
                Id = d.Id,
                PacienteId = GetString(d, "PacienteId", "pacienteId"),
                MedicoId = GetString(d, "MedicoId", "medicoId"),
                Fecha = GetString(d, "Fecha", "fecha"),
                Hora = GetString(d, "Hora", "hora"),
                Estado = GetString(d, "Estado", "estado")
            };
        }

        private static string GetString(DocumentSnapshot document, params string[] fieldNames)
        {
            foreach (var fieldName in fieldNames)
            {
                if (document.TryGetValue<string>(fieldName, out var value))
                {
                    return value;
                }
            }

            return string.Empty;
        }
    }
}
