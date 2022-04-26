using Microsoft.AspNetCore.HttpOverrides;
using UrlShortener;
using UrlShortener.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<UrlShortenerService>();

builder.Services.AddTransient<IShortLinkRepository, InMemoryShortLinkRepository>();
builder.Services.AddTransient<IShortLinkUsageHistoryRepository, InMemoryShortLinkRepository>();
builder.Services.AddTransient<IIdentifierGenerator, IdentifierGenerator>();
// TODO Interface?
builder.Services.AddTransient<UrlShortenerService>();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapControllers();
app.MapFallbackToPage("/_Host");

app.Run();