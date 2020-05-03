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
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.RegularExpressions;

namespace IGN_Zarzadzanie
{
    /// <summary>
    /// Interaction logic for Wprowadz_zamowienie.xaml
    /// </summary>
    public partial class Wprowadz_zamowienie : Window
    {
        /* -----------------------------------------------INICJALIZACJA FORMA----------------------------------------------------------*/
        public Wprowadz_zamowienie()
        {
            InitializeComponent();
            calendar1.Visibility = Visibility.Hidden;
            m2_suma_textBox.Text = "0";
            sztuki_textBox.Text = "0";
            cena_suma_textBox.Text = "0";
            cena_NETTO_textBox.Text = "0";
            cena_BRUTTO_textBox.Text = "0";
            szerokosc_textBox.Text = "1";
            dlugosc_textBox.Text = "1";
            m2_liczone_textBox.Text = "1";
            ilosc_sztuk_textBox.Text = "1";
            sumowanie = 0;
            sztuki_zliczanie = 0;
            DateTime czas = DateTime.Now;
            String.Format("{0:yyyy-MM-dd}", czas);
            data_textBox.Text = czas.Date.ToString("yyyy-MM-dd");
            eko_RadioBTN.Visibility = Visibility.Hidden;
            best_RadioBTN.Visibility = Visibility.Hidden;
            detal_RadioBTN.Visibility = Visibility.Hidden;
            zamowienie_DataTable.Columns.Add("Produkt:", typeof(string));
            zamowienie_DataTable.Columns.Add("Ilość:", typeof(float));
            zamowienie_DataTable.Columns.Add("Cena za m2:", typeof(float));
            zamowienie_DataTable.Columns.Add("Suma zł:", typeof(float));
            zamowienie_DataTable.Columns.Add("ID:", typeof(int));
            zamowienie_DataTable.Columns.Add("Sztuk:", typeof(int));
            zamowienie_DataTable.Columns.Add(" ", typeof(int));
        }

        private void zamowienia_dataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            zamowienia_dataGrid.Columns[0].Width = 250;
            zamowienia_dataGrid.Columns[1].Width = 60;
            zamowienia_dataGrid.Columns[2].Width = 90;
            zamowienia_dataGrid.Columns[3].Width = 70;
            zamowienia_dataGrid.Columns[4].Width = 20;
            zamowienia_dataGrid.Columns[5].Width = 50;
            zamowienia_dataGrid.Columns[6].Width = 20;
            zamowienia_dataGrid.Columns[0].Header = "Produkt:";
            zamowienia_dataGrid.Columns[1].Header = "Ilość:";
            zamowienia_dataGrid.Columns[2].Header = "Cena za m2:";
            zamowienia_dataGrid.Columns[3].Header = "Suma zł:";
            zamowienia_dataGrid.Columns[4].Header = " ";
            zamowienia_dataGrid.Columns[5].Header = "Sztuk:";
            zamowienia_dataGrid.Columns[6].Header = " ";
            zamowienia_dataGrid.Columns[4].Visibility = Visibility.Hidden;
            zamowienia_dataGrid.Columns[6].Visibility = Visibility.Hidden;

            if (status_zamowienia == 4)
            {
                DataRowView item = e.Row.Item as DataRowView;
                if (item != null)
                {
                    if (e.Row.GetIndex() % 2 == 0)
                    {
                        e.Row.Background = new SolidColorBrush(Colors.LightCyan);
                    }
                }
            }
            else
            {
                DataRowView item = e.Row.Item as DataRowView;
                if (item != null)
                {
                    DataRow row = item.Row;
                    var colValue = row[6];

                    if (Convert.ToInt16(colValue) == 1)
                    {
                        e.Row.Background = new SolidColorBrush(Colors.LightGreen);
                    }
                    if (Convert.ToInt16(colValue) == 0)
                    {
                        e.Row.Background = new SolidColorBrush(Colors.LightCyan);
                    }
                }
            }
        }
        /* -----------------------------------------------END_INICJALIZACJA FORMA----------------------------------------------------------*/

        /* -----------------------------------------------KALKULATOR----------------------------------------------------------*/
        private void Sumuj_BTN_Click(object sender, RoutedEventArgs e)
        {
                szerokosc_textBox.Text.Replace(".", ",");
                dlugosc_textBox.Text.Replace(".", ",");
                if (!string.IsNullOrEmpty(szerokosc_textBox.Text) && !string.IsNullOrEmpty(ilosc_sztuk_textBox.Text) && !string.IsNullOrEmpty(dlugosc_textBox.Text))
                {
                    if (Convert.ToDouble(szerokosc_textBox.Text.Replace(".", ",")) >= 0 && Convert.ToDouble(dlugosc_textBox.Text.Replace(".", ",")) >= 0)
                    {
                        sumowanie = sumowanie + (Convert.ToDouble(szerokosc_textBox.Text.Replace(".", ",")) * Convert.ToDouble(dlugosc_textBox.Text.Replace(".", ",")) * Convert.ToDouble(ilosc_sztuk_textBox.Text.Replace(".", ",")));
                    }

                    sztuki_zliczanie = sztuki_zliczanie + Convert.ToInt16(ilosc_sztuk_textBox.Text);
                    sztuki_textBox.Text = sztuki_zliczanie.ToString();
                    m2_suma_textBox.Text = (Math.Ceiling(sumowanie * 100) / 100).ToString();
                    szerokosc_textBox.Text = "1";
                    dlugosc_textBox.Text = "1";
                    ilosc_sztuk_textBox.Text = "1";
                }
        }

