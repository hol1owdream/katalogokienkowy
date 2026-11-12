using System.Globalization;

namespace KatalogMobilny;

public partial class NewProductPage : ContentPage
{
    public NewProductPage()
    {
        InitializeComponent();
    }

    private void NowyProdukt_Clicked(object sender, EventArgs e)
    {
        var nazwa = NazwaEntry.Text?.Trim();
        if (string.IsNullOrWhiteSpace(nazwa))
        {
            PokazKomunikat("Wpisz nazwę produktu.");
            return;
        }

        if (!TryParseCena(CenaEntry.Text, out var cena) || cena < 0)
        {
            PokazKomunikat("Wpisz poprawną, nieujemną cenę.");
            CenaEntry.Focus();
            return;
        }

        if (!int.TryParse(IloscEntry.Text?.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out var ilosc) || ilosc < 0)
        {
            PokazKomunikat("Wpisz poprawną, nieujemną ilość.");
            IloscEntry.Focus();
            return;
        }

        if (KategoriaPicker.SelectedItem is not string kategoria)
        {
            PokazKomunikat("Wybierz kategorię produktu.");
            return;
        }

        var wartosc = cena * ilosc;
        var dostepnosc = DostepnoscSwitch.IsToggled ? "dostępny" : "niedostępny";
        PokazKomunikat($"Produkt: {nazwa}\nCena: {cena:0.00} zł\nIlość: {ilosc}\n" +
                       $"Kategoria: {kategoria}\nWartość: {wartosc:0.00} zł\nStatus: {dostepnosc}");
    }

    private void Reset_Clicked(object sender, EventArgs e)
    {
        NazwaEntry.Text = string.Empty;
        CenaEntry.Text = string.Empty;
        IloscEntry.Text = string.Empty;
        KategoriaPicker.SelectedItem = null;
        DostepnoscSwitch.IsToggled = false;
        DostepnoscLabel.Text = "Produkt niedostępny";
        PokazKomunikat("Formularz został wyczyszczony.");
    }

    private void DostepnoscSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        DostepnoscLabel.Text = e.Value ? "Produkt dostępny" : "Produkt niedostępny";
    }

    private static bool TryParseCena(string? tekst, out decimal cena)
    {
        if (decimal.TryParse(tekst?.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out cena))
        {
            return true;
        }

        return decimal.TryParse(tekst?.Trim()?.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out cena);
    }

    private void PokazKomunikat(string tekst)
    {
        EtykietaWyniku.Text = tekst;
        SemanticScreenReader.Announce(tekst);
    }
}