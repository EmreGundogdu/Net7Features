using Microsoft.AspNetCore.OutputCaching;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOutputCache();

var app = builder.Build();

app.UseOutputCache();




//Geliþtirdiðimiz web mimarisinde çýktý olarak üretilen verinin sunucu tarafýnda verinin cachelenmesini saðlar


#region Minimal API'den output caching

app.MapGet("/", [OutputCache]() => //bu attribute'de kullanýlabilir
{
    return Results.Ok(DateTime.UtcNow);
}).CacheOutput(); //bu endpointe gelen istek neticesinde gelen veriyi 1 dakika cachler ve 1 dakika boyunca bu veriyi döndürür. 1 dakikadan sonra cacheden atýlýr | Default 1 dk'dýr

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
