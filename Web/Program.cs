using Web.Middleware;

namespace Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        // register custom middleware
        app.UseAuthMiddleware();

        app.MapGet("/", () => "Authorized!");

        app.Run();
    }
}
