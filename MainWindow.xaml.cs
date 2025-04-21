using MahApps.Metro.Controls;
using System.IO;
using System.Windows;
using Newtonsoft.Json;



namespace IW_AplikacjaProjektZaliczeniowy
{
    public partial class mainWindow : MetroWindow
    {
        public List<Zawodnik> Zawodnicy { get; set; }

        public mainWindow()
        {
            InitializeComponent();

            Zawodnicy = new List<Zawodnik>();
            ZawodnicyDataGrid.ItemsSource = Zawodnicy;
        }

        private void BTNDodajZawodnika_Click(object sender, RoutedEventArgs e)
        {
            Zawodnicy.Add(new Zawodnik { Imie = "", Nazwisko = "", NrKoszulki = 0, DataUrodzenia = "" });

            ZawodnicyDataGrid.ItemsSource = null;
            ZawodnicyDataGrid.ItemsSource = Zawodnicy;
        }

        private void BTNUsun_Click(object sender, RoutedEventArgs e)
        {
            Zawodnicy.Clear();

            ZawodnicyDataGrid.ItemsSource = null;
            ZawodnicyDataGrid.ItemsSource = Zawodnicy;
        }

        private void BTNOtworz_Click(object sender, RoutedEventArgs e)
        {
            
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                if (!File.Exists(openFileDialog.FileName))
                {
                    MessageBox.Show("Plik nie istnieje.");
                    return;
                }

                try
                {
                    string JsonSerializacja = File.ReadAllText(openFileDialog.FileName);
                    List<Zawodnik> otwarciezawodnikow = JsonConvert.DeserializeObject<List<Zawodnik>>(JsonSerializacja);

                    if (otwarciezawodnikow != null)
                    {
                        Zawodnicy.AddRange(otwarciezawodnikow);

                        ZawodnicyDataGrid.ItemsSource = null;
                        ZawodnicyDataGrid.ItemsSource = Zawodnicy;

                        MessageBox.Show("Dane zostały wczytane ", "", MessageBoxButton.OK);
                    }
                    else
                    {
                        MessageBox.Show("Dane nie zostały wczytane", "Coś jest nie tak :/", MessageBoxButton.OK);
                    }
                }
                catch
                {
                    MessageBox.Show("Dane nie zostały wczytane", "Coś jest nie tak :/", MessageBoxButton.OK);
                }
            }
        }

        private void BTNZapisz_Click(object sender, RoutedEventArgs e)
        {

            var PusciZawodnicy = from Zawodnik in Zawodnicy
                                  where (Zawodnik.Imie == "" || Zawodnik.Imie == null) && (Zawodnik.Nazwisko == "" || Zawodnik.Nazwisko == null) && (Zawodnik.NrKoszulki == 0 || Zawodnik.NrKoszulki == null) && (Zawodnik.DataUrodzenia == "" || Zawodnik.DataUrodzenia == null)
                                  select Zawodnik;

            Zawodnicy = Zawodnicy.Except(PusciZawodnicy).ToList();

            ZawodnicyDataGrid.ItemsSource = null;
            ZawodnicyDataGrid.ItemsSource = Zawodnicy;

            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                if (string.IsNullOrEmpty(saveFileDialog.FileName))
                {
                    MessageBox.Show("Proszę podać ścieżkę do pliku", "Ehhhh...", MessageBoxButton.OK);
                    return;
                }

                try
                {
                    string ZawodnicySerializacja = JsonConvert.SerializeObject(Zawodnicy, Formatting.Indented);
                    File.WriteAllText(saveFileDialog.FileName, ZawodnicySerializacja);

                    MessageBox.Show("Dane zostały pomyślnie zapisane do pliku JSON", "No i pięknie", MessageBoxButton.OK);
                }
                catch
                {
                    MessageBox.Show("Błąd, nie zapisano danych", "Coś zrobiłeś nie tak", MessageBoxButton.OK);
                }
            }
        }

        private void BTNSzukaj_Click(object sender, RoutedEventArgs e)
        {
            string szukane = SearchTextBox.Text;

            if (string.IsNullOrEmpty(szukane))
            {
                ZawodnicyDataGrid.ItemsSource = null;
                ZawodnicyDataGrid.ItemsSource = Zawodnicy;
                return;
            }

            List<Zawodnik> listaWynikow = new List<Zawodnik>();

            foreach (var z in Zawodnicy)
            {
                if ((z.Imie != null && z.Imie.ToLower().Contains(szukane.ToLower()))
                    || (z.Nazwisko != null && z.Nazwisko.ToLower().Contains(szukane.ToLower()))
                    || z.NrKoszulki.ToString().Contains(szukane)
                    || (z.DataUrodzenia != null && z.DataUrodzenia.ToLower().Contains(szukane.ToLower())))
                {
                    listaWynikow.Add(z);
                }
            }

            ZawodnicyDataGrid.ItemsSource = null;
            ZawodnicyDataGrid.ItemsSource = listaWynikow;
        }

    }
}