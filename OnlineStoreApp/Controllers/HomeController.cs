using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using OnlineStoreApp.Models;

namespace OnlineStoreApp.Controllers;

public class HomeController : Controller
{
    private readonly MyAppContext _context;

    public HomeController(MyAppContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error()
    {
        await LogErrorHitAsync();
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private async Task LogErrorHitAsync()
    {
        try
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var exception = feature?.Error;
            var path = feature?.Path ?? HttpContext.Request.Path.Value ?? "unknown";
            var exceptionType = exception?.GetType().FullName ?? "UnknownException";
            var message = exception?.Message ?? "Error page reached without exception details.";
            var now = DateTime.UtcNow;

            var signatureInput = $"{exceptionType}|{path}|{message}";
            var signature = ComputeSha256(signatureInput);

            var existing = await _context.ErrorLogs
                .FirstOrDefaultAsync(x => x.Signature == signature);

            if (existing is null)
            {
                _context.ErrorLogs.Add(new ErrorLog
                {
                    Signature = signature,
                    ExceptionType = exceptionType,
                    Message = message,
                    Path = path,
                    HitCount = 1,
                    FirstSeenUtc = now,
                    LastSeenUtc = now
                });
            }
            else
            {
                existing.HitCount += 1;
                existing.LastSeenUtc = now;
            }

            await _context.SaveChangesAsync();
        }
        catch
        {
            // Do not throw from error handling path.
        }
    }

    private static string ComputeSha256(string value)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(bytes);
    }
}
