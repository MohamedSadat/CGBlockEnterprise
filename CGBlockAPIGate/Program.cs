using CGBlockDA;
using CGBlockInfra.CGInterface;
using CGBlockInfra.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IApp,CAppModel>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(policy => policy.AddPolicy("open", opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddResponseCaching();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("open");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseResponseCaching();
GlobalConfig.cn = builder.Configuration.GetConnectionString("cn");

app.Run();
