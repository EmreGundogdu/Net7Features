using Microsoft.AspNetCore.OutputCaching;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(x => //Bu projede kullan�lan t�m output caching yap�lar�nda buradaki configurasyonlar ge�erli olucak addbasepolicy dedi�imiz i�in
    {
        x.Expire(TimeSpan.FromSeconds(5)); //t�m output cache oldu�u yerlerde policy yaz�lmaks�z�n default olarak eklenen �rnek: [outputcaching] gibi yerlerde kullan�lanacak olan base confiurasyonlard�r

    });
    options.AddPolicy("CustomPolicy", x => //bizim kendimiz istenilen yerle g�re �zel configure edebilece�imiz configurationlar ekleyebiliriz
    {
        x.Expire(TimeSpan.FromSeconds(10));
    });
});

var app = builder.Build();

app.UseOutputCache();




//Geli�tirdi�imiz web mimarisinde ��kt� olarak �retilen verinin sunucu taraf�nda verinin cachelenmesini sa�lar


#region Minimal API'den output caching

app.MapGet("/", [OutputCache]() => //bu attribute'de kullan�labilir | Daha fazla configure edilebilir
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
