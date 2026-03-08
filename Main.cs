using System.Diagnostics;
using SpaceRescueMission.Controllers;

namespace SpaceRescueMission
{
    // Main entry point for the web application
    class Program
    {
        static void Main(string[] args)
        {
            // Create web application
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            // Create game controller (creates Player, Spaceship, Levels inside)
            var game = new WebGameController();

            // Serve HTML/CSS/JS files from wwwroot folder
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // API endpoint - get current game state
            app.MapGet("/api/state", () => game.GetState());

            // API endpoint - process player action
            app.MapPost("/api/action", (ActionRequest req) =>
            {
                return game.ProcessAction(req.Action ?? "", req.Value ?? "");
            });

            // Set the URL and start
            string url = "http://localhost:5050";
            app.Urls.Add(url);

            // Open browser when ready
            app.Lifetime.ApplicationStarted.Register(() =>
            {
                Console.WriteLine("Space Rescue Mission is running at " + url);
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

// Simple class for receiving actions from the browser
public class ActionRequest
{
    public string? Action { get; set; }
    public string? Value { get; set; }
}
