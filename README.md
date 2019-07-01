# Osca Downloader

Ein einfaches Tool um alle deine Daten herunter zu laden.
Einfach [hier](https://github.com/ueman/osca-downloader/releases) ein Release herunterladen.

Unter anderem werden folgende Daten heruntergeladen:
- Noten (Prüfungsergebnisse)
- Module
- Ankündigungen
- Dateibereiche
- uvm

Die Daten landen größtenteils in einer Datenbank namens `osca.db`. Man kann sie beispielsweise mit [SQLite Browser](https://sqlitebrowser.org/) anschauen.

## Benutzung:

Auf der Kommandozeile kann `OscaDownloader` mit folgenden Parametern benutzt werden
```
  -u, --UserName         Required. Dein OSCA Benutzername

  -p, --Password         Required. Dein OSCA Passwort

  -c, --FedAuthCookie    Required. FedAuth-Cookie vom OscaPortal. Muss im
                         Format 'FedAuth=content' angegeben werden

  -o, --outputPath       Required. Dort werden alle Dateien abgespeichert

  --help                 Display this help screen.

  --version              Display version information.
```

## Was ist der `FedAuth`-Cookie und wie bekomme ich den?

1. Beispielsweise in Chrome im [Osca-Portal](https://osca.hs-osnabrueck.de) anmelden.
2. Auf das Schloss in der Adressleiste klicken.
3. Cookies anklicken
4. Auf osca.hs-osnabrueck.de -> Cookies -> FedAuth klicken
5. In der Kommandozeile mit `FedAuth=Inhalt` angeben

## Statistiken
Können leider noch nicht per CLI benutzt werden.
Können im Code [hier](Osca/Services/Statistics/StatisticsService.cs) gefunden werden.

### Wie baue ich ein Release-Build?

https://docs.microsoft.com/de-de/dotnet/core/deploying/deploy-with-cli
