using TheFinalProject;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.Register(config);

var app = builder.Build();
app.UseSession();
app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );


app.MapControllerRoute(
    "default",
    "{controller=home}/{action=index}/{id?}"
    );
app.UseStaticFiles();

app.Run();
