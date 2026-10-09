# DocuTrack

DocuTrack is a Windows desktop application for keeping track of seafarers' (sailors') certificates and the paperwork around them. It stores each sailor's certificates and scanned documents, warns you about certificates that are about to expire, and produces print-ready PDFs of the documents and request forms (*zahtevi*) you need to acquire or renew a certificate.

Built with C# / .NET 9, [Avalonia UI](https://avaloniaui.net/) and SQLite. The interface is in Serbian.

## Features

- **Login** – the app opens on a login screen; the sidebar is only visible after signing in.
- **Početna (Home) – certificate expirations** – a searchable, paginated list of certificates that expire soon. Only the latest certificate of each type per sailor is shown, and the default window is 300 days ahead.
- **Pomorci (Sailors)** – add, edit and delete sailors (name, surname, unique GID, refresh flag), with search and pagination.
- **Sailor profile** – a sailor's certificates with version history, and the files attached to each one. Certificates record the date acquired, expiration date and place.
- **Dokumenta (Documents)** – manage certificate types and the request form templates attached to each type. Templates are tagged by request type: *Sticanje* (acquisition), *Obnova* (renewal) or *Refresh*.
- **PDF generation** – select any number of files (PDF, Word `.docx` or images) and merge them into a single PDF for printing, or combine all request forms of one type into one PDF.
- **UI scaling** – zoom buttons in the sidebar scale the interface from 50% to 115%.
- **Izađi** – exits the application.

## Tech stack

| Area          | Technology                                            |
|---------------|-------------------------------------------------------|
| Language      | C# (nullable enabled), .NET 9                         |
| UI            | Avalonia 11.2.6 (Fluent theme, Inter font)            |
| Pattern       | MVVM with CommunityToolkit.Mvvm                       |
| Database      | SQLite via Microsoft.Data.Sqlite                      |
| PDF / Word    | PDFsharp 6.1.1, DocumentFormat.OpenXml 3.3.0          |
| Reactive      | System.Reactive 6.0.1                                 |

### NuGet packages

| Package                    | Version |
|----------------------------|---------|
| Avalonia                   | 11.2.6  |
| Avalonia.Desktop           | 11.2.6  |
| Avalonia.Diagnostics       | 11.2.6  |
| Avalonia.Fonts.Inter       | 11.2.6  |
| Avalonia.Themes.Fluent     | 11.2.6  |
| CommunityToolkit.Mvvm      | 8.4.0   |
| DocumentFormat.OpenXml     | 3.3.0   |
| Microsoft.Data.Sqlite      | 9.0.4   |
| Microsoft.NET.ILLink.Tasks | 9.0.4   |
| PDFsharp                   | 6.1.1   |
| System.Reactive            | 6.0.1   |

`Avalonia.Diagnostics` is only included in Debug builds.

## Getting started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Windows is the primary target. The code is cross-platform Avalonia, but publishing is set up for Windows.

### Run from source

```bash
git clone <repository-url>
cd DocuTrack
dotnet restore
dotnet run --project DocuTrack
```

In Debug builds the database is opened at `Data/identifier.sqlite` relative to the working directory, so run the app from the `DocuTrack/` project folder (`dotnet run` does this when you use `--project`).

### Build

```bash
dotnet build docutrack.sln -c Release
```

### Publish a single-file Windows executable

The project is configured for a self-contained, compressed single-file publish. Uncomment the `RuntimeIdentifiers` line in `DocuTrack.csproj`, then run:

```bash
dotnet publish DocuTrack -c Release -r win-x64
```

The included `FolderProfile.pubxml` publishes to `D:\DocuTrack`. Change `PublishDir` there if you want another location.

In Release builds the database path is resolved from `AppContext.BaseDirectory`, so the `Data/` folder must sit next to the executable (see below).

## Data and storage

All data lives in the `Data/` folder:

```
Data/
├── identifier.sqlite     # SQLite database
├── Certificates/         # files attached to sailors' certificates
├── CertificateTypes/     # request form templates per certificate type
└── PrintOutput/          # generated CombinedOutput.pdf (created on demand)
```

`identifier.sqlite` is copied to the output directory on build. When you publish, copy the whole `Data/` folder (including the `Certificates/` and `CertificateTypes/` folders) next to the executable.

### Database schema

| Table             | Purpose                                                              |
|-------------------|----------------------------------------------------------------------|
| `Accounts`        | Login credentials (`Username`, `Password`)                           |
| `Sailors`         | `ID`, `Name`, `Surname`, `IsRefresh`, unique `GID`                   |
| `CertificateTypes`| `ID`, `Name`                                                         |
| `Certificates`    | A sailor's certificate: type, `DateAcquired`, `DateExpiration`, `Place` |
| `SailorFiles`     | Files attached to a sailor's certificate (`FilePath`)                |
| `RequestFiles`    | Request form templates per certificate type, with `RequestType` (1 = Sticanje, 2 = Obnova, 3 = Refresh) |

Deleting a sailor or certificate type cascades to their certificates and files in the database.

## Project structure

```
docutrack.sln
└── DocuTrack/
    ├── Pages/          # MainView.axaml (main window + sidebar), MainViewModel, BaseViewModel
    ├── Views/          # Avalonia views (.axaml + code-behind)
    ├── ViewModels/     # View models, one per view
    ├── DataModels/     # Sailor, Certificate, CertificateType, RequestFile, ...
    ├── Data/           # DatabaseHelper (all SQL) and the SQLite database
    ├── Styles/         # Per-screen .axaml style files
    ├── UI/Scaling/     # Zoom / UI scaling infrastructure
    ├── Converters/     # Value converters
    ├── Assets/         # Logos, icons, arrows
    ├── Program.cs      # Entry point
    └── App.axaml(.cs)  # Application setup
```

Navigation is handled by `MainViewModel`, which swaps the current page between the login screen, expirations, sailors and certificate types. Views are resolved from view models by `ViewLocator`.

## Security notes

- The database ships with a default `admin` account. **Change its password before real use.**
- Passwords are currently stored and compared as plain text in the `Accounts` table. Hashing them (for example with PBKDF2 or Argon2) is recommended before storing real credentials.
- The database and attached documents contain personal data. Keep the `Data/` folder in a protected location and include it in your backups.

## Contributing

1. Create a feature branch.
2. Keep database access in `DatabaseHelper`, UI logic in view models, and styling in `Styles/`.
3. Open a pull request describing the change.

## License

No license has been specified yet. Add a `LICENSE` file before distributing the project.
