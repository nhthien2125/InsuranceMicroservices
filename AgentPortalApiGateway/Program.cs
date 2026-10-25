using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Eureka;

var builder = WebApplication.CreateBuilder(args);

// 1. Cố định cổng chạy là http://localhost:8099 (hoặc truyền thẳng vào app.Run bên dưới)
builder.WebHost.UseUrls("http://localhost:8099");

// 2. Nạp thêm ocelot.json (WebApplication.CreateBuilder đã tự nạp sẵn appsettings.json và BasePath)
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: false);

// 3. Đăng ký dịch vụ Ocelot + Eureka + CacheManager
builder.Services
    .AddOcelot(builder.Configuration)
    .AddEureka()
    .AddCacheManager(x => x.WithDictionaryHandle());

var app = builder.Build();

// 4. Sử dụng Middleware Ocelot
await app.UseOcelot();

app.Run();