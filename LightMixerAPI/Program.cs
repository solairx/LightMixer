using LightMixer;
using LightMixerAngular;
using LightMixerAPI.Controllers;
using Unity;

LightMixer.LightMixerBootStrap bs = new LightMixer.LightMixerBootStrap(new DummyDispatcher());

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(LightMixerBootStrap.UnityContainer.Resolve<SceneService>());


var policyName = "defaultCorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(policyName, builder =>
    {
        builder.WithOrigins() // the Angular app url
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<SongHub>("/hub");

app.Run();
