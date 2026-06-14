using Microsoft.AspNetCore.Localization;
using PortfolioHub.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

// Localization. English is the primary/default language; Spanish is optional.
// NOTE: no ResourcesPath here on purpose. SharedResource.cs lives in /Resources but
// declares "namespace PortfolioHub", so the build's DependentUpon convention embeds the
// resx as "PortfolioHub.SharedResource". Setting ResourcesPath="Resources" would make the
// localizer look for "PortfolioHub.Resources.SharedResource" instead — a mismatch that
// returns the raw keys. Leaving it unset keeps base name = type full name = a match.
builder.Services.AddLocalization();

var supportedCultures = new[] { "en", "es" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture("en")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Resolve the request culture (from the cookie set by /culture/set) before rendering.
app.UseRequestLocalization();

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

// Language switch: store the chosen culture in a cookie and redirect back.
app.MapGet("/culture/set", (string culture, string redirectUri, HttpContext context) =>
{
    if (!string.IsNullOrWhiteSpace(culture))
    {
        context.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true });
    }

    return Results.LocalRedirect(string.IsNullOrEmpty(redirectUri) ? "/" : redirectUri);
});

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(PortfolioHub.Client._Imports).Assembly);

app.Run();
