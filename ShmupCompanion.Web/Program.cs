using Bit.BlazorUI;
using ShmupCompanion.Web.Components;
using ShmupCompanion.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddServerSideBlazor()
    .AddHubOptions(options =>
    {
        // Increase limit to 5MB
        options.MaximumReceiveMessageSize = 5 * 1024 * 1024;
    });

builder.Services.AddHttpClient("ShmupApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7135/");
});

builder.Services.AddScoped<GameApiService>();
builder.Services.AddScoped<RunApiService>();
builder.Services.AddScoped<NoteApiService>();
builder.Services.AddBitBlazorUIServices();
builder.Services.AddBitBlazorUIExtrasServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
