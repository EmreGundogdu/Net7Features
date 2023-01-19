using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Temel Rate Limit
//builder.Services.AddRateLimiter(options =>
//{
//    options.AddFixedWindowLimiter("Basic", _options =>
//    {
//        _options.Window = TimeSpan.FromSeconds(12); //her 12 saniye de bir politika ge�erli olucak
//        _options.PermitLimit = 4; //12 saniyede 4 tane istek hakk� var
//        _options.QueueLimit = 2; //12 saniyede 4ten fazlaistek geldi�inde ka� tane iste�i kuyru�a als�n
//        _options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst; //12 saniyede 4 istek hakk�n�n fazlas�nda gelenleri yani varsa kuyru�a alanlar� 12 saniye bittikten sonra kuyrukta olanlar� i�lemeye ba�lar

//    });
//});

#endregion
#region Rate Limiter Algoritmalar� Nelerdir
#region Fixed Window
//Sabit bir zaman aral��� kullan�larak istekleri sn�n�rlar

//builder.Services.AddRateLimiter(options =>
//{
//    options.AddFixedWindowLimiter("Fixed", _options =>
//    {
//        _options.Window = TimeSpan.FromSeconds(12); //sabit zaman aral���
//        _options.PermitLimit = 4; //sabit istek say�s�
//        _options.QueueLimit = 2; //12 saniyede 4ten fazlaistek geldi�inde ka� tane iste�i kuyru�a als�n
//        _options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst; //12 saniyede 4 istek hakk�n�n fazlas�nda gelenleri yani varsa kuyru�a alanlar� 12 saniye bittikten sonra kuyrukta olanlar� i�lemeye ba�lar

//    });
//});

#endregion

#region Sliding Window
//Fixed window algroitmas�na benzerlik g�stermektedir.(Her sabir s�rede bir zaman aral���nda istekleri s�n�rland�rmamaktad�r.Lakin s�renin yar�s�ndan sonra di�er periyodun request kotas�n� harcayarak �ekilde istekleri kar��lar) 12 saniyelik bir zaman diliminde veya bellir bir zaman diliminde ve bu zaman�n yar�s�ndan sonra sonraki periyottan requestleri i�lemeye ba�lar
//builder.Services.AddRateLimiter(opt =>
//{
//    opt.AddSlidingWindowLimiter("Sliding", options =>
//    {
//        options.Window = TimeSpan.FromSeconds(12);
//        options.PermitLimit = 4;
//        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
//        options.QueueLimit = 2;
//        options.SegmentsPerWindow = 2; //12 saniyelik s�reden sonra gelicek olan requestlerden 2 tane kar��lar ilk 12 saniyelik s�re i�erisinde
//    });
//});
#endregion

#region Token Bucket 
//Her periyotta i�lenecek request say�s� kadar token �retilmektedir. E�er ki bu tokenlar kullan�ld�ysa di�er periyottan bor� al�nabilir. Lakin her periyotta token �retim miktar� kadar token �retilecek ve bu �ekilde rate limit uygulanacakt�r. Her periyodun maximum token limit verilen sabit say� zaman dili kadar olacakt�r.
//builder.Services.AddRateLimiter(opt =>
//{
//    opt.AddTokenBucketLimiter("Token", options =>
//    {
//        options.TokenLimit = 4; //token limit 4
//        options.TokensPerPeriod = 4; // bu alan 4'ten by�k bir say� girsek bile token limit parametresine g�re i�leyece�i i�in token limitten az veya e�it olmal� o y�zden 4 yaz�ld�
//        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
//        options.QueueLimit = 2;
//        options.ReplenishmentPeriod = TimeSpan.FromSeconds(12); //periyotlar�n zaman dilimleri 
//    });
//});
#endregion

#region Concurrency
//Asenkron requestleri s�n�rlamak i�in kullan�lan bir algoritmiktir. her istek conccurrency s�n�r�n� bir azaltmakta ve bittikleri taktirde bu s�n�r� bir artt�rmaktad�r. Di�er algoritmalara nazaran sadece asenkron requestleri s�n�rland�r�rlar.

//builder.Services.AddRateLimiter(opt =>
//{
//    opt.AddConcurrencyLimiter("Concurrency", options =>
//    {
//        options.PermitLimit = 4;
//        options.QueueLimit = 2;
//        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
//    });
//});
#endregion

#endregion

#region A�a��da �zelle�tirdi�im rate limiti ekleme
builder.Services.AddRateLimiter(opt =>
{
    opt.AddPolicy<string, CustomRateLimitPolicy>("customPolicy");
});
#endregion

var app = builder.Build();

app.UseRateLimiter(); // rate limit middleware


#region OnRejected Property
//OnRejected Property: Rate limit uygulanan operasyonlarda s�n�rdan dolay� bo�a ��kan requestlerin s�z konsuu oldu�u durumlarda loglama vs gibi i�lemleri yapabilmek i�in kulland���m�z event mant���nda bir propertydir.
//builder.Services.AddRateLimiter(opt =>
//{
//    opt.AddFixedWindowLimiter("Fixed", options =>
//    {
//        options.Window = TimeSpan.FromSeconds(10);
//        options.PermitLimit = 4;
//        options.QueueLimit = 4;
//        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
//    });
//    opt.OnRejected = (context, cancellationToken) =>
//    {
//        //12 saniyelik bir rate limitte 4 iste�i i�ledi 2'de kurukta vard� 6 oldu ve hala 12 saniye i�indeyse 7. istek buraya d��er context ve cancellation token �zerinde  i�lem yapabiliriz
//        //loglama 
//        return new();
//    };
//});
#endregion

#region Minimal API Rate limit
app.MapGet("/", () =>
{

}).RequireRateLimiting("Politika ismi");
#endregion

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

#region �zelle�tirilmi� rate limit
class CustomRateLimitPolicy : IRateLimiterPolicy<string> // bu interfaceden implement olmal�
{
    public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected => (context, cancellationToken) =>
    {
        return new();
    };

    public RateLimitPartition<string> GetPartition(HttpContext httpContext)
    {
        return RateLimitPartition.GetFixedWindowLimiter("", _ => new()
        {
            PermitLimit = 4,
            Window = TimeSpan.FromSeconds(5),
            QueueLimit = 2,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
        });
    }
}
#endregion

