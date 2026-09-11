using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PdfChargesSummary.Models;
using PdfChargesSummary.Services;
using System.Text.Json;

namespace PdfChargesSummary.Pages;

public class UploadModel : PageModel
{
    private readonly PdfParserService _parser;
    private const int MaxFiles = 30;
    private const string SessionKey = "MonthlyCharges";

    public UploadModel(PdfParserService parser)
    {
        _parser = parser;
    }

    [BindProperty]
    public List<IFormFile> Files { get; set; } = new();

    public string? ErrorMessage { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Files.Count == 0)
        {
            ErrorMessage = "Please select at least one PDF file.";
            return Page();
        }

        if (Files.Count > MaxFiles)
        {
            ErrorMessage = $"Please select {MaxFiles} files or fewer at a time.";
            return Page();
        }

        var results = new List<DailyCharge>();
        var tempDir = Path.Combine(Path.GetTempPath(), "PdfChargesSummary", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        try
        {
            foreach (var file in Files)
            {
                if (!file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                    continue;

                var tempPath = Path.Combine(tempDir, file.FileName);
                await using (var stream = System.IO.File.Create(tempPath))
                {
                    await file.CopyToAsync(stream);
                }

                var parsed = _parser.ParsePdf(tempPath, file.FileName);
                results.Add(parsed);
            }
        }
        finally
        {
            // Clean up temp files regardless of outcome
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, recursive: true);
        }

        results = results.OrderBy(r => r.DocumentDate ?? DateTime.MaxValue).ToList();
        HttpContext.Session.SetString(SessionKey, JsonSerializer.Serialize(results));

        return RedirectToPage("/Summary");
    }
}