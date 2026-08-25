# Lagerverwaltungssystem (Inventory Management System)

<p align="center">
  <a href="#-deutsch">Deutsch</a> • 
  <a href="#-english">English</a>
</p>

---

## 🇩🇪 Deutsch

### 🚀 Funktionen
* **Vollständige CRUD-Operationen**: Nahtloses Erstellen, Lesen, Aktualisieren und Löschen von Lagerartikeln in Echtzeit.
* **Asynchrone Datenverarbeitung**: Alle Datenbankoperationen laufen vollständig asynchron (`async/await`), um ein Einfrieren der Benutzeroberfläche (UI-Freeze) zu verhindern.
* **Intelligente Dateneingabe & Validierung**: Automatische Formatierung bei der Preiseingabe (Punkte werden für das deutsche Nummernformat autonom in Kommas umgewandelt). Fehleingaben werden durch Validierungsprüfungen abgefangen und dem Nutzer signalisiert.
* **Erhöhte Datensicherheit**: Integrierte Sicherheitsabfrage vor dem endgültigen Löschen eines Artikels aus der Datenbank.
* **Dynamische UI-Steuerung**: Die Buttons "Löschen" und "Speichern" aktivieren oder deaktivieren sich kontextabhängig (über einen modernen `AsyncRelayCommand`) nur bei Selektion eines Artikels. Zudem wird ein "Doppel-Klick"-Fehler bei asynchronen Prozessen abgefangen.
* **Persistente Speicherung**: Alle Daten bleiben durch die lokale Datenbankverbindung auch nach dem Schließen der Anwendung vollständig erhalten.
* **Wirtschaftlicher Nutzen**: Das System automatisiert Prozesse und minimiert Erfassungsfehler – ein zentraler Aspekt für die IHK-Projektbewertung.

### 🛠️ Technologien
* C# 14 & .NET 10.0 (LTS)
* GUI-Framework: **WPF** (Windows Presentation Foundation) mit modernisiertem XAML-Layout (Hover-Effekte, strukturierte Tabellenansicht)
* Architekturmuster: **MVVM** (Model-View-ViewModel) für saubere Funktionstrennung
* ORM-Mapper: **Entity Framework Core** (EF Core Code-First)
* Datenbank: **SQLite** (Generiert sich beim ersten App-Start vollautomatisch als `lager.db` über kurzlebige Kontexte zur Vermeidung von Speicherlecks)

### 📂 Struktur der Benutzeroberfläche
Das WPF-Fenster ist wie folgt aufgeteilt:
* **Lagerbestand-Übersicht**: Ein zentrales, optisch optimiertes `DataGrid`, das alle Artikel mit ID, Artikelnummer, Bezeichnung, Bestand und Preis (€) listet.
* **Artikelverwaltung**: Ein Eingabebereich (`GroupBox`) im unteren Bereich zum Hinzufügen neuer Artikel sowie Steuerungselemente zum Ändern oder Löschen bestehender Einträge.

### 📦 Installation & Ausführung

#### Option 1: Standalone-Ausführung (Empfohlen für IHK-Prüfer)
Für die Ausführung des Programms ist keine Installation von .NET Core oder Entwicklungsumgebungen erforderlich. Die Anwendung wurde als eigenständige Einzeldatei kompiliert:
1. Wechseln Sie in den Bereich **Releases** dieses Repositories (oder nutzen Sie die bereitgestellte ausführbare Datei).
2. Laden Sie die ausführbare Datei `Lagerverwaltung.exe` herunter.
3. Starten Sie die Anwendung per Doppelklick. Die Model-Datei `lager.db` wird vollautomatisch im selben Verzeichnis angelegt.

#### Option 2: Start über CLI (Entwicklungsmodus)
1. Repository klonen:
   ```bash
   git clone https://github.com
   ```
2. In den Projektordner wechseln:
   ```bash
   cd Lagerverwaltung
   ```
3. Anwendung kompilieren und ausführen:
   ```bash
   dotnet run
   ```

---

## 🇺🇸 English

### 🚀 Features
* **Full CRUD Operations**: Seamless real-time creation, reading, updating, and deletion of inventory items.
* **Asynchronous Data Operations**: Fully non-blocking database queries (`async/await`) ensuring a smooth, freeze-free UI experience.
* **Smart Data Input & Validation**: Automatic formatting for price text fields (converts dots to commas for the German standard) combined with strict user input validation.
* **Data Safety**: Built-in confirmation prompt to prevent accidental deletion of inventory items.
* **Dynamic UI Control**: "Delete" and "Save" buttons dynamically toggle availability via an advanced `AsyncRelayCommand` when an item is selected, preventing accidental double-triggering.
* **Persistent Data**: Short-lived database context ensures zero memory leaks while safely persisting data across application restarts.
* **Business Value**: Automates core processes and reduces human error – a crucial evaluation metric for German IHK exam standards.

### 🛠️ Technologies
* C# 14 & .NET 10.0 (LTS)
* GUI Framework: **WPF** (Windows Presentation Foundation) via stylized XAML (custom hover states, alternating row backgrounds)
* Design Pattern: **MVVM** (Model-View-ViewModel) for clean separation of concerns
* ORM Mapper: **Entity Framework Core** (EF Core Code-First)
* Database: **SQLite** (The database file `lager.db` is automatically deployed on the first launch)

### 📂 User Interface Structure
The layout is divided into the following sections:
* **Inventory Overview**: An enhanced main `DataGrid` component listing all items by ID, article number, description, stock count, and formatted price (€).
* **Item Management**: A stylized control panel (`GroupBox`) at the bottom featuring focus-responsive input fields and actions for modifying or deleting selected entries.

### 📦 Installation & Setup

#### Option 1: Standalone Execution (Recommended)
No prior installation of .NET Runtimes or Visual Studio is required. The app is packaged as a Self-Contained single executable file:
1. Navigate to the **Releases** tab of this repository.
2. Download the standalone executable file `Lagerverwaltung.exe`.
3. Double-click to run on Windows (x64). The database `lager.db` will deploy itself automatically in the same folder.

#### Option 2: Build from Source
1. Clone the repository:
   ```bash
   git clone https://github.com
   ```
2. Navigate to the project directory:
   ```bash
   cd Lagerverwaltung
   ```
3. Build and run the application:
   ```bash
   dotnet run
   ```
