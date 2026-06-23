using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MedAppWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// Firebase
var credentialsJson = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS_JSON");
if (credentialsJson != null)
{
    var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(credentialsJson));
    FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.FromStream(stream)
    });
}
else
{
    FirebaseApp.Create(new AppOptions()
    {
        Credential = GoogleCredential.FromFile(
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "firebase-credentials.json"))
    });
}

builder.Services.AddSingleton<FirebaseService>();
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MobileApp", policy =>
    {
        var origins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
        if (origins is { Length: > 0 })
        {
            policy.WithOrigins(origins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
        else
        {
            policy.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        }
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("MobileApp");
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.Run();
