namespace PdfChargesSummary.Models;

public class DailyCharge
{
    public string FileName { get; set; } = string.Empty;
    public DateTime? DocumentDate { get; set; }

    // Grand Total Value of POS Transactions
    public decimal PosTotal { get; set; }
    // Grand Total Value of VAT
    public decimal Vat { get; set; }
    // Grand Total Value of Commission
    public decimal Commission { get; set; }
    // Grand Total Value of Fee Transactions
    public decimal Fee { get; set; }

    // Net settlement value for this single document
    public decimal NetTotal => PosTotal + Vat + Commission + Fee;

    public bool ParseWarning { get; set; }
    public string? WarningMessage { get; set; }
}