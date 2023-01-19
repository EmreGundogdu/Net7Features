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
//        _options.Window = TimeSpan.FromSeconds(12); //her 12 saniye de bir politika geçerli olucak
//        _options.PermitLimit = 4; //12 saniyede 4 tane istek hakký var
//        _options.QueueLimit = 2; //12 saniyede 4ten fazlaistek geldiðinde kaç tane isteði kuyruða alsýn
//        _options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst; //12 saniyede 4 istek hakkýnýn fazlasýnda gelenleri yani varsa kuyruða alanlarý 12 saniye bittikten sonra kuyrukta olanlarý iþlemeye baþlar

//    });
//});

#endregion
#region Rate Limiter Algoritmalarý Nelerdir
#region Fixed Window
//Sabit bir zaman aralýðý kullanýlarak istekleri snýnýrlar

//builder.Services.AddRateLimiter(options =>
//{
//    options.AddFixedWindowLimiter("Fixed", _options =>
//    {
//        _options.Window = TimeSpan.FromSeconds(12); //sabit zaman aralýðý
//        _options.PermitLimit = 4; //sabit istek sayýsý
//        _options.QueueLimit = 2; //12 saniyede 4ten fazlaistek geldiðinde kaç tane isteði kuyruða alsýn
//        _options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst; //12 saniyede 4 istek hakkýnýn fazlasýnda gelenleri yani varsa kuyruða alanlarý 12 saniye bittikten sonra kuyrukta olanlarý iþlemeye baþlar

//    });
//});

#endregion

#region Sliding Window
//Fixed window algroitmasýna benzerlik göstermektedir.(Her sabir sürede bir zaman aralýðýnda istekleri sýnýrlandýrmamaktadýr.Lakin sürenin yarýsýndan sonra diðer periyodun request kotasýný harcayarak þekilde istekleri karþýlar) 12 saniyelik bir zaman diliminde veya bellir bir zaman diliminde ve bu zamanýn yarýsýndan sonra sonraki periyottan requestleri iþlemeye baþlar
//builder.Services.AddRateLimiter(opt =>
//{
//    opt.AddSlidingWindowLimiter("Sliding", options =>
//    {
//        options.Window = TimeSpan.FromSeconds(12);
//        options.PermitLimit = 4;
//        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
//        options.QueueLimit = 2;
//        options.SegmentsPerWindow = 2; //12 saniyelik süreden sonra gelicek olan requestlerden 2 tane karþýlar ilk 12 saniyelik süre içerisinde
//    });
//});
#endregion

#region Token Bucket 
//Her periyotta iþlenecek request sayýsý kadar token üretilmektedir. Eðer ki bu tokenlar kullanýldýysa diðer periyottan borç alýnabilir. Lakin her periyotta token üretim miktarý kadar token üretilecek ve bu þekilde rate limit uygulanacaktýr. Her periyodun maximum token limit verilen sabit sayý zaman dili kadar olacaktýr.
//builder.Services.AddRateLimiter(opt =>
//{
//    opt.AddTokenBucketLimiter("Token", options =>
//    {
//        options.TokenLimit = 4; //token limit 4
//        options.TokensPerPeriod = 4; // bu alan 4'ten byük bir sayý girsek bile token limit parametresine göre iþleyeceði için token limitten az veya eþit olmalý o yüzden 4 yazýldý
//        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
//        options.QueueLimit = 2;
//        options.ReplenishmentPeriod = TimeSpan.FromSeconds(12); //periyotlarýn zaman dilimleri 
//    });
//});
#endregion

#region Concurrency
//Asenkron requestleri sýnýrlamak için kullanýlan bir algoritmiktir. her istek conccurrency sýnýrýný bir azaltmakta ve bittikleri taktirde bu sýnýrý bir arttýrmaktadýr. Diðer algoritmalara nazaran sadece asenkron requestleri sýnýrlandýrýrlar.

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

#region Aþaðýda özelleþtirdiðim rate limiti ekleme
builder.Services.AddRateLimiter(opt =>
{
    opt.AddPolicy<string, CustomRateLimitPolicy>("customPolicy");
});
#endregion

var app = builder.Build();

app.UseRateLimiter(); // rate limit middleware


#region OnRejected Property
//OnRejected Property: Rate limit uygulanan operasyonlarda sýnýrdan dolayý boþa çýkan requestlerin söz konsuu olduðu durumlarda loglama vs gibi iþlemleri yapabilmek için kullandýðýmýz event mantýðýnda bir propertydir.
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
//        //12 saniyelik bir rate limitte 4 isteði iþledi 2'de kurukta vardý 6 oldu ve hala 12 saniye içindeyse 7. istek buraya düþer context ve cancellation token üzerinde  iþlem yapabiliriz
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

#region Özelleþtirilmiþ rate limit
class CustomRateLimitPolicy : IRateLimiterPolicy<string> // bu interfaceden implement olmalý
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

