using Microsoft.AspNetCore.OutputCaching;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(x => //Bu projede kullanýlan tüm output caching yapýlarýnda buradaki configurasyonlar geçerli olucak addbasepolicy dediðimiz için
    {
        x.Expire(TimeSpan.FromSeconds(5)); //tüm output cache olduðu yerlerde policy yazýlmaksýzýn default olarak eklenen örnek: [outputcaching] gibi yerlerde kullanýlanacak olan base confiurasyonlardýr

    });
    options.AddPolicy("CustomPolicy", x => //bizim kendimiz istenilen yerle göre özel configure edebileceðimiz configurationlar ekleyebiliriz
    {
        x.Expire(TimeSpan.FromSeconds(10));
    });
});

var app = builder.Build();

app.UseOutputCache();




//Geliþtirdiðimiz web mimarisinde çýktý olarak üretilen verinin sunucu tarafýnda verinin cachelenmesini saðlar


#region Minimal API'den output caching

app.MapGet("/", [OutputCache]() => //bu attribute'de kullanýlabilir | Daha fazla configure edilebilir
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
