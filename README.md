# POS PDF Charges Summary

An ASP.NET Core web application that generates merchant charges summaries from POS transaction data and exports them as PDF reports, including monthly totals, yearly totals, VAT and extra fines.


## Overview

Finance and accounts teams often have to calculate merchant charges by hand and trace differences across months. This project automates that work:

- Calculates monthly and yearly charge totals for each merchant
- Applies VAT and extra fine calculations automatically
- Lets users search and trace transactions and merchant balances quickly
- Exports clear, printable PDF summaries

## Features

- **Merchant summary:** monthly and yearly totals per merchant
- **VAT calculation:** automatic VAT on charges
- **Extra fines:** fine amounts included in the summary and totals
- **Fast tracing:** search by merchant, date range or transaction to find where a figure comes from
- **PDF export:** downloadable charges summary report
- **Clean architecture:** separated layers for web, application logic and data access

## Tech Stack

| Area | Technology |
|------|------------|
| Language | C# 14 |
| Framework | .NET 10, ASP.NET Core |
| Data access | Entity Framework Core |
| Database | SQL Server |
| PDF generation | [QuestPDF / iText / your library] |
| Testing | xUnit |

## Project Structure

```
POS-PDF-Charges-Summary/
├── src/
│   ├── PosCharges.Web/            # ASP.NET Core UI and API controllers
│   ├── PosCharges.Application/    # Business logic (totals, VAT, fines)
│   ├── PosCharges.Domain/         # Entities and models
│   └── PosCharges.Infrastructure/ # EF Core, repositories, PDF service
├── tests/
│   └── PosCharges.Tests/          # Unit tests
└── README.md
```

Adjust the folder names to match your actual solution.

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)
- Visual Studio 2026 or VS Code

### Setup

```bash
# 1. Clone the repository
git clone https://github.com/yourname/POS-PDF-Charges-Summary.git
cd POS-PDF-Charges-Summary

# 2. Update the connection string in src/PosCharges.Web/appsettings.json

# 3. Apply database migrations
dotnet ef database update --project src/PosCharges.Infrastructure --startup-project src/PosCharges.Web

# 4. Run the application
dotnet run --project src/PosCharges.Web
```

Then open `https://localhost:5001` in your browser.

## Usage

1. Open the merchant summary page.
2. Select a merchant and a month or year.
3. Review the totals, VAT and extra fines.
4. Use search to trace any transaction behind a figure.
5. Click **Export PDF** to download the report.


## What I Learned

- Designing accurate financial calculations (totals, VAT, fines) with clear, testable rules
- Building fast search and tracing for large transaction tables with EF Core and SQL Server
- Generating professional PDF reports from ASP.NET Core
- Investigating and fixing accounts and invoice discrepancies

## Roadmap

- [ ] Role-based access for finance and support users
- [ ] Excel export
- [ ] Scheduled monthly report generation
- [ ] Docker support