        private void OnCheckChanged(object sender, EventArgs e)
        {
            sztuki_textBox.Text = "klik";
        }

        private void sumuj_m2_BTN_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(m2_liczone_textBox.Text) && !string.IsNullOrEmpty(ilosc_sztuk_textBox.Text))
            {
                sumowanie = sumowanie + (Convert.ToDouble(m2_liczone_textBox.Text.Replace(".", ",")) * Convert.ToDouble(ilosc_sztuk_textBox.Text.Replace(".", ",")));
                m2_suma_textBox.Text = (Math.Ceiling(sumowanie*100)/100).ToString();
                sztuki_zliczanie = sztuki_zliczanie + Convert.ToInt16(ilosc_sztuk_textBox.Text);
                sztuki_textBox.Text = sztuki_zliczanie.ToString();
                m2_liczone_textBox.Text = "1";
                ilosc_sztuk_textBox.Text = "1";
            }
        }

        private void m2_suma_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!string.IsNullOrEmpty(cena_m2_textBox.Text))
            {
                cena_suma_textBox.Text = (Convert.ToDouble(m2_suma_textBox.Text) * Convert.ToDouble(cena_m2_textBox.Text.Replace(".", ","))).ToString("f2");
            }
        }

        private void cena_m2_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(cena_m2_textBox.Text))
            {
                cena_suma_textBox.Text = (Convert.ToDouble(m2_suma_textBox.Text) * Convert.ToDouble(cena_m2_textBox.Text.Replace(".", ","))).ToString("f2");
            }
            if (string.IsNullOrEmpty(cena_m2_textBox.Text))
            {
                cena_suma_textBox.Text = "0";
            }
        }

        private void Czysc_BTN_Click(object sender, RoutedEventArgs e)
        {
            m2_suma_textBox.Text = "0";
            sztuki_textBox.Text = "0";
            szerokosc_textBox.Text = "1";
            dlugosc_textBox.Text = "1";
            m2_liczone_textBox.Text = "1";
            ilosc_sztuk_textBox.Text = "1";
            sumowanie = 0;
            sztuki_zliczanie = 0;
        }

        private void sumowanie_zamowienia()
        {
            double cena = 0;
            for (int i = 0; i < zamowienie_DataTable.Rows.Count; i++)
            {
                cena = cena + Convert.ToDouble(zamowienie_DataTable.Rows[i]["Suma zł:"]);
            }

            cena_NETTO_textBox.Text = cena.ToString("f2");
            cena = cena * 1.23;
            cena_BRUTTO_textBox.Text = cena.ToString("f2");
        }
        /* -----------------------------------------------END_KALKULATOR----------------------------------------------------------*/

        /* -----------------------------------------------ZATWIERDZENIE_ZAMOWIENIA----------------------------------------------------------*/
        private void Zatwierdz_BTN_Click(object sender, RoutedEventArgs e)
        {
                string sql = "SELECT COUNT(*), id_klienci FROM klienci WHERE nazwa = '" + klient_textBox.Text + "'";
                _zmienna.dt = new DataTable();
                _zmienna.polaczenie.Open();    
                MySqlDataAdapter da = new MySqlDataAdapter(sql, _zmienna.polaczenie);
                da.Fill(_zmienna.dt);
                _zmienna.polaczenie.Close();
                if (Convert.ToDouble(_zmienna.dt.Rows[0][0]) == 0 && !string.IsNullOrEmpty(klient_textBox.Text))
                {
                    MessageBox.Show("Dodaj nowego klienta");
                }
                    else if (string.IsNullOrEmpty(klient_textBox.Text))
                    {
                        MessageBox.Show("Wprowadź nazwę klienta");
                    }
                        else if (zamowienie_DataTable.Rows.Count == 0)
                            {
                                MessageBox.Show("Wprowadź produkty");
                            }
                            else
                            {
                                string id_zamowienia;
                                MySqlCommand zapytanie;


                                if(utworzenie_edycja == true)
                                {
                                    sql = "SELECT MAX(id_zamowienia) FROM zamowienia";
                                    zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                                    _zmienna.polaczenie.Open();
                                    id_zamowienia = zapytanie.ExecuteScalar().ToString();
                                    _zmienna.polaczenie.Close();

                                    if (string.IsNullOrEmpty(id_zamowienia))
                                    {
                                        id_zamowienia = "0";
                                    }
                                    
                                    sql = "INSERT INTO zamowienia (id_zamowienia, data, id_klienci, id_status, id_uzytkownicy, id_faktury, id_zaplata, uwagi) values ('" + (Convert.ToInt32(id_zamowienia) + 1) + "', '"
                                        + data_textBox.Text + "', '" + _zmienna.dt.Rows[0][1] + "', '" + status_zaznaczenie + "', '" + Nazwa_uzytkownika + "', '" + faktura_zaznaczenie + "', '" + status_zaplata + "', '" + uwagi_textbox.Text + "')";
                                    zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                                    _zmienna.polaczenie.Open();
                                    zapytanie.ExecuteNonQuery();
                                    _zmienna.polaczenie.Close();

                                    for (int i = 0; i < zamowienie_DataTable.Rows.Count; i++)
                                    {
                                        sql = "INSERT INTO zamowienia_produkty (id_zamowienia_produkty, id_zamowienia, id_produkty, ilosc_m2, cena_m2, sztuki, stan_realizacji) values (NULL, '"
                                        + (Convert.ToInt32(id_zamowienia) + 1) + "', '" + zamowienie_DataTable.Rows[i][4] + "', '" + Convert.ToString(zamowienie_DataTable.Rows[i][1]).Replace(",", ".") + "', '" + Convert.ToString(zamowienie_DataTable.Rows[i][2]).Replace(",", ".") + "', '" + Convert.ToInt16(zamowienie_DataTable.Rows[i][5]) + "', '" + Convert.ToInt16(zamowienie_DataTable.Rows[i][6]) + "')";
                                        zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                                        _zmienna.polaczenie.Open();
                                        zapytanie.ExecuteNonQuery();
                                        _zmienna.polaczenie.Close();
                                    }
                                }
                                else if (utworzenie_edycja == false)
                                    {
                                        sql = "DELETE FROM zamowienia_produkty WHERE id_zamowienia = '" + _index + "'";
                                        zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                                        _zmienna.polaczenie.Open();
                                        zapytanie.ExecuteNonQuery();
                                        _zmienna.polaczenie.Close();

                                        sql = "UPDATE zamowienia SET data = '" + data_textBox.Text + "', id_klienci = '" + _zmienna.dt.Rows[0][1] + "', id_status = '" + status_zaznaczenie + "', id_faktury = '" + faktura_zaznaczenie + "', id_zaplata = '" + status_zaplata + "', uwagi = '" + uwagi_textbox.Text + "' WHERE id_zamowienia = '" + _index + "'";
                                        zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                                        _zmienna.polaczenie.Open();
                                        zapytanie.ExecuteNonQuery();
                                        _zmienna.polaczenie.Close();

                                        for (int i = 0; i < zamowienie_DataTable.Rows.Count; i++)
                                        {
                                            sql = "INSERT INTO zamowienia_produkty (id_zamowienia_produkty, id_zamowienia, id_produkty, ilosc_m2, cena_m2, sztuki, stan_realizacji) values (NULL, '"
                                            + (Convert.ToInt32(_index)) + "', '" + zamowienie_DataTable.Rows[i][4] + "', '" + Convert.ToString(zamowienie_DataTable.Rows[i][1]).Replace(",", ".") + "', '" + Convert.ToString(zamowienie_DataTable.Rows[i][2]).Replace(",", ".") + "', '" + Convert.ToInt16(zamowienie_DataTable.Rows[i][5]) + "', '" + Convert.ToInt16(zamowienie_DataTable.Rows[i][6]) + "')";
                                            zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                                            _zmienna.polaczenie.Open();
                                            zapytanie.ExecuteNonQuery();
                                            _zmienna.polaczenie.Close();
                                        }
                                    }
                                    else
                                    {
                                        return;
                                    }


                                odswiez.zamowienia_odswiezanie();
                                _zmienna.Row = -1;
                                odswiez.grid_zamowienia.SelectedIndex = -1;
                                odswiez.Zgoda_na_odswiezenie_zamowienia = true;
                                this.Close();
                            }
                        
        }
        /* -----------------------------------------------END_ZATWIERDZENIE_ZAMOWIENIA----------------------------------------------------------*/

        /* -----------------------------------------------ANULOWANIE_ZAMOWIENIA----------------------------------------------------------*/
        private void Anuluj_BTN_Click(object sender, RoutedEventArgs e)
        {
            data_textBox.Clear();
            klient_textBox.Clear();
            produkt_textBox.Clear();
            szerokosc_textBox.Text = "0";
            dlugosc_textBox.Text = "0";
            m2_liczone_textBox.Text = "1";
            ilosc_sztuk_textBox.Text = "1";
            cena_m2_textBox.Clear();
            m2_suma_textBox.Text = "0";
            sztuki_textBox.Text = "0";
            cena_NETTO_textBox.Text = "0";
            cena_BRUTTO_textBox.Text = "0";
            this.Close();
        }
        /* -----------------------------------------------END_ANULOWANIE_ZAMOWIENIA----------------------------------------------------------*/

        /* -----------------------------------------------KALENDARZ----------------------------------------------------------*/

        private void Zmien_calendarz_Click(object sender, RoutedEventArgs e)
        {
            if (calendar1.Visibility == Visibility.Visible)
            {
                calendar1.Visibility = Visibility.Hidden;
            }
            else if (calendar1.Visibility == Visibility.Hidden)
            {
                calendar1.Visibility = Visibility.Visible;
            }
        }

        private void calendar1_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            data_textBox.Text = calendar1.SelectedDate.Value.ToString("yyyy-MM-dd");
            calendar1.Visibility = Visibility.Hidden;
        }

        /* -----------------------------------------------END_KALENDARZ----------------------------------------------------------*/

        /* -----------------------------------------------WYBÓR KLIENTA----------------------------------------------------------*/
        private void klient_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (klient_textBox.Text.Equals(""))
            {
                wyszukaj_klienta.Items.Clear();
                wyszukaj_klienta.IsDropDownOpen = false;
            }
            else
            {
                _zmienna.polaczenie.Open();
                string sql = "SELECT * FROM klienci WHERE nazwa LIKE '%" + klient_textBox.Text + "%' ORDER BY nazwa";
                
                _zmienna.dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, _zmienna.polaczenie);
                da.Fill(_zmienna.dt);
                _zmienna.polaczenie.Close();

                for (int i = 0; i < _zmienna.dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        wyszukaj_klienta.Items.Clear();
                        wyszukaj_klienta.IsDropDownOpen = true;
                        wyszukaj_klienta.SelectedIndex = -1;
                    }

                    wyszukaj_klienta.Items.Add(_zmienna.dt.Rows[i]["nazwa"].ToString());

                    if (i > 4)
                    {
                        return;
                    }
                }

                if (_zmienna.dt.Rows.Count == 0)
                {
                    wyszukaj_klienta.Items.Clear();
                    wyszukaj_klienta.IsDropDownOpen = false;
                }
            }
        }


        private void wyszukaj_klienta_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            zmiana_klient = true;
        }

        private void wyszukaj_klienta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (zmiana_klient == true)
            {
                klient_textBox.Text = wyszukaj_klienta.Text;
                zmiana_klient = false;
            }
        }

        private void dodaj_klienta_Click(object sender, RoutedEventArgs e)
        {
            string sprawdzenie;
            string sql = "SELECT COUNT(*) FROM klienci WHERE nazwa = '" + klient_textBox.Text + "'";
            MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
            _zmienna.polaczenie.Open();
            sprawdzenie = zapytanie.ExecuteScalar().ToString();
            _zmienna.polaczenie.Close();

            if (sprawdzenie == "0" && !string.IsNullOrEmpty(klient_textBox.Text))
            {
                _zmienna.polaczenie.Open();
                sql = "INSERT INTO klienci (id_klienci, nazwa) values (NULL, '" + klient_textBox.Text + "')";
                zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                zapytanie.ExecuteNonQuery();
                _zmienna.polaczenie.Close();
                MessageBox.Show("Dodano nowego klienta");
            }
        }

        /* -----------------------------------------------END_WYBÓR_KLIENTA----------------------------------------------------------*/

        /* -----------------------------------------------WYBÓR klienta i produktu - KLAWISZE----------------------------------------------------------*/

        private void Wprowadz_Zamowienie_Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down && wyszukaj_klienta.IsDropDownOpen == true && wyszukaj_klienta.SelectedIndex <= 5)
            {
                wyszukaj_klienta.SelectedIndex = wyszukaj_klienta.SelectedIndex + 1;
            }
            if (e.Key == Key.Up && wyszukaj_klienta.IsDropDownOpen == true && wyszukaj_klienta.SelectedIndex > 0)
            {
                wyszukaj_klienta.SelectedIndex = wyszukaj_klienta.SelectedIndex - 1;
            }
            if (e.Key == Key.Enter && wyszukaj_klienta.IsDropDownOpen == true)
            {
                klient_textBox.Text = wyszukaj_klienta.Text;
                wyszukaj_klienta.IsDropDownOpen = false;
            }

            if (e.Key == Key.Down && wyszukaj_produkt.IsDropDownOpen == true && wyszukaj_produkt.SelectedIndex <= 5)
            {
                wyszukaj_produkt.SelectedIndex = wyszukaj_produkt.SelectedIndex + 1;
            }
            if (e.Key == Key.Up && wyszukaj_produkt.IsDropDownOpen == true && wyszukaj_produkt.SelectedIndex > 0)
            {
                wyszukaj_produkt.SelectedIndex = wyszukaj_produkt.SelectedIndex - 1;
            }
            if (e.Key == Key.Enter && wyszukaj_produkt.IsDropDownOpen == true)
            {
                produkt_textBox.Text = wyszukaj_produkt.Text;
                wyszukaj_produkt.IsDropDownOpen = false;
                eko_RadioBTN.Visibility = Visibility.Visible;
                best_RadioBTN.Visibility = Visibility.Visible;
                detal_RadioBTN.Visibility = Visibility.Visible;
                for (int i = 0; i < _zmienna.dt.Rows.Count; i++)
                {
                    if (_zmienna.dt.Rows[i]["nazwa"].ToString() == produkt_textBox.Text)
                    {
                        cena_m2_textBox.Text = _zmienna.dt.Rows[i]["cena_detaliczna"].ToString();
                        iteracja_cena = i;
                        detal_RadioBTN.IsChecked = true;
                        return;
                    }
                }
            }
        }

        /* -----------------------------------------------END_WYBÓR klienta i produktu - KLAWISZE----------------------------------------------------------*/

        /* -----------------------------------------------WYBÓR PRODUKTU----------------------------------------------------------*/

        private void produkt_textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            eko_RadioBTN.Visibility = Visibility.Hidden;
            best_RadioBTN.Visibility = Visibility.Hidden;
            detal_RadioBTN.Visibility = Visibility.Hidden;
            cena_m2_textBox.Clear();
            

            if (produkt_textBox.Text.Equals(""))
            {
                wyszukaj_produkt.Items.Clear();
                wyszukaj_produkt.IsDropDownOpen = false;
            }
            else
            {
                _zmienna.polaczenie.Open();
                string sql = "SELECT * FROM produkty WHERE nazwa LIKE '%" + produkt_textBox.Text + "%' ORDER BY nazwa";
                _zmienna.dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, _zmienna.polaczenie);
                da.Fill(_zmienna.dt);
                _zmienna.polaczenie.Close();

                for (int i = 0; i < _zmienna.dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        wyszukaj_produkt.Items.Clear();
                        wyszukaj_produkt.IsDropDownOpen = true;
                        wyszukaj_produkt.SelectedIndex = -1;
                    }

                    wyszukaj_produkt.Items.Add(_zmienna.dt.Rows[i]["nazwa"].ToString());

                    if (i > 4)
                    {
                        return;
                    }
                }

                if (_zmienna.dt.Rows.Count == 0)
                {
                    wyszukaj_produkt.Items.Clear();
                    wyszukaj_produkt.IsDropDownOpen = false;
                }
            }
        }

        private void wyszukaj_produkt_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            zmiana_produkt = true;
        }

        private void wyszukaj_produkt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (zmiana_produkt == true)
            {
                produkt_textBox.Text = wyszukaj_produkt.Text;
                zmiana_produkt = false;
                eko_RadioBTN.Visibility = Visibility.Visible;
                best_RadioBTN.Visibility = Visibility.Visible;
                detal_RadioBTN.Visibility = Visibility.Visible;
                for (int i = 0; i < _zmienna.dt.Rows.Count; i++)
                {
                    if (_zmienna.dt.Rows[i]["nazwa"].ToString() == produkt_textBox.Text)
                    {
                        cena_m2_textBox.Text = _zmienna.dt.Rows[i]["cena_detaliczna"].ToString();
                        iteracja_cena = i;
                        detal_RadioBTN.IsChecked = true;
                        return;
                    }
                }
            }
        }

        private void dodaj_produkt_Click(object sender, RoutedEventArgs e)
        {
            string sprawdzenie;
            string sql = "SELECT COUNT(*) FROM produkty WHERE nazwa = '" + produkt_textBox.Text + "'";
            MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
            _zmienna.polaczenie.Open();
            sprawdzenie = zapytanie.ExecuteScalar().ToString();
            _zmienna.polaczenie.Close();

            if (sprawdzenie == "0" && !string.IsNullOrEmpty(produkt_textBox.Text))
            {
                _zmienna.polaczenie.Open();
                sql = "INSERT INTO produkty (id_produkty, nazwa) values (NULL, '" + produkt_textBox.Text + "')";
                zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                zapytanie.ExecuteNonQuery();
                _zmienna.polaczenie.Close();
                MessageBox.Show("Dodano nowy produkt");
                if (!string.IsNullOrEmpty(cena_m2_textBox.Text))
                {
                    _zmienna.polaczenie.Open();
                    sql = "UPDATE produkty SET cena_agencyjna_EKO = " + cena_m2_textBox.Text.Replace(",", ".") + " WHERE nazwa = '" + produkt_textBox.Text + "'";
                    zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    zapytanie.ExecuteNonQuery();
                    _zmienna.polaczenie.Close();
                }
            }
        }

        private void eko_RadioBTN_Checked(object sender, RoutedEventArgs e)
        {
            cena_m2_textBox.Text = _zmienna.dt.Rows[iteracja_cena]["cena_agencyjna_EKO"].ToString();
        }

        private void best_RadioBTN_Checked(object sender, RoutedEventArgs e)
        {
            cena_m2_textBox.Text = _zmienna.dt.Rows[iteracja_cena]["cena_agencyjna_BEST"].ToString();
        }

        private void detal_RadioBTN_Checked(object sender, RoutedEventArgs e)
        {
            cena_m2_textBox.Text = _zmienna.dt.Rows[iteracja_cena]["cena_detaliczna"].ToString();
        }

        /* -----------------------------------------------END_WYBÓR PRODUKTU----------------------------------------------------------*/


        /* -----------------------------------------------WPROWADZANIE PRODUKTU ZAMAWIANEGO----------------------------------------------------------*/
        private void sumuj_zamowienia_BTN_Click(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT COUNT(*), id_produkty FROM produkty WHERE nazwa = '" + produkt_textBox.Text + "'";
            DataTable licznik = new DataTable();
            _zmienna.polaczenie.Open();
            MySqlDataAdapter da = new MySqlDataAdapter(sql, _zmienna.polaczenie);
            da.Fill(licznik);
            _zmienna.polaczenie.Close();

            if (Convert.ToDouble(licznik.Rows[0][0]) == 0 && !string.IsNullOrEmpty(produkt_textBox.Text))
            {
                MessageBox.Show("Dodaj nowy produkt");
            }
            else if(string.IsNullOrEmpty(produkt_textBox.Text))
            {
                MessageBox.Show("Podaj nazwę produktu");
            }
            else
            {
                if(string.IsNullOrEmpty(cena_m2_textBox.Text))
                {
                    cena_m2_textBox.Text = "0";
                }
                cena_m2_textBox.Text = String.Format("{0:0.00}", Convert.ToDouble(cena_m2_textBox.Text.Replace(".", ",")));
                if (zamowienia_dataGrid.IsEnabled == false)
                {
                    zamowienie_DataTable.Rows[zamowienia_dataGrid.SelectedIndex][0] = produkt_textBox.Text;
                    zamowienie_DataTable.Rows[zamowienia_dataGrid.SelectedIndex][1] = Convert.ToDouble(m2_suma_textBox.Text.Replace(".", ","));
                    zamowienie_DataTable.Rows[zamowienia_dataGrid.SelectedIndex][2] = Convert.ToDouble(cena_m2_textBox.Text.Replace(".", ","));
                    zamowienie_DataTable.Rows[zamowienia_dataGrid.SelectedIndex][3] = Math.Round((Convert.ToDouble(m2_suma_textBox.Text.Replace(".", ",")) * Convert.ToDouble(cena_m2_textBox.Text.Replace(".", ","))), 2);
                    zamowienie_DataTable.Rows[zamowienia_dataGrid.SelectedIndex][4] = licznik.Rows[0][1];
                    zamowienie_DataTable.Rows[zamowienia_dataGrid.SelectedIndex][5] = Convert.ToInt16(sztuki_textBox.Text);
                    zamowienia_dataGrid.IsEnabled = true;
                }
                else
                {
                    zamowienie_DataTable.Rows.Add(produkt_textBox.Text, Convert.ToDouble(m2_suma_textBox.Text.Replace(".", ",")), Convert.ToDouble(cena_m2_textBox.Text.Replace(".", ",")), Math.Round((Convert.ToDouble(m2_suma_textBox.Text.Replace(".", ",")) * Convert.ToDouble(cena_m2_textBox.Text.Replace(".", ","))), 2), licznik.Rows[0][1], Convert.ToInt32(sztuki_textBox.Text), 0);
                }
                
                zamowienia_dataGrid.DataContext = zamowienie_DataTable;
                zamowienia_dataGrid.Columns[4].Visibility = Visibility.Hidden;
                zamowienia_dataGrid.Columns[6].Visibility = Visibility.Hidden;

                sumowanie_zamowienia();

                produkt_textBox.Clear();
                szerokosc_textBox.Text = "1";
                dlugosc_textBox.Text = "1";
                ilosc_sztuk_textBox.Text = "1";
                m2_liczone_textBox.Text = "1";
                m2_suma_textBox.Text = "0";
                sztuki_textBox.Text = "0";
                cena_suma_textBox.Text = "0";
                sumowanie = 0;
                sztuki_zliczanie = 0;
            }
        }
        /* -----------------------------------------------END_WPROWADZANIE PRODUKTU ZAMAWIANEGO------------------------------------------------------*/

        /* -----------------------------------------------USUWANIE PRODUKTU ZAMAWIANEGO------------------------------------------------------*/
        private void usun_zamowienie_BTN_Click(object sender, RoutedEventArgs e)
        {
            if (zamowienia_dataGrid.SelectedIndex == -1)
            {
                return;
            }
            else
            {

                zamowienie_DataTable.Rows[zamowienia_dataGrid.SelectedIndex].Delete();

                double cena = 0;
                for (int i = 0; i < zamowienie_DataTable.Rows.Count; i++)
                {
                    cena = cena + Convert.ToDouble(zamowienie_DataTable.Rows[i]["Suma zł:"]);
                }

                if (zamowienia_dataGrid.IsEnabled == false)
                {
                    zamowienia_dataGrid.IsEnabled = true;
                }

                zamowienia_dataGrid.DataContext = zamowienie_DataTable;
                zamowienia_dataGrid.Items.Refresh();
                zamowienia_dataGrid.Columns[4].Visibility = Visibility.Hidden;
                zamowienia_dataGrid.Columns[6].Visibility = Visibility.Hidden;
                cena_NETTO_textBox.Text = cena.ToString("f2");
                cena = cena * 1.23;
                    cena_BRUTTO_textBox.Text = cena.ToString("f2");
                zamowienia_dataGrid.SelectedIndex = -1;
                produkt_textBox.Clear();
                sumowanie = 0;
                szerokosc_textBox.Text = "1";
                dlugosc_textBox.Text = "1";
                ilosc_sztuk_textBox.Text = "1";
                m2_liczone_textBox.Text = "1";
                m2_suma_textBox.Text = "0";
                sztuki_textBox.Text = "0";
                cena_suma_textBox.Text = "0";
                sztuki_zliczanie = 0;
            }
        }
        /* -----------------------------------------------END_USUWANIE PRODUKTU ZAMAWIANEGO------------------------------------------------------*/

        /* -----------------------------------------------TextBox, wprowadzenie tylko cyfr------------------------------------------------------*/
        private void szerokosc_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            tylko_liczby(sender, e);
        }

        private void dlugosc_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            tylko_liczby(sender, e);
        }

        private void ilosc_sztuk_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            tylko_liczby(sender, e);
        }

        private void m2_liczone_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            tylko_liczby(sender, e);
        }

        private void cena_m2_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            tylko_liczby(sender, e);
        }

        private void tylko_liczby(object sender, TextCompositionEventArgs e)
        {
            //allows only number
            if (!char.IsNumber(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }

            if (((e.Text).ToCharArray()[e.Text.Length - 1] == '.') || ((e.Text).ToCharArray()[e.Text.Length - 1] == ','))
            {
                e.Handled = true;
                if (!(((TextBox)sender).Text.Contains('.')))
                {
                    if (((TextBox)sender).Text.Length == 0) { ((TextBox)sender).Text = "0."; ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length; }
                    else { ((TextBox)sender).Text += "."; ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length; }
                }
            }
        }

        /* -----------------------------------------------END_TextBox, wprowadzenie tylko cyfr------------------------------------------------------*/


        /* -----------------------------------------------Nowe zamowienie / Edycja------------------------------------------------------*/
        private string _theValue;
        public string TheValue
        {
            get
            {
                return _theValue;
            }
            set
            {
                _theValue = value;
            }
        }

        private int _index;
        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
            }
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

        private void Wprowadz_Zamowienie_Window_Loaded(object sender, RoutedEventArgs e)
        {

            string mojePolaczenie = "SERVER= 21615.m.tld.pl;" + "DATABASE= baza_1031;" + "UID= a_1031;" + "PASSWORD= 4SgrTvf^WF;";
            _zmienna.polaczenie = new MySqlConnection(mojePolaczenie);

            utworzenie_edycja = Convert.ToBoolean(_theValue);

            if (utworzenie_edycja == true)
            {
                this.Title = "Nowe zamówienia";
            }
            else if (utworzenie_edycja == false)
            {
                this.Title = "Edycja zamówienia";

                _zmienna.polaczenie.Open();
                string sql = "SELECT DATE_FORMAT(data,'%Y-%m-%d') as data, nazwa, id_status, id_faktury, id_zaplata, uwagi from zamowienia, klienci where zamowienia.id_zamowienia = '" + _index + "' AND zamowienia.id_klienci = klienci.id_klienci ORDER BY data DESC";
                DataTable zamowienie_edit = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, _zmienna.polaczenie);
                da.Fill(zamowienie_edit);
                _zmienna.polaczenie.Close();
                data_textBox.Text = zamowienie_edit.Rows[0].ItemArray[0].ToString();
                klient_textBox.Text = zamowienie_edit.Rows[0].ItemArray[1].ToString();
                status_zaznaczenie = Convert.ToInt16(zamowienie_edit.Rows[0].ItemArray[2]);
                status_zamowienia = status_zaznaczenie;
                status_comboBox.SelectedIndex = status_zaznaczenie - 1;
                faktura_zaznaczenie = Convert.ToInt16(zamowienie_edit.Rows[0].ItemArray[3]);
                faktura_comboBox.SelectedIndex = faktura_zaznaczenie - 1;
                status_zaplata = Convert.ToInt16(zamowienie_edit.Rows[0].ItemArray[4]);
                zaplata_comboBox.SelectedIndex = status_zaplata - 1;
                wyszukaj_klienta.IsDropDownOpen = false;
                //zamowienia_dataGrid.DataContext = zamowienie_edit;
                uwagi_textbox.Text = zamowienie_edit.Rows[0].ItemArray[5].ToString();

                sql = "SELECT nazwa, ilosc_m2,  cena_m2, produkty.id_produkty, sztuki, stan_realizacji from produkty, zamowienia_produkty where zamowienia_produkty.id_zamowienia = '" + _index + "' AND zamowienia_produkty.id_produkty = produkty.id_produkty  ORDER BY nazwa";
                _zmienna.dt = new DataTable();
                da = new MySqlDataAdapter(sql, _zmienna.polaczenie);
                da.Fill(_zmienna.dt);
                _zmienna.polaczenie.Close();

                for (int i = 0; i < _zmienna.dt.Rows.Count; i++)
                {
                    zamowienie_DataTable.Rows.Add(_zmienna.dt.Rows[i].ItemArray[0], (Math.Ceiling(Convert.ToDouble(_zmienna.dt.Rows[i].ItemArray[1]) * 100) / 100), _zmienna.dt.Rows[i].ItemArray[2], Math.Round((Convert.ToDouble((Math.Ceiling(Convert.ToDouble(_zmienna.dt.Rows[i].ItemArray[1]) * 100) / 100)) * Convert.ToDouble(_zmienna.dt.Rows[i].ItemArray[2])), 2), _zmienna.dt.Rows[i].ItemArray[3], _zmienna.dt.Rows[i].ItemArray[4], Convert.ToInt16(_zmienna.dt.Rows[i].ItemArray[5]));
                }
                sumowanie_zamowienia();

                zamowienia_dataGrid.DataContext = zamowienie_DataTable;
            }
        }
        /* -----------------------------------------------END Nowe zamowienie / Edycja------------------------------------------------------*/

        /* -----------------------------------------------Wyświetlanie edycji wiersza z DataGrid------------------------------------------------------*/
        private void zamowienia_dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (zamowienia_dataGrid.SelectedIndex != -1)
            {
                produkt_textBox.Text = zamowienie_DataTable.Rows[zamowienia_dataGrid.SelectedIndex].ItemArray[0].ToString();
                wyszukaj_produkt.IsDropDownOpen = false;
                sumowanie = Convert.ToDouble(zamowienie_DataTable.Rows[zamowienia_dataGrid.SelectedIndex].ItemArray[1]);
                m2_suma_textBox.Text = zamowienie_DataTable.Rows[zamowienia_dataGrid.SelectedIndex].ItemArray[1].ToString();
                cena_m2_textBox.Text = zamowienie_DataTable.Rows[zamowienia_dataGrid.SelectedIndex].ItemArray[2].ToString();
                sztuki_textBox.Text = zamowienie_DataTable.Rows[zamowienia_dataGrid.SelectedIndex].ItemArray[5].ToString();
                sztuki_zliczanie = Convert.ToInt16(sztuki_textBox.Text);
                zamowienia_dataGrid.IsEnabled = false;
            }
        }
        /* -----------------------------------------------END_Wyświetlanie edycji wiersza z DataGrid------------------------------------------------------*/

        /* -----------------------------------------------Wyświetlanie edycji wiersza z DataGrid------------------------------------------------------*/
        private void zamowienia_dataGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (status_zamowienia != 4)
            {
                if (zamowienia_dataGrid.SelectedIndex != -1)
                {
                    if (Convert.ToInt16(zamowienie_DataTable.Rows[zamowienia_dataGrid.SelectedIndex][6]) == 0)
                    {
                        zamowienie_DataTable.Rows[zamowienia_dataGrid.SelectedIndex][6] = 1;
                    }
                        else if (Convert.ToInt16(zamowienie_DataTable.Rows[zamowienia_dataGrid.SelectedIndex][6]) == 1)
                        {
                            zamowienie_DataTable.Rows[zamowienia_dataGrid.SelectedIndex][6] = 0;
                        }
                }
                zamowienia_dataGrid.DataContext = zamowienie_DataTable;
                zamowienia_dataGrid.Items.Refresh();
                zamowienia_dataGrid.Columns[4].Visibility = Visibility.Hidden;
                zamowienia_dataGrid.Columns[6].Visibility = Visibility.Hidden;
            }
        }
        /* -----------------------------------------------END_Wyświetlanie edycji wiersza z DataGrid------------------------------------------------------*/

        /* -----------------------------------------------ComboBox------------------------------------------------------*/
        private void status_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            status_zaznaczenie = (Convert.ToInt16(status_comboBox.SelectedIndex) + 1);
        }

        private void faktura_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            faktura_zaznaczenie = (Convert.ToInt16(faktura_comboBox.SelectedIndex) + 1);
        }

        private void zaplata_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            status_zaplata = (Convert.ToInt16(zaplata_comboBox.SelectedIndex) + 1);
        }
        /* -----------------------------------------------END_ComboBox------------------------------------------------------*/


        /*----------------------------------------------------------- Akcja dla naciśniecia klawisza TAB ---------------------------------------------*/
        private void szerokosc_textBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            dlugosc_textBox.SelectAll();
        }

        private void cena_m2_textBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            szerokosc_textBox.SelectAll();
        }

        private void dlugosc_textBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ilosc_sztuk_textBox.SelectAll();
        }
        /*-----------------------------------------------------------END Akcja dla naciśniecia klawisza TAB ---------------------------------------------*/

        Zmienne _zmienna = new Zmienne();
        MainWindow odswiez = new MainWindow();
        DataTable zamowienie_DataTable = new DataTable();
        private double sumowanie;
        private bool zmiana_klient = false;
        private bool zmiana_produkt = false;
        public bool utworzenie_edycja;
        private int iteracja_cena = 0;
        private int status_zaznaczenie = 1;
        private int faktura_zaznaczenie = 1;
        private int status_zaplata = 1;
        private int sztuki_zliczanie = 0;
        private int status_zamowienia = 1;
    }
}
