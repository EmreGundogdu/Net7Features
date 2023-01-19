using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("Basic", _options =>
    {
        _options.Window = TimeSpan.FromSeconds(12); //her 12 saniye de bir politika ge�erli olucak
        _options.PermitLimit = 4; //12 saniyede 4 tane istek hakk� var
        _options.QueueLimit = 2; //12 saniyede 4ten fazlaistek geldi�inde ka� tane iste�i kuyru�a als�n
        _options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst; //12 saniyede 4 istek hakk�n�n fazlas�nda gelenleri yani varsa kuyru�a alanlar� 12 saniye bittikten sonra kuyrukta olanlar� i�lemeye ba�lar

    }); 
});

var app = builder.Build();

app.UseRateLimiter(); // rate limit middleware

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
