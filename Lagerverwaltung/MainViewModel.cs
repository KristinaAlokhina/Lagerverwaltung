using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Lagerverwaltung
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly AppDbContext _context;
        private ObservableCollection<Product> _products = null!;
        private Product? _selectedProduct;

        // 1. Felder für die Dateneingabe über die Benutzeroberfläche (als Strings, damit WPF keine Kommas blockiert)
        private string _newName = string.Empty;
        private string _newArticleNumber = string.Empty;
        private string _newStockInput = "0";
        private string _newPriceInput = "0,00";

        // 2. Eigenschaften für das Data-Binding mit den TextBoxen in der XML-Ansicht
        public string NewName { get => _newName; set { _newName = value; OnPropertyChanged(); } }
        public string NewArticleNumber { get => _newArticleNumber; set { _newArticleNumber = value; OnPropertyChanged(); } }
        public string NewStock { get => _newStockInput; set { _newStockInput = value; OnPropertyChanged(); } }
        public string NewPrice { get => _newPriceInput; set { _newPriceInput = value; OnPropertyChanged(); } }

        public MainViewModel()
        {
            _context = new AppDbContext();
            // Erstellt die Datenbank automatisch beim ersten Start, falls sie noch nicht existiert
            _context.Database.EnsureCreated();

            LoadProducts();

            // Verknüpfung der Buttons mit den entsprechenden Methoden
            AddCommand = new RelayCommand(AddProduct);
            DeleteCommand = new RelayCommand(DeleteProduct, () => SelectedProduct != null);
            UpdateStockCommand = new RelayCommand(UpdateStock, () => SelectedProduct != null);
        }

        public ObservableCollection<Product> Products
        {
            get => _products;
            set { _products = value; OnPropertyChanged(); }
        }

        public Product? SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged();
                // Benachrichtigt die Buttons "Löschen" und "Speichern", ob sie aktiviert sein dürfen
                ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
                ((RelayCommand)UpdateStockCommand).RaiseCanExecuteChanged();
            }
        }

        // ICommand-Eigenschaften für die Button-Aktionen
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand UpdateStockCommand { get; }

        private void LoadProducts()
        {
            Products = new ObservableCollection<Product>(_context.Products.ToList());
        }

        // 3. Methode zum Hinzufügen eines Produkts mit Konvertierung von String in Zahlentypen
        private void AddProduct()
        {
            if (string.IsNullOrWhiteSpace(NewName)) return;

            // Sichere Konvertierung der Bestandsmenge in eine Ganzzahl
            int.TryParse(NewStock, out int parsedStock);

            // Ersetzt Punkte automatisch durch Kommas für das deutsche Zahlenformat
            string sanitizedPrice = NewPrice.Replace('.', ',');
            // Konvertiert den String-Preis in ein Decimal-Format unter Verwendung der deutschen Formatierungregeln
            decimal.TryParse(sanitizedPrice, System.Globalization.CultureInfo.GetCultureInfo("de-DE"), out decimal parsedPrice);

            var product = new Product
            {
                Name = NewName,
                ArticleNumber = NewArticleNumber,
                Stock = parsedStock,
                Price = parsedPrice
            };

            _context.Products.Add(product);
            _context.SaveChanges(); // Speichert die Änderungen direkt in der SQLite-Datenbankdatei
            Products.Add(product);    // Aktualisiert die Anzeige in der Tabelle auf dem Bildschirm

            // Setzt die Eingabefelder nach dem erfolgreichen Hinzufügen zurück
            NewName = string.Empty;
            NewArticleNumber = string.Empty;
            NewStock = "0";
            NewPrice = "0,00";
        }

        private void DeleteProduct()
        {
            if (SelectedProduct == null) return;

            _context.Products.Remove(SelectedProduct);
            _context.SaveChanges(); // Löscht den Eintrag dauerhaft aus der Datenbank
            Products.Remove(SelectedProduct); // Entfernt das Produkt aus der sichtbaren Liste
        }

        private void UpdateStock()
        {
            if (SelectedProduct == null) return;

            _context.SaveChanges(); // Speichert alle Änderungen, die direkt in der Tabellenzeile gemacht wurden
            MessageBox.Show("Bestand erfolgreich aktualisiert!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Hilfsklasse für die Verarbeitung von Klicks und Befehlen im MVVM-Entwurfsmuster
    public class RelayCommand : ICommand
    {
        private readonly System.Action _execute;
        private readonly System.Func<bool>? _canExecute;

        public RelayCommand(System.Action execute, System.Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();
        public void Execute(object? parameter) => _execute();
        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, System.EventArgs.Empty);
        public event System.EventHandler? CanExecuteChanged;
    }
}
