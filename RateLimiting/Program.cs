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
        _options.Window = TimeSpan.FromSeconds(12); //her 12 saniye de bir politika geçerli olucak
        _options.PermitLimit = 4; //12 saniyede 4 tane istek hakký var
        _options.QueueLimit = 2; //12 saniyede 4ten fazlaistek geldiðinde kaç tane isteði kuyruða alsýn
        _options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst; //12 saniyede 4 istek hakkýnýn fazlasýnda gelenleri yani varsa kuyruða alanlarý 12 saniye bittikten sonra kuyrukta olanlarý iþlemeye baþlar

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
