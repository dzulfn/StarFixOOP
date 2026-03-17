using System.Diagnostics;
using StarFix.Controllers;

namespace StarFix
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            var game = new WebGameController();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.MapGet("/api/state", () => game.GetState());
            app.MapPost("/api/action", (ActionRequest req) =>
            {
                return game.ProcessAction(req.Action ?? "", req.Value ?? "");
            });

            string url = "http://localhost:5050";
            app.Urls.Add(url);

            app.Lifetime.ApplicationStarted.Register(() =>
            {
                Console.WriteLine("StarFix is running at " + url);
                Console.WriteLine("Press Ctrl+C to stop.");
                try
                {
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                catch
                {
                    Console.WriteLine("Open your browser to " + url);
                }
            });

            app.Run();
        }
    }
}

public class ActionRequest
{
    public string? Action { get; set; }
    public string? Value { get; set; }
}
