using Microsoft.AspNetCore.Mvc.RazorPages;
using PdfChargesSummary.Models;
using System.Text.Json;

namespace PdfChargesSummary.Pages;

public class SummaryModel : PageModel
{
    private const string SessionKey = "MonthlyCharges";

    public List<DailyCharge> Charges { get; set; } = new();

    public decimal PosTotalSum => Charges.Sum(c => c.PosTotal);
    public decimal VatSum => Charges.Sum(c => c.Vat);
    public decimal CommissionSum => Charges.Sum(c => c.Commission);
    public decimal FeeSum => Charges.Sum(c => c.Fee);
    public decimal NetTotalSum => Charges.Sum(c => c.NetTotal);

    public void OnGet()
    {
        var json = HttpContext.Session.GetString(SessionKey);
        if (!string.IsNullOrEmpty(json))
        {
            Charges = JsonSerializer.Deserialize<List<DailyCharge>>(json) ?? new();
        }
    }
}