using Microsoft.AspNetCore.OutputCaching;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOutputCache();

var app = builder.Build();

app.UseOutputCache();




//Geli�tirdi�imiz web mimarisinde ��kt� olarak �retilen verinin sunucu taraf�nda verinin cachelenmesini sa�lar


#region Minimal API'den output caching

app.MapGet("/", [OutputCache]() => //bu attribute'de kullan�labilir
{
    return Results.Ok(DateTime.UtcNow);
}).CacheOutput(); //bu endpointe gelen istek neticesinde gelen veriyi 1 dakika cachler ve 1 dakika boyunca bu veriyi d�nd�r�r. 1 dakikadan sonra cacheden at�l�r | Default 1 dk'd�r

#endregion


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
