using RoKa.Components;
using RoKa.Services;
using RoKa.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSignalR();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapBlazorHub();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapHub<EmailNotificationHub>("/emailNotificationHub");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();