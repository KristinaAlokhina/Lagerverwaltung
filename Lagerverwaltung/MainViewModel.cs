using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;

namespace Lagerverwaltung
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Product> _products = null!;
        private Product? _selectedProduct;

        // Felder für die Dateneingabe über die Benutzeroberfläche
        private string _newName = string.Empty;
        private string _newArticleNumber = string.Empty;
        private string _newStockInput = "0";
        private string _newPriceInput = "0,00";

        private string _filterText = string.Empty;
        private ICollectionView? _productsView;

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged();
                _productsView?.Refresh(); 
            }
        }

        // Eigenschaften für das Data-Binding mit den TextBoxen in der XAML-Ansicht
        public string NewName { get => _newName; set { _newName = value; OnPropertyChanged(); } }
        public string NewArticleNumber { get => _newArticleNumber; set { _newArticleNumber = value; OnPropertyChanged(); } }
        public string NewStock { get => _newStockInput; set { _newStockInput = value; OnPropertyChanged(); } }
        public string NewPrice { get => _newPriceInput; set { _newPriceInput = value; OnPropertyChanged(); } }

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
                (DeleteCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
                (UpdateStockCommand as AsyncRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        // Asynchrone ICommand-Eigenschaften für die Button-Aktionen (verhindert UI-Freeze)
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand UpdateStockCommand { get; }

        public MainViewModel()
        {
            // Verknüpfung der Buttons mit den entsprechenden asynchronen Methoden
            AddCommand = new AsyncRelayCommand(AddProductAsync);
            DeleteCommand = new AsyncRelayCommand(DeleteProductAsync, () => SelectedProduct != null);
            UpdateStockCommand = new AsyncRelayCommand(UpdateStockAsync, () => SelectedProduct != null);

            // Asynchrones Laden der Datenbank beim Starten des Programms
            _ = InitializeDatabaseAsync();
        }

        // Initialisiert die Datenbank im Hintergrund
        private async Task InitializeDatabaseAsync()
        {
            using var context = new AppDbContext();
            await context.Database.EnsureCreatedAsync();
            var list = await context.Products.ToListAsync();
            Products = new ObservableCollection<Product>(list);

            _productsView = CollectionViewSource.GetDefaultView(Products);
            _productsView.Filter = FilterProductsMethod;
        }

        // Methode zum Hinzufügen eines Produkts mit strenger Validierung
        private async Task AddProductAsync()
        {
            if (string.IsNullOrWhiteSpace(NewName))
            {
                MessageBox.Show("Bitte geben Sie einen Produktnamen ein.", "Validierungsfehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Sichere Konvertierung und Validierung des Lagerbestands
            if (!int.TryParse(NewStock, out int parsedStock) || parsedStock < 0)
            {
                MessageBox.Show("Ungültiger Lagerbestand. Bitte geben Sie eine positive ganze Zahl ein.", "Validierungsfehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Ersetzt Punkte automatisch durch Kommas für das deutsche Zahlenformat
            string sanitizedPrice = NewPrice.Replace('.', ',');
            // Konvertiert den String-Preis in ein Decimal-Format unter Verwendung der deutschen Formatierungsregeln
            if (!decimal.TryParse(sanitizedPrice, CultureInfo.GetCultureInfo("de-DE"), out decimal parsedPrice) || parsedPrice < 0)
            {
                MessageBox.Show("Ungültiger Preis. Bitte ein korrektes Format eingeben (z.B. 12,34).", "Validierungsfehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var product = new Product
            {
                Name = NewName,
                ArticleNumber = NewArticleNumber,
                Stock = parsedStock,
                Price = parsedPrice
            };

            // Kurzlebiger Kontext verhindert Speicherlecks (Memory Leaks)
            using (var context = new AppDbContext())
            {
                context.Products.Add(product);
                await context.SaveChangesAsync(); // Speichert asynchron in der SQLite-Datenbank
            }

            Products.Add(product); // Aktualisiert die UI-Liste

            // Setzt die Eingabefelder nach dem erfolgreichen Hinzufügen zurück
            NewName = string.Empty;
            NewArticleNumber = string.Empty;
            NewStock = "0";
            NewPrice = "0,00";
        }

        // Methode zum asynchronen Löschen eines Produkts
        private async Task DeleteProductAsync()
        {
            if (SelectedProduct == null) return;

            var result = MessageBox.Show($"Möchten Sie das Produkt '{SelectedProduct.Name}' wirklich löschen?", "Löschen bestätigen", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            using (var context = new AppDbContext())
            {
                context.Products.Remove(SelectedProduct);
                await context.SaveChangesAsync();
            }

            Products.Remove(SelectedProduct);
        }

        // Methode zum Speichern von Änderungen direkt aus der DataGrid-Zeile
        private async Task UpdateStockAsync()
        {
            if (SelectedProduct == null) return;

            using (var context = new AppDbContext())
            {
                context.Products.Update(SelectedProduct);
                await context.SaveChangesAsync();
            }

            MessageBox.Show("Bestand und Änderungen erfolgreich aktualisiert!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool FilterProductsMethod(object item)
        {
            if (item is Product product)
            {
                if (string.IsNullOrWhiteSpace(FilterText))
                    return true;

                string query = FilterText.Trim().ToLower();

                bool nameMatch = product.Name != null && product.Name.ToLower().Contains(query);
                bool numberMatch = product.ArticleNumber != null && product.ArticleNumber.ToLower().Contains(query);

                return nameMatch || numberMatch;
            }
            return false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Moderne Implementierung von AsyncRelayCommand zur Vermeidung von Doppel-Klicks bei asynchronen Operationen
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool>? _canExecute;
        private bool _isExecuting;

        public AsyncRelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => !_isExecuting && (_canExecute == null || _canExecute());

        public async void Execute(object? parameter)
        {
            _isExecuting = true;
            RaiseCanExecuteChanged();
            try
            {
                await _execute();
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        public event EventHandler? CanExecuteChanged;

    }
}
