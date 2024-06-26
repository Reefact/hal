#region Usings declarations

using Microsoft.AspNetCore.HttpOverrides;

using Reefact.Hateoas.Hal.AspNetCore;

#endregion

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddHalSupport(builder.Configuration.GetSection("hal"));
builder.Services.AddControllers(options => {
    options.SuppressAsyncSuffixInActionNames = false;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ForwardedHeadersOptions>(options => {
    options.ForwardedHeaders = ForwardedHeaders.All;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

WebApplication app = builder.Build();

app.UseForwardedHeaders();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();