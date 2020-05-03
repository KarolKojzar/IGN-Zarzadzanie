using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;

namespace IGN_Zarzadzanie
{
    /// <summary>
    /// </summary>
    /// 
    /// Interaction logic for MainWindow.xaml

    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Wyloguj_BTN.Visibility = Visibility.Hidden;
            tabControl1.Visibility = Visibility.Hidden;
            uzytkownicy_tab.Visibility = Visibility.Hidden;
            statystyki_tab.Visibility = Visibility.Hidden;
            tabControl1.SelectedIndex = 0;
            Zgoda_na_odswiezenie_zamowienia = true;
            Zgoda_na_odswiezenie_material = true;
            Zgoda_na_odswiezenie_klient = true;
            Zgoda_na_odswiezenie_archiwum = true;
            Zgoda_na_odswiezenie_uzytkownik = true;
            Zgoda_na_odswiezenie_statystyki = true;
        }


        /* -----------------------------------------------------------------LOGOWANIE-----------------------------------------------*/
        private void Zaloguj_BTN_Click(object sender, RoutedEventArgs e)
        {
            pobierzdane();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                pobierzdane();
            }
        }

        public void pobierzdane()
        {
            string mojePolaczenie = "SERVER= 21615.m.tld.pl;" + "DATABASE= baza_1031;" + "UID= a_1031;" + "PASSWORD= 4SgrTvf^WF;";
            _zmienna.polaczenie = new MySqlConnection(mojePolaczenie);

            try
            {
                if (!string.IsNullOrEmpty(passwordBox.Password))
                {
                    string sql = "SELECT id_uzytkownicy FROM uzytkownicy WHERE password = BINARY '" + passwordBox.Password + "'";
                    _zmienna.polaczenie.Open();
                    _zmienna.dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Close();
                    da.Fill(_zmienna.dt);
                    if (_zmienna.dt.Rows.Count == 0)
                    {
                        Nazwa_uzytkownika = 0;
                        MessageBox.Show("Błędne hasło", "Błąd");
                    }
                    else
                    {
                        Nazwa_uzytkownika = Convert.ToInt16(_zmienna.dt.Rows[0].ItemArray[0]);
                        if (Nazwa_uzytkownika == 5 || Nazwa_uzytkownika == 3)
                        {
                            uzytkownicy_tab.Visibility = Visibility.Visible;
                            statystyki_tab.Visibility = Visibility.Visible;
                            rectangle7.Visibility = Visibility.Visible;
                            podlicz_archiwum_Button.Visibility = Visibility.Visible;
                            podlicz_archiwum_textBox.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            uzytkownicy_tab.Visibility = Visibility.Hidden;
                            statystyki_tab.Visibility = Visibility.Hidden;
                            rectangle7.Visibility = Visibility.Hidden;
                            podlicz_archiwum_Button.Visibility = Visibility.Hidden;
                            podlicz_archiwum_textBox.Visibility = Visibility.Hidden;
                        }
                        zamowienia_odswiezanie();
                        rectangle6.Visibility = Visibility.Visible;
                        podlicz_Button.Visibility = Visibility.Visible;
                        podlicz_textBox.Visibility = Visibility.Visible;
                        label2.Visibility = Visibility.Hidden;
                        label10.Visibility = Visibility.Hidden;
                        label11.Visibility = Visibility.Hidden;
                        edycja_hasla_textbox.Visibility = Visibility.Hidden;
                        edycja_uzytkownika_textbox.Visibility = Visibility.Hidden;
                        Zatwierdz_uzytkownika_BTN.Visibility = Visibility.Hidden;
                        Anuluj_uzytkownika_BTN.Visibility = Visibility.Hidden;
                        rectangle2.Visibility = Visibility.Hidden;
                        rectangle1.Visibility = Visibility.Hidden;
                        passwordBox.Visibility = Visibility.Hidden;
                        Zaloguj_BTN.Visibility = Visibility.Hidden;
                        Wyloguj_BTN.Visibility = Visibility.Visible;
                        tabControl1.Visibility = Visibility.Visible;
                        edycja_textbox.Visibility = Visibility.Hidden;
                        Anuluj_BTN.Visibility = Visibility.Hidden;
                        Zatwierdz_BTN.Visibility = Visibility.Hidden;
                        tabControl1.SelectedIndex = 0;
                    }
                }
                else
                {
                    Nazwa_uzytkownika = 0;
                    MessageBox.Show("Wprowadź hasło użytkownika", "Błąd");
                }
            }

            catch (MySql.Data.MySqlClient.MySqlException)
            {
                MessageBox.Show("Błąd logowania do bazy danych MySQL", "Błąd");
            }
        }

        private void Wyloguj_BTN_Click(object sender, RoutedEventArgs e)
        {
            Wyszukaj_klienta_textBox.Clear();
            Wyszukaj_produktu_textBox.Clear();
            zamowienie_wyszukiwanie_textBox.Clear();
            archiwum_wyszukiwanie_textBox.Clear();
            wyszukaj_statystyki_textBox.Clear();
            Wyszukaj_uzytkownika_textBox.Clear();

            for (int intCounter = App.Current.Windows.Count - 1; intCounter >= 0; intCounter--)
            {
                if (App.Current.Windows[intCounter].Title != "IGN-Zarzadzanie")
                {
                    App.Current.Windows[intCounter].Close();
                }
            }

            _zmienna.Row = -1;
            _zmienna.polaczenie.Close();
            passwordBox.Clear();
            Nazwa_uzytkownika = 0;
            tabControl1.SelectedIndex = 0;
            label2.Visibility = Visibility.Visible;
            passwordBox.Visibility = Visibility.Visible;
            Zaloguj_BTN.Visibility = Visibility.Visible;
            Wyloguj_BTN.Visibility = Visibility.Hidden;
            tabControl1.Visibility = Visibility.Hidden;
            edycja_textbox.Visibility = Visibility.Hidden;
            Anuluj_BTN.Visibility = Visibility.Hidden;
            Zatwierdz_BTN.Visibility = Visibility.Hidden;
            uzytkownicy_tab.Visibility = Visibility.Hidden;
            rectangle6.Visibility = Visibility.Hidden;
            podlicz_Button.Visibility = Visibility.Hidden;
            podlicz_textBox.Visibility = Visibility.Hidden;
            rectangle7.Visibility = Visibility.Hidden;
            podlicz_archiwum_Button.Visibility = Visibility.Hidden;
            podlicz_archiwum_textBox.Visibility = Visibility.Hidden;
            podlicz_statystyki_textBox.Clear();
            edycja_textbox.Clear();
            podlicz_archiwum_textBox.Clear();
            podlicz_textBox.Clear();
        }
        /* -----------------------------------------------------------------END_LOGOWANIE-----------------------------------------------*/


        private void btnSelectedTab_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Selected tab: " + (tabControl1.SelectedItem as TabItem).Header);
        }



        /* -----------------------------------------------------------------ZAMÓWIENIA-----------------------------------------------*/
        private void zamowienia_tab_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Zgoda_na_odswiezenie_zamowienia == true)
            {
                zamowienia_odswiezanie();
                Zgoda_na_odswiezenie_zamowienia = false;
                Zgoda_na_odswiezenie_material = true;
                Zgoda_na_odswiezenie_klient = true;
                Zgoda_na_odswiezenie_archiwum = true;
                Zgoda_na_odswiezenie_uzytkownik = true;
                Zgoda_na_odswiezenie_statystyki = true;
            }
        }

        public void zamowienia_odswiezanie()
        {
            string mojePolaczenie = "SERVER= 21615.m.tld.pl;" + "DATABASE= baza_1031;" + "UID= a_1031;" + "PASSWORD= 4SgrTvf^WF;";
            _zmienna.polaczenie = new MySqlConnection(mojePolaczenie);

            try
            {
                grid_zamowienia.Columns.Clear();
                _zmienna.polaczenie.Open();
                string sql = "SELECT DATE_FORMAT(data,'%Y-%m-%d') as data, id_zamowienia, nazwa, status_zamowienia, uzytkownik, status_faktury, status_zaplaty from zamowienia, status, klienci, uzytkownicy, faktury, zaplata WHERE klienci.nazwa   LIKE '%" + zamowienie_wyszukiwanie_textBox.Text + "%' AND zamowienia.id_status = status.id_status AND zamowienia.id_status != 4 AND zamowienia.id_klienci = klienci.id_klienci AND zamowienia.id_uzytkownicy = uzytkownicy.id_uzytkownicy AND zamowienia.id_faktury = faktury.id_faktury AND zamowienia.id_zaplata = zaplata.id_zaplata ORDER BY id_zamowienia";
                _zmienna.dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, _zmienna.polaczenie);
                _zmienna.polaczenie.Close();
                da.Fill(_zmienna.dt);
                if (_zmienna.dt.Rows.Count == 0)
                {
                    grid_zamowienia.Visibility = Visibility.Hidden;
                }
                else
                {
                    grid_zamowienia.Visibility = Visibility.Visible;
                    grid_zamowienia.DataContext = _zmienna.dt;
                }
            }

            catch (MySql.Data.MySqlClient.MySqlException)
            {

            }
        }

        private void grid_zamowienia_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grid_zamowienia.SelectedIndex != -1)
            {
                int edytuj = 0;
                foreach (DataRowView row in grid_zamowienia.SelectedItems)
                {
                    edytuj = (int)row.Row.ItemArray[1];
                }
                Wprowadz_zamowienie Wprowadz_Zamowienie = new Wprowadz_zamowienie();
                Wprowadz_Zamowienie.TheValue = "false";
                Wprowadz_Zamowienie.Index = edytuj;
                Wprowadz_Zamowienie.Show();
            }
        }

        private void Wprowadz_zamowienie_BTN_Click(object sender, RoutedEventArgs e)
        {
            Wprowadz_zamowienie Wprowadz_Zamowienie = new Wprowadz_zamowienie();
            Wprowadz_Zamowienie.Nazwa_uzytkownika = Nazwa_uzytkownika;
            Wprowadz_Zamowienie.TheValue = "true";
            Wprowadz_Zamowienie.Index = -1;
            Wprowadz_Zamowienie.Show();
        }

        private void Edytuj_zamowienie_BTN_Click(object sender, RoutedEventArgs e)
        {
            if (grid_zamowienia.SelectedIndex != -1)
            {
                int edytuj = 0;
                foreach (DataRowView row in grid_zamowienia.SelectedItems)
                {
                    edytuj = (int)row.Row.ItemArray[1];
                }
                Wprowadz_zamowienie Wprowadz_Zamowienie = new Wprowadz_zamowienie();
                Wprowadz_Zamowienie.TheValue = "false";
                Wprowadz_Zamowienie.Index = edytuj;
                Wprowadz_Zamowienie.Show();
            }
        }

        private void zamowienie_wyszukiwanie_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            zamowienia_odswiezanie();
        }

        private void grid_zamowienia_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (grid_zamowienia.SelectedIndex != -1 && grid_zamowienia.SelectedIndex != poprzednie_zaznaczenie)
            {
                _zmienna.Row = grid_zamowienia.SelectedIndex;
                poprzednie_zaznaczenie = _zmienna.Row;
            }
            else
            {
                grid_zamowienia.SelectedIndex = -1;
                poprzednie_zaznaczenie = -1;
                _zmienna.Row = -1;
            }
        }

        private void grid_zamowienia_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            grid_zamowienia.Columns[0].Width = 100;
            grid_zamowienia.Columns[1].Width = 60;
            grid_zamowienia.Columns[2].Width = 170;
            grid_zamowienia.Columns[3].Width = 125;
            grid_zamowienia.Columns[4].Width = 105;
            grid_zamowienia.Columns[5].Width = 100;
            grid_zamowienia.Columns[6].Width = 80;
            grid_zamowienia.Columns[0].Header = "Data:";
            grid_zamowienia.Columns[1].Header = "Nr zam.:";
            grid_zamowienia.Columns[2].Header = "Klienci:";
            grid_zamowienia.Columns[3].Header = "Status zamówienia:";
            grid_zamowienia.Columns[4].Header = "Zlecił:";
            grid_zamowienia.Columns[5].Header = "Faktura:";
            grid_zamowienia.Columns[6].Header = "Zapłata:";

            DataRowView item = e.Row.Item as DataRowView;
            if (item != null)
            {
                DataRow row = item.Row;
                var colValue = row[3];

                if (Convert.ToString(colValue) == "Zrobione")
                {
                    e.Row.Background = new SolidColorBrush(Colors.LightGreen);
                }
                if (Convert.ToString(colValue) == "W kolejce")
                {
                    e.Row.Background = new SolidColorBrush(Colors.LightCyan);
                }
                if (Convert.ToString(colValue) == "W realizacji")
                {
                    e.Row.Background = new SolidColorBrush(Colors.LightGoldenrodYellow);
                }
            }
        }

        private void Odswiez_zamowienie_Button_Click(object sender, RoutedEventArgs e)
        {
            podlicz_textBox.Clear();
            zamowienia_odswiezanie();
        }

        private void podlicz_Button_Click(object sender, RoutedEventArgs e)
        {
            string mojePolaczenie = "SERVER= 21615.m.tld.pl;" + "DATABASE= baza_1031;" + "UID= a_1031;" + "PASSWORD= 4SgrTvf^WF;";
            _zmienna.polaczenie = new MySqlConnection(mojePolaczenie);
            string sprawdzenie = "0";

            try
            {
                string sql = "Select COUNT(*) from klienci where klienci.nazwa   LIKE '%" + zamowienie_wyszukiwanie_textBox.Text + "%'";
                MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                _zmienna.polaczenie.Open();
                sprawdzenie = zapytanie.ExecuteScalar().ToString();
                _zmienna.polaczenie.Close();

                if (sprawdzenie == "0")
                {
                    podlicz_textBox.Text = "0 zł";
                }
                else
                {
                    sql = "Select sum(ilosc_m2 * cena_m2) from zamowienia_produkty, zamowienia, klienci where klienci.nazwa   LIKE '%" + zamowienie_wyszukiwanie_textBox.Text + "%' AND zamowienia.id_klienci = klienci.id_klienci AND zamowienia.id_status != '4' AND zamowienia.id_zamowienia = zamowienia_produkty.id_zamowienia";
                    zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    podlicz_textBox.Text = Convert.ToDouble(zapytanie.ExecuteScalar()).ToString("f2") + " zł";
                    _zmienna.polaczenie.Close();
                }
            }

            catch (MySql.Data.MySqlClient.MySqlException)
            {

            }
        }
        /* -----------------------------------------------------------------END_ZAMÓWIENIA-----------------------------------------------*/



        /* -----------------------------------------------------------------MATERIAŁY-----------------------------------------------*/
        private void materialy_click(object sender, MouseEventArgs e)
        {
            if (Zgoda_na_odswiezenie_material == true)
            {
                materialy_odswiezanie();
                Zgoda_na_odswiezenie_zamowienia = true;
                Zgoda_na_odswiezenie_material = false;
                Zgoda_na_odswiezenie_klient = true;
                Zgoda_na_odswiezenie_archiwum = true;
                Zgoda_na_odswiezenie_uzytkownik = true;
                Zgoda_na_odswiezenie_statystyki = true;
            }
        }

        public void materialy_odswiezanie()
        {
            string mojePolaczenie = "SERVER= 21615.m.tld.pl;" + "DATABASE= baza_1031;" + "UID= a_1031;" + "PASSWORD= 4SgrTvf^WF;";
            _zmienna.polaczenie = new MySqlConnection(mojePolaczenie);

            try
            {
                grid_materialy.Columns.Clear();
                _zmienna.polaczenie.Open();
                string sql = "SELECT * FROM produkty WHERE nazwa LIKE '%" + Wyszukaj_produktu_textBox.Text + "%' ORDER BY nazwa";
                _zmienna.dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, _zmienna.polaczenie);
                _zmienna.polaczenie.Close();
                da.Fill(_zmienna.dt);
                if (_zmienna.dt.Rows.Count == 0)
                {
                    grid_materialy.Visibility = Visibility.Hidden;
                }
                else
                {
                    grid_materialy.Visibility = Visibility.Visible;
                    ResultTable = AutoNumberedTable(_zmienna.dt);
                    grid_materialy.DataContext = ResultTable;
                }
            }

            catch (MySql.Data.MySqlClient.MySqlException)
            {

            }

        }

        private void Wprowadz_material_BTN_Click(object sender, RoutedEventArgs e)
        {
            Dodawanie_Produktow Dodaj_produkt = new Dodawanie_Produktow();
            Dodaj_produkt.Show();
        }

        private void Edytuj_material_BTN_Click(object sender, RoutedEventArgs e)
        {
            if (grid_materialy.SelectedIndex != -1)
            {
                Edycja_Produktow Edycja_produktow = new Edycja_Produktow();
                int edytuj = 0;
                foreach (DataRowView row in grid_materialy.SelectedItems)
                {
                    edytuj = (int)row.Row.ItemArray[1];
                }
                Edycja_produktow.edytowanie(edytuj);
            }
        }

        private void grid_materialy_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grid_materialy.SelectedIndex != -1)
            {
                Edycja_Produktow Edycja_produktow = new Edycja_Produktow();
                int edytuj = 0;
                foreach (DataRowView row in grid_materialy.SelectedItems)
                {
                    edytuj = (int)row.Row.ItemArray[1];
                }
                Edycja_produktow.edytowanie(edytuj);
            }
        }

        private void Usun_material_BTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sprawdzenie = "0";
                int kasuj = 0;
                foreach (DataRowView row in grid_materialy.SelectedItems)
                {
                    kasuj = (int)row.Row.ItemArray[1];
                }

                if (_zmienna.Row >= 0)
                {
                    string sql = "SELECT COUNT(*) FROM zamowienia_produkty WHERE id_produkty = '" + kasuj + "'";
                    MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);

                    _zmienna.polaczenie.Open();
                    sprawdzenie = zapytanie.ExecuteScalar().ToString();
                    _zmienna.polaczenie.Close();
                }

                if (_zmienna.dt.Rows.Count > 0 && _zmienna.Row != -1 && sprawdzenie == "0")
                {
                    string sql = "DELETE FROM produkty WHERE id_produkty = '" + kasuj + "'";
                    MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    zapytanie.ExecuteNonQuery();
                    _zmienna.polaczenie.Close();
                    materialy_odswiezanie();
                    grid_materialy.SelectedIndex = -1;
                    poprzednie_zaznaczenie = -1;
                    _zmienna.Row = -1;
                }
                else if (sprawdzenie != "0")
                {
                    MessageBox.Show("Nie można usunąć produktu. Występuje w zamówieniach.");
                }
            }

            catch (MySql.Data.MySqlClient.MySqlException)
            {

            }
        }

        private void grid_materialy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (grid_materialy.SelectedIndex != -1 && grid_materialy.SelectedIndex != poprzednie_zaznaczenie)
            {
                _zmienna.Row = grid_materialy.SelectedIndex;
                poprzednie_zaznaczenie = _zmienna.Row;
            }
            else
            {
                grid_materialy.SelectedIndex = -1;
                poprzednie_zaznaczenie = -1;
                _zmienna.Row = -1;
            }
        }

        private void Wyszukaj_produktu_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            materialy_odswiezanie();
        }

        private void grid_materialy_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            grid_materialy.Columns[0].Width = 50;
            grid_materialy.Columns[1].Width = 50;
            grid_materialy.Columns[2].Width = 430;
            grid_materialy.Columns[3].Width = 90;
            grid_materialy.Columns[4].Width = 90;
            grid_materialy.Columns[5].Width = 90;
            grid_materialy.Columns[1].Header = "ID:";
            grid_materialy.Columns[2].Header = "Produkt:";
            grid_materialy.Columns[3].Header = "Cena EKO:";
            grid_materialy.Columns[4].Header = "Cena Best:";
            grid_materialy.Columns[5].Header = "Cena Detal:";
            grid_materialy.Columns[1].Visibility = Visibility.Hidden;

            DataRowView item = e.Row.Item as DataRowView;
            if (item != null)
            {
                if (e.Row.GetIndex() % 2 == 0)
                {
                    e.Row.Background = new SolidColorBrush(Colors.LightCyan);
                }
                else
                {
                    e.Row.Background = new SolidColorBrush(Colors.White);
                }
            }
        }
        
        private void Odswiez_material_Button_Click(object sender, RoutedEventArgs e)
        {
            materialy_odswiezanie();
        }

        /* -----------------------------------------------------------------END_MATERIAŁY-----------------------------------------------*/


        /* -------------------------------------------------------------------KLIENT-----------------------------------------------*/
        private void klienci_click(object sender, MouseEventArgs e)
        {
            if (Zgoda_na_odswiezenie_klient == true)
            {
                klient_odswiezanie();
                Zgoda_na_odswiezenie_zamowienia = true;
                Zgoda_na_odswiezenie_material = true;
                Zgoda_na_odswiezenie_klient = false;
                Zgoda_na_odswiezenie_archiwum = true;
                Zgoda_na_odswiezenie_uzytkownik = true;
                Zgoda_na_odswiezenie_statystyki = true;
            }
        }

        private void Wprowadz_klienta_BTN_Click(object sender, RoutedEventArgs e)
        {
            string sprawdzenie;
            string sql = "SELECT COUNT(*) FROM klienci WHERE nazwa = '" + wprowadzanie_klienta.Text + "'";
            MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
            try
            {
                _zmienna.polaczenie.Open();
                sprawdzenie = zapytanie.ExecuteScalar().ToString();

                if (sprawdzenie == "0" && !string.IsNullOrEmpty(wprowadzanie_klienta.Text))
                {
                    sql = "INSERT INTO klienci (id_klienci, nazwa) values (NULL, '" + wprowadzanie_klienta.Text + "')";
                    zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    zapytanie.ExecuteNonQuery();
                }
                else if (string.IsNullOrEmpty(wprowadzanie_klienta.Text))
                {
                    MessageBox.Show("Wprowadź nazwę");
                }
                else
                {
                    MessageBox.Show("Podany klient już istnieje");
                }
                _zmienna.polaczenie.Close();
                klient_odswiezanie();
                wprowadzanie_klienta.Clear();
            }

            catch (MySql.Data.MySqlClient.MySqlException)
            {

            }
        }

        private void klient_odswiezanie()
        {
            grid_klienci.Columns.Clear();
            try
            {
                _zmienna.polaczenie.Open();
                string sql = "SELECT * FROM klienci WHERE nazwa LIKE '%" + Wyszukaj_klienta_textBox.Text + "%' ORDER BY nazwa";
                _zmienna.dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, _zmienna.polaczenie);
                _zmienna.polaczenie.Close();
                da.Fill(_zmienna.dt);

                if (_zmienna.dt.Rows.Count == 0)
                {
                    grid_klienci.Visibility = Visibility.Hidden;
                }
                else
                {
                    grid_klienci.Visibility = Visibility.Visible;
                    ResultTable = AutoNumberedTable(_zmienna.dt);
                    grid_klienci.DataContext = ResultTable;
                }
            }

            catch (MySql.Data.MySqlClient.MySqlException)
            {

            }
        }

        private void Usun_klienta_BTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sprawdzenie = "0";
                int kasuj = 0;
                foreach (DataRowView row in grid_klienci.SelectedItems)
                {
                    kasuj = (int)row.Row.ItemArray[1];
                }

                if (_zmienna.Row >= 0)
                {
                    string sql = "SELECT COUNT(*) FROM zamowienia WHERE id_klienci = '" + kasuj + "'";
                    MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    sprawdzenie = zapytanie.ExecuteScalar().ToString();
                    _zmienna.polaczenie.Close();
                }

                if (_zmienna.dt.Rows.Count > 0 && _zmienna.Row != -1 && sprawdzenie == "0")
                {
                    string sql = "DELETE FROM klienci WHERE id_klienci = '" + kasuj + "'";
                    MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    zapytanie.ExecuteNonQuery();
                    _zmienna.polaczenie.Close();
                    klient_odswiezanie();
                    grid_klienci.SelectedIndex = -1;
                    poprzednie_zaznaczenie = -1;
                    _zmienna.Row = -1;
                }
                else if (sprawdzenie != "0")
                {
                    MessageBox.Show("Nie można usunąć klienta. Występuje w zamówieniu.");
                }
            }

            catch (MySql.Data.MySqlClient.MySqlException)
            {

            }
        }

        private void grid_klienci_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (grid_klienci.SelectedIndex != -1 && grid_klienci.SelectedIndex != poprzednie_zaznaczenie)
            {
                _zmienna.Row = grid_klienci.SelectedIndex;
                poprzednie_zaznaczenie = _zmienna.Row;
            }
            else
            {
                grid_klienci.SelectedIndex = -1;
                poprzednie_zaznaczenie = -1;
                _zmienna.Row = -1;
            }
        }

        private void Edycja_BTN_Click(object sender, RoutedEventArgs e)
        {
            if (grid_klienci.SelectedIndex != -1)
            {
                try
                {
                    int edytuj = 0;
                    foreach (DataRowView row in grid_klienci.SelectedItems)
                    {
                        edytuj = (int)row.Row.ItemArray[1];
                    }
                    edycja_textbox.Visibility = Visibility.Visible;
                    Anuluj_BTN.Visibility = Visibility.Visible;
                    Zatwierdz_BTN.Visibility = Visibility.Visible;
                    rectangle1.Visibility = Visibility.Visible;
                    _zmienna.polaczenie.Open();
                    string sql = "SELECT nazwa FROM klienci WHERE id_klienci = '" + edytuj + "'";
                    MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);

                    using (MySqlDataReader reader = zapytanie.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            edycja_textbox.Text = reader.GetString(0);
                        }
                    }
                    _zmienna.polaczenie.Close();
                }

                catch (MySql.Data.MySqlClient.MySqlException)
                {

                }

            }
        }



        private void Zatwierdz_BTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int zatwierdz = 0;
                foreach (DataRowView row in grid_klienci.SelectedItems)
                {
                    zatwierdz = (int)row.Row.ItemArray[1];
                }
                string sql = "UPDATE klienci SET nazwa = '" + edycja_textbox.Text + "' WHERE id_klienci = '" + zatwierdz + "'";
                MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                _zmienna.polaczenie.Open();
                zapytanie.ExecuteNonQuery();
                _zmienna.polaczenie.Close();
                edycja_textbox.Clear();
                edycja_textbox.Visibility = Visibility.Hidden;
                Anuluj_BTN.Visibility = Visibility.Hidden;
                Zatwierdz_BTN.Visibility = Visibility.Hidden;
                rectangle1.Visibility = Visibility.Hidden;
                klient_odswiezanie();
            }

            catch (MySql.Data.MySqlClient.MySqlException)
            {

            }
        }

        private void Anuluj_BTN_Click(object sender, RoutedEventArgs e)
        {
            edycja_textbox.Clear();
            edycja_textbox.Visibility = Visibility.Hidden;
            Anuluj_BTN.Visibility = Visibility.Hidden;
            Zatwierdz_BTN.Visibility = Visibility.Hidden;
            rectangle1.Visibility = Visibility.Hidden;
        }

        private void Wyszukaj_klienta_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            klient_odswiezanie();
        }

        private void grid_klienci_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            grid_klienci.Columns[0].Width = 80;
            grid_klienci.Columns[1].Width = 480;
            grid_klienci.Columns[2].Width = 480;
            grid_klienci.Columns[0].Header = "Nr:";
            grid_klienci.Columns[1].Header = "ID:";
            grid_klienci.Columns[2].Header = "Nazwa:";
            grid_klienci.Columns[1].Visibility = Visibility.Hidden;

            DataRowView item = e.Row.Item as DataRowView;
            if (item != null)
            {
                if (e.Row.GetIndex() % 2 == 0)
                {
                    e.Row.Background = new SolidColorBrush(Colors.LightCyan);
                }
                else
                {
                    e.Row.Background = new SolidColorBrush(Colors.White);
                }
            }
        }

        private void Odswiez_klient_Button_Click(object sender, RoutedEventArgs e)
        {
            klient_odswiezanie();
        }
        /* -----------------------------------------------------------------END_KLIENT-----------------------------------------------*/

        /* -----------------------------------------------------------------ARCHIWUM-----------------------------------------------*/
        private void archiwum_click(object sender, MouseEventArgs e)
        {
            if(Zgoda_na_odswiezenie_archiwum == true)
            {
                archiwum_odswiezanie();
                Zgoda_na_odswiezenie_zamowienia = true;
                Zgoda_na_odswiezenie_material = true;
                Zgoda_na_odswiezenie_klient = true;
                Zgoda_na_odswiezenie_archiwum = false;
                Zgoda_na_odswiezenie_uzytkownik = true;
                Zgoda_na_odswiezenie_statystyki = true;
            }
        }

        public void archiwum_odswiezanie()
        {
            string mojePolaczenie = "SERVER= 21615.m.tld.pl;" + "DATABASE= baza_1031;" + "UID= a_1031;" + "PASSWORD= 4SgrTvf^WF;";
            _zmienna.polaczenie = new MySqlConnection(mojePolaczenie);

            try
            {
                grid_archiwum.Columns.Clear();
                _zmienna.polaczenie.Open();
                string sql = "SELECT DATE_FORMAT(data,'%Y-%m-%d') as data, id_zamowienia, nazwa, uzytkownik, status_faktury, status_zaplaty from zamowienia, status, klienci, uzytkownicy, faktury, zaplata WHERE klienci.nazwa   LIKE '%" + archiwum_wyszukiwanie_textBox.Text + "%' AND zamowienia.id_status = status.id_status AND zamowienia.id_status = '4' AND zamowienia.id_klienci = klienci.id_klienci AND zamowienia.id_faktury = faktury.id_faktury AND zamowienia.id_uzytkownicy = uzytkownicy.id_uzytkownicy AND zamowienia.id_zaplata = zaplata.id_zaplata ORDER BY id_zamowienia DESC";               
                _zmienna.dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, _zmienna.polaczenie);
                da.Fill(_zmienna.dt);
                _zmienna.polaczenie.Close();

                if (_zmienna.dt.Rows.Count == 0)
                {
                    grid_archiwum.Visibility = Visibility.Hidden;
                }
                else
                {
                    grid_archiwum.Visibility = Visibility.Visible;
                    grid_archiwum.DataContext = _zmienna.dt;
                }

            }

            catch (MySql.Data.MySqlClient.MySqlException)
            {

            }
        }

        private void grid_archiwum_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grid_archiwum.SelectedIndex != -1)
            {
                int edytuj = 0;
                foreach (DataRowView row in grid_archiwum.SelectedItems)
                {
                    edytuj = (int)row.Row.ItemArray[1];
                }
                Wprowadz_zamowienie Wprowadz_Zamowienie = new Wprowadz_zamowienie();
                Wprowadz_Zamowienie.TheValue = "false";
                Wprowadz_Zamowienie.Index = edytuj;
                Wprowadz_Zamowienie.Show();
            }
        }

        private void Edytuj_archiwum_BTN_Click(object sender, RoutedEventArgs e)
        {
            if (grid_archiwum.SelectedIndex != -1)
            {
                int edytuj = 0;
                foreach (DataRowView row in grid_archiwum.SelectedItems)
                {
                    edytuj = (int)row.Row.ItemArray[1];
                }
                Wprowadz_zamowienie Wprowadz_Zamowienie = new Wprowadz_zamowienie();
                Wprowadz_Zamowienie.TheValue = "false";
                Wprowadz_Zamowienie.Index = edytuj;
                Wprowadz_Zamowienie.Show();
            }
        }

        private void Usun_archiwum_BTN_Click(object sender, RoutedEventArgs e)
        {
            if (_zmienna.dt.Rows.Count > 0 && _zmienna.Row != -1)
            {
                try
                {
                    int kasuj = 0;
                    foreach (DataRowView row in grid_archiwum.SelectedItems)
                    {
                        kasuj = (int)row.Row.ItemArray[1];
                    }
                    string sql = "DELETE FROM zamowienia WHERE id_zamowienia = '" + kasuj + "'; SET SQL_SAFE_UPDATES = 0;DELETE FROM zamowienia_produkty WHERE id_zamowienia = '" + kasuj + "';";
                    MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    zapytanie.ExecuteNonQuery();
                    _zmienna.polaczenie.Close();
                    archiwum_odswiezanie();
                    grid_archiwum.SelectedIndex = -1;
                    poprzednie_zaznaczenie = -1;
                    _zmienna.Row = -1;
                }

                catch (MySql.Data.MySqlClient.MySqlException)
                {

                }
            }
        }

        private void archiwum_wyszukiwanie_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            archiwum_odswiezanie();
        }


        private void grid_archiwum_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (grid_archiwum.SelectedIndex != -1 && grid_archiwum.SelectedIndex != poprzednie_zaznaczenie)
            {
                _zmienna.Row = grid_archiwum.SelectedIndex;
                poprzednie_zaznaczenie = _zmienna.Row;
            }
            else
            {
                grid_archiwum.SelectedIndex = -1;
                poprzednie_zaznaczenie = -1;
                _zmienna.Row = -1;
            }
        }

        private void grid_archiwum_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            grid_archiwum.Columns[0].Width = 100;
            grid_archiwum.Columns[1].Width = 60;
            grid_archiwum.Columns[2].Width = 290;
            grid_archiwum.Columns[3].Width = 110;
            grid_archiwum.Columns[4].Width = 100;
            grid_archiwum.Columns[5].Width = 80;
            grid_archiwum.Columns[0].Header = "Data:";
            grid_archiwum.Columns[1].Header = "Nr zam.:";
            grid_archiwum.Columns[2].Header = "Klienci:";
            grid_archiwum.Columns[3].Header = "Zlecił:";
            grid_archiwum.Columns[4].Header = "Faktura:";
            grid_archiwum.Columns[5].Header = "Zapłata:";

            DataRowView item = e.Row.Item as DataRowView;
            if (item != null)
            {
                if (e.Row.GetIndex() % 2 == 0)
                {
                    e.Row.Background = new SolidColorBrush(Colors.LightCyan);
                }
                else
                {
                    e.Row.Background = new SolidColorBrush(Colors.White);
                }
            }
        }

        private void Odswiez_archiwum_Button_Click(object sender, RoutedEventArgs e)
        {
            podlicz_archiwum_textBox.Clear();
            archiwum_odswiezanie();
        }

        private void podlicz_archiwum_Button_Click(object sender, RoutedEventArgs e)
        {
            string mojePolaczenie = "SERVER= 21615.m.tld.pl;" + "DATABASE= baza_1031;" + "UID= a_1031;" + "PASSWORD= 4SgrTvf^WF;";
            _zmienna.polaczenie = new MySqlConnection(mojePolaczenie);
            string sprawdzenie = "0";

            try
            {
                string sql = "Select COUNT(*) from klienci where klienci.nazwa   LIKE '%" + archiwum_wyszukiwanie_textBox.Text + "%'";
                MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                _zmienna.polaczenie.Open();
                sprawdzenie = zapytanie.ExecuteScalar().ToString();
                _zmienna.polaczenie.Close();

                if (sprawdzenie == "0")
                {
                    podlicz_archiwum_textBox.Text = "0 zł";
                }
                else
                {
                    sql = "Select sum(ilosc_m2 * cena_m2) from zamowienia_produkty, zamowienia, klienci where klienci.nazwa   LIKE '%" + archiwum_wyszukiwanie_textBox.Text + "%' AND zamowienia.id_klienci = klienci.id_klienci AND zamowienia.id_status = '4' AND zamowienia.id_zamowienia = zamowienia_produkty.id_zamowienia";
                    zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    podlicz_archiwum_textBox.Text = Convert.ToDouble(zapytanie.ExecuteScalar()).ToString("f2") + " zł";
                    _zmienna.polaczenie.Close();
                }
            }

            catch (MySql.Data.MySqlClient.MySqlException)
            {

            }
        }
        /* -----------------------------------------------------------------END_ARCHIWUM-----------------------------------------------*/


        /* -----------------------------------------------------------------Statystyki-----------------------------------------------*/
        private void statystyki_click(object sender, MouseEventArgs e)
        {
            if (Zgoda_na_odswiezenie_statystyki == true)
            {
                statystyki_odswiezanie();
                Zgoda_na_odswiezenie_zamowienia = true;
                Zgoda_na_odswiezenie_material = true;
                Zgoda_na_odswiezenie_klient = true;
                Zgoda_na_odswiezenie_archiwum = true;
                Zgoda_na_odswiezenie_uzytkownik = true;
                Zgoda_na_odswiezenie_statystyki = false;
            }
        }

        private void statystyki_odswiezanie()
        {
            string sprawdzenie = "0";

            if (klienci_radioButton.IsChecked == true)
            {
                string mojePolaczenie = "SERVER= 21615.m.tld.pl;" + "DATABASE= baza_1031;" + "UID= a_1031;" + "PASSWORD= 4SgrTvf^WF;";
                _zmienna.polaczenie = new MySqlConnection(mojePolaczenie);

                try
                {
                    string sql = "Select COUNT(*) from klienci where klienci.nazwa   LIKE '%" + wyszukaj_statystyki_textBox.Text + "%'";
                    MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    sprawdzenie = zapytanie.ExecuteScalar().ToString();
                    _zmienna.polaczenie.Close();

                    if (sprawdzenie == "0")
                    {
                        podlicz_statystyki_textBox.Text = "0 zł";
                    }
                    else
                    {
                        sql = "Select sum(ilosc_m2 * cena_m2) from zamowienia_produkty, zamowienia, klienci where klienci.nazwa   LIKE '%" + wyszukaj_statystyki_textBox.Text + "%' AND zamowienia.id_klienci = klienci.id_klienci AND zamowienia.id_zamowienia = zamowienia_produkty.id_zamowienia";
                        zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                        _zmienna.polaczenie.Open();
                        podlicz_statystyki_textBox.Text = Convert.ToDouble(zapytanie.ExecuteScalar()).ToString("f2") + " zł";
                        _zmienna.polaczenie.Close();
                    }
                }

                catch (MySql.Data.MySqlClient.MySqlException)
                {

                }

                grid_statystyki.Columns.Clear();
                try
                {
                    _zmienna.polaczenie.Open();
                    string sql = "SELECT klienci.nazwa, ROUND(sum(ilosc_m2 * cena_m2),2) FROM zamowienia_produkty, zamowienia, klienci WHERE klienci.nazwa LIKE '%" + wyszukaj_statystyki_textBox.Text + "%' AND zamowienia.id_klienci = klienci.id_klienci AND zamowienia.id_zamowienia = zamowienia_produkty.id_zamowienia GROUP BY klienci.id_klienci ORDER BY sum(ilosc_m2 * cena_m2) DESC";
                    _zmienna.dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Close();
                    da.Fill(_zmienna.dt);

                    if (_zmienna.dt.Rows.Count == 0)
                    {
                        grid_statystyki.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        grid_statystyki.Visibility = Visibility.Visible;
                        grid_statystyki.DataContext = _zmienna.dt;
                    }
                }

                catch (MySql.Data.MySqlClient.MySqlException)
                {

                }
            }else if (materialy_radioButton.IsChecked == true)
            {
                string mojePolaczenie = "SERVER= 21615.m.tld.pl;" + "DATABASE= baza_1031;" + "UID= a_1031;" + "PASSWORD= 4SgrTvf^WF;";
                _zmienna.polaczenie = new MySqlConnection(mojePolaczenie);

                try
                {
                    string sql = "Select COUNT(*) from produkty where produkty.nazwa   LIKE '%" + wyszukaj_statystyki_textBox.Text + "%'";
                    MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    sprawdzenie = zapytanie.ExecuteScalar().ToString();
                    _zmienna.polaczenie.Close();

                    if (sprawdzenie == "0")
                    {
                        podlicz_statystyki_textBox.Text = "0 zł";
                    }
                    else
                    {
                        sql = "Select sum(ilosc_m2 * cena_m2) from zamowienia_produkty, zamowienia, produkty where produkty.nazwa   LIKE '%" + wyszukaj_statystyki_textBox.Text + "%' AND zamowienia_produkty.id_produkty = produkty.id_produkty AND zamowienia.id_zamowienia = zamowienia_produkty.id_zamowienia";
                        zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                        _zmienna.polaczenie.Open();
                        podlicz_statystyki_textBox.Text = Convert.ToDouble(zapytanie.ExecuteScalar()).ToString("f2") + " zł";
                        _zmienna.polaczenie.Close();
                    }
                }

                catch (MySql.Data.MySqlClient.MySqlException)
                {

                }

                grid_statystyki.Columns.Clear();
                try
                {
                    _zmienna.polaczenie.Open();
                    string sql = "SELECT produkty.nazwa, ROUND(sum(ilosc_m2 * cena_m2),2) FROM zamowienia_produkty, zamowienia, produkty WHERE produkty.nazwa LIKE '%" + wyszukaj_statystyki_textBox.Text + "%' AND zamowienia_produkty.id_produkty = produkty.id_produkty AND zamowienia.id_zamowienia = zamowienia_produkty.id_zamowienia GROUP BY produkty.id_produkty ORDER BY sum(ilosc_m2 * cena_m2) DESC";
                    _zmienna.dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Close();
                    da.Fill(_zmienna.dt);

                    if (_zmienna.dt.Rows.Count == 0)
                    {
                        grid_statystyki.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        grid_statystyki.Visibility = Visibility.Visible;
                        grid_statystyki.DataContext = _zmienna.dt;
                    }
                }

                catch (MySql.Data.MySqlClient.MySqlException)
                {

                }
            }
        }

        private void statystyki_wyszukiwanie_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            statystyki_odswiezanie();
        }

        private void grid_statystyki_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (materialy_radioButton.IsChecked == true)
            {
                grid_statystyki.Columns[0].Width = 200;
                grid_statystyki.Columns[1].Width = 300;
                grid_statystyki.Columns[0].Header = "Materiały:";
                grid_statystyki.Columns[1].Header = "Kwota:";
            }
            else
            {
                grid_statystyki.Columns[0].Width = 200;
                grid_statystyki.Columns[1].Width = 300;
                grid_statystyki.Columns[0].Header = "Klienci:";
                grid_statystyki.Columns[1].Header = "Kwota:";
            }

            DataRowView item = e.Row.Item as DataRowView;
            if (item != null)
            {
                if (e.Row.GetIndex() % 2 == 0)
                {
                    e.Row.Background = new SolidColorBrush(Colors.LightCyan);
                }
                else
                {
                    e.Row.Background = new SolidColorBrush(Colors.White);
                }
            }
        }

        private void Odswiez_statystyki_Button_Click(object sender, RoutedEventArgs e)
        {
            podlicz_statystyki_textBox.Clear();
            statystyki_odswiezanie();
        }

        private void materialy_radioButton_Checked(object sender, RoutedEventArgs e)
        {
            //klienci_radioButton.IsChecked = false;
            statystyki_odswiezanie();
        }

        private void klienci_radioButton_Checked(object sender, RoutedEventArgs e)
        {
            //materialy_radioButton.IsChecked = false;
            statystyki_odswiezanie();
        }

        /* -----------------------------------------------------------------END_Statystyki-----------------------------------------------*/


        /* -----------------------------------------------------------------UZYtkownicy-----------------------------------------------*/

        private void uzytkownicy_click(object sender, MouseEventArgs e)
        {
            if (Zgoda_na_odswiezenie_uzytkownik == true)
            {
                uzytkownicy_odswiezanie();
                Zgoda_na_odswiezenie_zamowienia = true;
                Zgoda_na_odswiezenie_material = true;
                Zgoda_na_odswiezenie_klient = true;
                Zgoda_na_odswiezenie_archiwum = true;
                Zgoda_na_odswiezenie_uzytkownik = false;
                Zgoda_na_odswiezenie_statystyki = true;
            }
        }

        private void Wprowadz_uzytkownika_BTN_Click(object sender, RoutedEventArgs e)
        {
            string sprawdzenie;
            string sql = "SELECT COUNT(*) FROM uzytkownicy WHERE uzytkownik = '" + wprowadzanie_uzytkownika.Text + "'";
            MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
            try
            {
                _zmienna.polaczenie.Open();
                sprawdzenie = zapytanie.ExecuteScalar().ToString();

                if (sprawdzenie == "0" && !string.IsNullOrEmpty(wprowadzanie_uzytkownika.Text))
                {
                    sql = "INSERT INTO uzytkownicy (id_uzytkownicy, uzytkownik, password) values (NULL, '" + wprowadzanie_uzytkownika.Text + "', '" + wprowadzanie_hasla_uzytkownika.Text + "')";
                    zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    zapytanie.ExecuteNonQuery();
                }
                else if (string.IsNullOrEmpty(wprowadzanie_uzytkownika.Text))
                {
                    MessageBox.Show("Wprowadź nazwę");
                }
                else
                {
                    MessageBox.Show("Podany użytkownik już istnieje");
                }
                _zmienna.polaczenie.Close();
                uzytkownicy_odswiezanie();
                wprowadzanie_uzytkownika.Clear();
                wprowadzanie_hasla_uzytkownika.Clear();
            }

            catch (MySql.Data.MySqlClient.MySqlException)
            {

            }
        }

        private void uzytkownicy_odswiezanie()
        {
            grid_uzytkownicy.Columns.Clear();
            try
            {
                _zmienna.polaczenie.Open();
                string sql = "SELECT * FROM uzytkownicy WHERE uzytkownik LIKE '%" + Wyszukaj_uzytkownika_textBox.Text + "%' ORDER BY uzytkownik";
                _zmienna.dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, _zmienna.polaczenie);
                _zmienna.polaczenie.Close();
                da.Fill(_zmienna.dt);

                if (_zmienna.dt.Rows.Count == 0)
                {
                    grid_uzytkownicy.Visibility = Visibility.Hidden;
                }
                else
                {
                    grid_uzytkownicy.Visibility = Visibility.Visible;
                    ResultTable = AutoNumberedTable(_zmienna.dt);
                    grid_uzytkownicy.DataContext = ResultTable;
                }
            }

            catch (MySql.Data.MySqlClient.MySqlException)
            {

            }
        }

        private void Usun_uzytkownika_BTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sprawdzenie = "0";
                int kasuj = 0;
                foreach (DataRowView row in grid_uzytkownicy.SelectedItems)
                {
                    kasuj = (int)row.Row.ItemArray[1];
                }

                if (_zmienna.Row >= 0)
                {
                    string sql = "SELECT COUNT(*) FROM zamowienia WHERE id_uzytkownicy = '" + kasuj + "'";
                    MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    sprawdzenie = zapytanie.ExecuteScalar().ToString();
                    _zmienna.polaczenie.Close();
                }

                if (_zmienna.dt.Rows.Count > 0 && _zmienna.Row != -1 && sprawdzenie == "0" && kasuj != 5 && kasuj != 3)
                {
                    string sql = "DELETE FROM uzytkownicy WHERE id_uzytkownicy = '" + kasuj + "'";
                    MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    zapytanie.ExecuteNonQuery();
                    _zmienna.polaczenie.Close();
                    uzytkownicy_odswiezanie();
                    grid_uzytkownicy.SelectedIndex = -1;
                    poprzednie_zaznaczenie = -1;
                    _zmienna.Row = -1;
                }
                else if (sprawdzenie != "0")
                {
                    MessageBox.Show("Nie można usunąć użytkownika. Występuje w zamówieniach.");
                }
                else if (Nazwa_uzytkownika == 5 || Nazwa_uzytkownika == 3)
                {
                    MessageBox.Show("Nie można usunąć administratora.");
                }
            }

            catch (MySql.Data.MySqlClient.MySqlException)
            {

            }
        }

        private void grid_uzytkownicy_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (grid_uzytkownicy.SelectedIndex != -1 && grid_uzytkownicy.SelectedIndex != poprzednie_zaznaczenie)
            {
                _zmienna.Row = grid_uzytkownicy.SelectedIndex;
                poprzednie_zaznaczenie = _zmienna.Row;
            }
            else
            {
                grid_uzytkownicy.SelectedIndex = -1;
                poprzednie_zaznaczenie = -1;
                _zmienna.Row = -1;
            }
        }

        private void Edycja_uzytkownika_BTN_Click(object sender, RoutedEventArgs e)
        {
            if (grid_uzytkownicy.SelectedIndex != -1)
            {
                try
                {
                    int edytuj = 0;
                    foreach (DataRowView row in grid_uzytkownicy.SelectedItems)
                    {
                        edytuj = (int)row.Row.ItemArray[1];
                    }
                    edycja_uzytkownika_textbox.Visibility = Visibility.Visible;
                    edycja_hasla_textbox.Visibility = Visibility.Visible;
                    Anuluj_uzytkownika_BTN.Visibility = Visibility.Visible;
                    Zatwierdz_uzytkownika_BTN.Visibility = Visibility.Visible;
                    label10.Visibility = Visibility.Visible;
                    label11.Visibility = Visibility.Visible;
                    rectangle2.Visibility = Visibility.Visible;
                    _zmienna.polaczenie.Open();
                    string sql = "SELECT uzytkownik FROM uzytkownicy WHERE id_uzytkownicy = '" + edytuj + "'";
                    MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);

                    using (MySqlDataReader reader = zapytanie.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            edycja_uzytkownika_textbox.Text = reader.GetString(0);
                        }
                    }
                    _zmienna.polaczenie.Close();
                }

                catch (MySql.Data.MySqlClient.MySqlException)
                {

                }

            }
        }



        private void Zatwierdz_uzytkownika_BTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int zatwierdz = 0;
                foreach (DataRowView row in grid_uzytkownicy.SelectedItems)
                {
                    zatwierdz = (int)row.Row.ItemArray[1];
                }
                string sql = "UPDATE uzytkownicy SET uzytkownik = '" + edycja_uzytkownika_textbox.Text + "', password = '" + edycja_hasla_textbox.Text + "' WHERE id_uzytkownicy = '" + zatwierdz + "'";
                MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                _zmienna.polaczenie.Open();
                zapytanie.ExecuteNonQuery();
                _zmienna.polaczenie.Close();
                edycja_uzytkownika_textbox.Clear();
                edycja_hasla_textbox.Clear();
                edycja_hasla_textbox.Visibility = Visibility.Hidden;
                edycja_uzytkownika_textbox.Visibility = Visibility.Hidden;
                Anuluj_uzytkownika_BTN.Visibility = Visibility.Hidden;
                Zatwierdz_uzytkownika_BTN.Visibility = Visibility.Hidden;
                label10.Visibility = Visibility.Hidden;
                label11.Visibility = Visibility.Hidden;
                rectangle2.Visibility = Visibility.Hidden;
                uzytkownicy_odswiezanie();
            }

            catch (MySql.Data.MySqlClient.MySqlException)
            {

            }
        }

        private void Anuluj_uzytkownika_BTN_Click(object sender, RoutedEventArgs e)
        {
            edycja_hasla_textbox.Clear();
            edycja_uzytkownika_textbox.Clear();
            edycja_hasla_textbox.Visibility = Visibility.Hidden;
            edycja_uzytkownika_textbox.Visibility = Visibility.Hidden;
            Anuluj_uzytkownika_BTN.Visibility = Visibility.Hidden;
            Zatwierdz_uzytkownika_BTN.Visibility = Visibility.Hidden;
            rectangle2.Visibility = Visibility.Hidden;
            label10.Visibility = Visibility.Hidden;
            label11.Visibility = Visibility.Hidden;
        }

        private void Wyszukaj_uzytkownika_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            uzytkownicy_odswiezanie();
        }

        private void grid_uzytkownicy_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            grid_uzytkownicy.Columns[0].Width = 80;
            grid_uzytkownicy.Columns[1].Width = 240;
            grid_uzytkownicy.Columns[2].Width = 240;
            grid_uzytkownicy.Columns[3].Width = 240;
            grid_uzytkownicy.Columns[0].Header = "Nr:";
            grid_uzytkownicy.Columns[1].Header = "ID:";
            grid_uzytkownicy.Columns[2].Header = "Nazwa:";
            grid_uzytkownicy.Columns[3].Header = "Hasło:";
            grid_uzytkownicy.Columns[1].Visibility = Visibility.Hidden;

            DataRowView item = e.Row.Item as DataRowView;
            if (item != null)
            {
                if (e.Row.GetIndex() % 2 == 0)
                {
                    e.Row.Background = new SolidColorBrush(Colors.LightCyan);
                }
                else
                {
                    e.Row.Background = new SolidColorBrush(Colors.White);
                }
            }
        }

        private void Odswiez_uzytkownik_Button_Click(object sender, RoutedEventArgs e)
        {
            uzytkownicy_odswiezanie();
        }
        /* -----------------------------------------------------------------END_UZYtkownicy-----------------------------------------------*/


        /* -----------------------------------------------ZAMYKANIE APLIKACJI----------------------------------------------------------*/

        private void IGN_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /* -----------------------------------------------------------------END_ZAMYKANIE-APLIKACJI-----------------------------------------------*/


        /* -----------------------------------------------ZMIENNE I FUNKCJE----------------------------------------------------------*/
        Zmienne _zmienna = new Zmienne();
        int poprzednie_zaznaczenie = -1;
        DataTable ResultTable;
        bool Zgoda_na_odswiezenie_klient;
        bool Zgoda_na_odswiezenie_uzytkownik;

        private bool _zgoda_zamowienie;
        public bool Zgoda_na_odswiezenie_zamowienia
        {
            get
            {
                return _zgoda_zamowienie;
            }
            set
            {
                _zgoda_zamowienie = value;
            }
        }

        private bool _zgoda_statystyki;
        public bool Zgoda_na_odswiezenie_statystyki
        {
            get
            {
                return _zgoda_statystyki;
            }
            set
            {
                _zgoda_statystyki = value;
            }
        }

        private bool _zgoda_material;
        public bool Zgoda_na_odswiezenie_material
        {
            get
            {
                return _zgoda_material;
            }
            set
            {
                _zgoda_material = value;
            }
        }

        private bool _zgoda_archiwum;
        public bool Zgoda_na_odswiezenie_archiwum
        {
            get
            {
                return _zgoda_archiwum;
            }
            set
            {
                _zgoda_archiwum = value;
            }
        }

        private DataTable AutoNumberedTable(DataTable SourceTable)
        {
            ResultTable = new DataTable();
            DataColumn AutoNumberColumn = new DataColumn();
            AutoNumberColumn.ColumnName = "Nr:";
            AutoNumberColumn.DataType = typeof(int);
            AutoNumberColumn.AutoIncrement = true;
            AutoNumberColumn.AutoIncrementSeed = 1;
            AutoNumberColumn.AutoIncrementStep = 1;
            ResultTable.Columns.Add(AutoNumberColumn);
            ResultTable.Merge(SourceTable);
            return ResultTable;
        }

        private int _nazwa_uzytkownika;
        public int Nazwa_uzytkownika
        {
            get
            {
                return _nazwa_uzytkownika;
            }
            set
            {
                _nazwa_uzytkownika = value;
            }
        }

        private void IGN_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            tabControl1.Height = IGN.Height - 80;

        }

        private void IGN_StateChanged(object sender, EventArgs e)
        {
            if (IGN.WindowState == System.Windows.WindowState.Maximized)
            {
                IGN.Height = SystemParameters.WorkArea.Height;
            }
            else
            {
                IGN.Height = 600;
            }
        }
    }

}