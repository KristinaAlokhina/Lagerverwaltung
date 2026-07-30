# Lagerverwaltungssystem (Inventory Management System)

<p align="center">
  <a href="#-deutsch">Deutsch</a> • 
  <a href="#-english">English</a>
</p>

---

## 🇩🇪 Deutsch

### 🚀 Funktionen
* **Vollständige CRUD-Operationen**: Nahtloses Erstellen, Lesen, Aktualisieren und Löschen von Lagerartikeln in Echtzeit.
* **Intelligente Dateneingabe**: Automatische Formatierung bei der Preiseingabe (Punkte werden für das deutsche Nummernformat autonom in Kommas umgewandelt).
* **Dynamische UI-Steuerung**: Die Buttons "Löschen" und "Speichern" aktivieren oder deaktivieren sich kontextabhängig nur bei Selektion eines Artikels.
* **Persistente Speicherung**: Alle Daten bleiben durch die lokale Datenbankverbindung auch nach dem Schließen der Anwendung vollständig erhalten.
* **Wirtschaftlicher Nutzen**: Das System automatisiert Prozesse und minimiert Erfassungsfehler – ein zentraler Aspekt für die IHK-Projektbewertung.

### 🛠️ Technologien
* C# 14 & .NET 10.0 (LTS)
* GUI-Framework: **WPF** (Windows Presentation Foundation) mit XAML-Layout
* Architekturmuster: **MVVM** (Model-View-ViewModel) für saubere Funktionstrennung
* ORM-Mapper: **Entity Framework Core** (EF Core Code-First)
* Datenbank: **SQLite** (Generiert sich beim ersten App-Start vollautomatisch als `lager.db`)

### 📂 Struktur der Benutzeroberfläche
Das WPF-Fenster ist wie folgt aufgeteilt:
* **Lagerbestand-Übersicht**: Ein zentrales `DataGrid`, das alle Artikel mit ID, Artikelnummer, Bezeichnung, Bestand und Preis (€) listet.
* **Artikelverwaltung**: Ein Eingabebereich (`GroupBox`) im unteren Bereich zum Hinzufügen neuer Artikel sowie Steuerungсеlemente zum Ändern oder Löschen bestehender Einträge.

### 📦 Installation & Ausführung

#### Option 1: Standalone-Ausführung (Empfohlen für IHK-Prüfer)
Для выполнения программы не требуется установка .NET Core или сред разработки:
1. Wechseln Sie in den Bereich **Releases** dieses Repositories.
2. Laden Sie die ausführbare Datei `Lagerverwaltung.exe` herunter.
3. Starten Sie die Anwendung per Doppelklick. Die Model-Datei `lager.db` wird vollautomatisch im selben Verzeichnis angelegt.

#### Option 2: Start über CLI (Entwicklungsmodus)
1. Repository klonen:
   ```bash
   git clone https://[github.com](https://github.com/KristinaAlokhina/Lagerverwaltung)
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
* **Smart Data Input**: Automatic formatting for price text fields (automatically converts dots to commas to fit the German numbering standard).
* **Dynamic UI Control**: The "Delete" and "Save" buttons toggle their availability contextually only when an item is selected.
* **Persistent Data**: All inventory records are securely stored locally and remain intact after closing the application.
* **Business Value**: The application automates core processes and reduces human error – a crucial metric for German IHK exam standards.

### 🛠️ Technologies
* C# 14 & .NET 10.0 (LTS)
* GUI Framework: **WPF** (Windows Presentation Foundation) via XAML
* Design Pattern: **MVVM** (Model-View-ViewModel) for clean separation of concerns
* ORM Mapper: **Entity Framework Core** (EF Core Code-First)
* Database: **SQLite** (The database file `lager.db` is automatically created on the first launch)

### 📂 User Interface Structure
The layout is divided into the following sections:
* **Inventory Overview**: A main `DataGrid` component listing all items by ID, article number, description, stock count, and price (€).
* **Item Management**: A control panel (`GroupBox`) at the bottom featuring input fields for adding items and actions for modifying or deleting selected entries.

### 📦 Installation & Setup

#### Option 1: Standalone Execution (Recommended)
No prior installation of .NET Runtimes or Visual Studio is required:
1. Navigate to the **Releases** tab of this repository.
2. Download the executable file `Lagerverwaltung.exe`.
3. Double-click to run on Windows (x64). The database `lager.db` will deploy itself in the background.

#### Option 2: Build from Source
1. Clone the repository:
   ```bash
   git clone https://[github.com](https://github.com/KristinaAlokhina/Lagerverwaltung)
   ```
2. Navigate to the project directory:
   ```bash
   cd Lagerverwaltung
   ```
3. Build and run the application:
   ```bash
   dotnet run
   ```
