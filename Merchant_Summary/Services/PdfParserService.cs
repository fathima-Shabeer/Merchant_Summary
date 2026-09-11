using System.Globalization;
using System.Text.RegularExpressions;
using PdfChargesSummary.Models;
using UglyToad.PdfPig;

namespace PdfChargesSummary.Services;

public partial class PdfParserService
{
    // Matches the settlement report's summary block, e.g.:
    // "GRAND TOTAL VALUE OF POS TRANSACTIONS : 15899.66"
    // "GRAND TOTAL VALUE OF COMMISSION : -327.19"
    // Values can be negative and use "," thousands separators.
    [GeneratedRegex(@"GRAND\s*TOTAL\s*VALUE\s*OF\s*POS\s*TRANSACTIONS\s*:?\s*(-?[\d,]+\.?\d*)", RegexOptions.IgnoreCase)]
    private static partial Regex PosTotalRegex();

    [GeneratedRegex(@"GRAND\s*TOTAL\s*VALUE\s*OF\s*VAT\s*:?\s*(-?[\d,]+\.?\d*)", RegexOptions.IgnoreCase)]
    private static partial Regex VatRegex();

    [GeneratedRegex(@"GRAND\s*TOTAL\s*VALUE\s*OF\s*COMMISSION\s*:?\s*(-?[\d,]+\.?\d*)", RegexOptions.IgnoreCase)]
    private static partial Regex CommissionRegex();

    [GeneratedRegex(@"GRAND\s*TOTAL\s*VALUE\s*OF\s*FEE\s*TRANSACTIONS\s*:?\s*(-?[\d,]+\.?\d*)", RegexOptions.IgnoreCase)]
    private static partial Regex FeeRegex();

    // Report date, e.g. "04/09" under DTOF TRN — day/month only, no year on the report.
    [GeneratedRegex(@"(\d{1,2}[\/\-]\d{1,2}[\/\-]\d{2,4})")]
    private static partial Regex DateRegex();

    public DailyCharge ParsePdf(string filePath, string originalFileName)
    {
        var result = new DailyCharge { FileName = originalFileName };

        try
        {
            using var document = PdfDocument.Open(filePath);
            var text = string.Join("\n", document.GetPages().Select(p => p.Text));

            result.PosTotal = ExtractDecimal(PosTotalRegex(), text);
            result.Vat = ExtractDecimal(VatRegex(), text);
            result.Commission = ExtractDecimal(CommissionRegex(), text);
            result.Fee = ExtractDecimal(FeeRegex(), text);

            var dateMatch = DateRegex().Match(text);
            if (dateMatch.Success && DateTime.TryParse(dateMatch.Groups[1].Value, out var parsedDate))
            {
                result.DocumentDate = parsedDate;
            }

            if (result.PosTotal == 0 && result.Vat == 0 && result.Commission == 0 && result.Fee == 0)
            {
                result.ParseWarning = true;
                result.WarningMessage = "No matching totals found — check the PDF wording against the regex patterns.";
            }
        }
        catch (Exception ex)
        {
            result.ParseWarning = true;
            result.WarningMessage = $"Failed to read PDF: {ex.Message}";
        }

        return result;
    }

    private static decimal ExtractDecimal(Regex regex, string text)
    {
        var match = regex.Match(text);
        if (!match.Success) return 0m;

        var raw = match.Groups[1].Value.Replace(",", "");
        return decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out var value)
            ? value
            : 0m;
    }
}