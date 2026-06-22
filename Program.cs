using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using MedAppWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// Firebase
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile(
    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "firebase-credentials.json"))
});

builder.Services.AddSingleton<FirebaseService>();
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();

app.Run();