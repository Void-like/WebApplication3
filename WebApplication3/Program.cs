using WebApplication3.DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using WebApplication3.dto;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ItCompany1135Context>();
// Add services to the container.
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // óêàçûâàåò, áóäåò ëè âàëèäèðîâàòüñÿ èçäàòåëü ïðè âàëèäàöèè òîêåíà
            ValidateIssuer = true,
            // ñòðîêà, ïðåäñòàâëÿþùàÿ èçäàòåëÿ
            ValidIssuer = AuthOptions.ISSUER,
            // áóäåò ëè âàëèäèðîâàòüñÿ ïîòðåáèòåëü òîêåíà
            ValidateAudience = true,
            // óñòàíîâêà ïîòðåáèòåëÿ òîêåíà
            ValidAudience = AuthOptions.AUDIENCE,
            // áóäåò ëè âàëèäèðîâàòüñÿ âðåìÿ ñóùåñòâîâàíèÿ
            ValidateLifetime = true,
            // óñòàíîâêà êëþ÷à áåçîïàñíîñòè
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // âàëèäàöèÿ êëþ÷à áåçîïàñíîñòè
            ValidateIssuerSigningKey = true,
        };
    });

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();