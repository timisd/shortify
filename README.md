# shortify

Shortify ist ein einfacher URL Kürzer

## Voraussetzungen

- .NET Core SDK muss installiert sein

## Installation

1. Repository klonen:
   ```bash
   git clone https://github.com/timisd/shortify.git
   ```
2. In das Projektverzeichnis wechseln:
   ```bash
   cd shortify
   ```

## Datenbank aktualisieren

Um die API auszuführen, muss die EF-Datenbank aktualisiert werden. Dafür muss man im Ordner `src` sein und folgenden Befehl ausführen:

```bash
dotnet ef database update --project Shortify.Persistence.EfCore --startup-project Shortify.API
```

## API starten

1. In das API-Verzeichnis wechseln:
   ```bash
   cd src/Shortify.API
   ```
2. API starten:
   ```bash
   dotnet run
   ```

## Nutzung

Nach dem Start der API kann man sie unter `http://localhost:5134` erreichen und verwenden, um URLs zu kürzen.
