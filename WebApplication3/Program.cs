using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using WebApplication3.Controllers;
using WebApplication3.DB;
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

//Дана база данных: it_company_1135(192.168.200.13, mariadb)

//К ней нужно разработать ряд контроллеров (все контроллеры делать не надо!! в списке отмечено, кто что делает).

//Общие требования для api:
//Настроить выдачу и проверку JWT-токенов (пример тут: https://github.com/stenly87/JwtSample_2025)
//Каждый делает контроллер для авторизации!

//Общие требования для контроллеров:
//!UserSid не передаётся в URL или теле запроса, если это касается действий от имени текущего пользователя.
//! Если операция требует указать другого пользователя (например, админ вручает бейдж), тогда userSid передаётся в теле запроса.
//! Все методы GET /.../my/... и GET /.../me работают с данными текущего пользователя (из JWT).
//! Все изменения (CreatedBy, ModifiedBy, DeletedBy) автоматически заполняются на основе UserSid из токена.
//! Обработка методов по CQRS (используем медиатор, https://github.com/stenly87/MyMediator), принимаем и возвращаем DTO-объекты, обработка команд в хэндлерах
//!Заметка: Методы PATCH предполагают работу не с полным DTO, а с DTO, в котором есть только изменяемый параметр (например: { "email": "newemail@example.com" })
//!Заметка: Некоторые методы требуют проверки прав - в бд есть роли, проверяйте, что у текущего пользователя есть требуемая роль (добавьте роль, если не хватает)

//{
//    IncidentsController

//    POST / incidents / report---- > Создать инцидент от текущего пользователя.

//    PATCH / incidents /{ id}/ assign---- > Назначить исполнителя(AssignedToSid в теле; только модератор).

//    PATCH / incidents /{ id}/ resolve---- > Закрыть инцидент(только исполнитель или админ).
//    GET / incidents / my / open---- > Получить открытые инциденты текущего пользователя.

//    GET / incidents / priority /{ priority}/ list---- > Получить инциденты по приоритету(для поддержки).
//}