using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using frontend.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; });
builder.Services.AddSingleton<RecipientService>();

// Add HttpClient with base address
builder.Services.AddHttpClient("BookRecommenderAPI", client =>
{
    client.BaseAddress = new Uri("http://bookrecomenderapi-server");
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
