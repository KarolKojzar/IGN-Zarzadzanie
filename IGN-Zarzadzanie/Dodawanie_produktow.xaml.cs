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
using System.Text.RegularExpressions;

namespace IGN_Zarzadzanie
{
    /// <summary>
    /// Interaction logic for Dodawanie_produktow.xaml
    /// </summary>
    public partial class Dodawanie_Produktow : Window
    {
        public Dodawanie_Produktow()
        {
            InitializeComponent();
            odswiez.IGN.IsEnabled = false;
            //string mojePolaczenie = "SERVER= xxx;" + "DATABASE= xxx;" + "UID= xxx;" + "PASSWORD= xxx;";
        }

        private void Zatwierdz_BTN_Click(object sender, RoutedEventArgs e)
        {
            string sprawdzenie;
            string sql = "SELECT COUNT(*) FROM produkty WHERE nazwa = '" + wprowadz_material_textBox.Text + "'";
            MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
            _zmienna.polaczenie.Open();
            sprawdzenie = zapytanie.ExecuteScalar().ToString();
            _zmienna.polaczenie.Close();

            if (sprawdzenie == "0" && !string.IsNullOrEmpty(wprowadz_material_textBox.Text))
            {
                sql = "INSERT INTO produkty (id_produkty, nazwa) values (NULL, '" + wprowadz_material_textBox.Text + "')";
                zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                _zmienna.polaczenie.Open();
                zapytanie.ExecuteNonQuery();
                _zmienna.polaczenie.Close();
                int id;
                sql = "SELECT id_produkty FROM produkty WHERE nazwa = '" + wprowadz_material_textBox.Text + "'";
                zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                _zmienna.polaczenie.Open();
                id = (int)zapytanie.ExecuteScalar();
                _zmienna.polaczenie.Close();


                if(!string.IsNullOrEmpty(wprowadz_cena_detal_textBox.Text))
                {
                    sql = "UPDATE produkty SET cena_detaliczna = " + wprowadz_cena_detal_textBox.Text.Replace(",", ".") + " WHERE id_produkty = '" + id + "'";
                    zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    zapytanie.ExecuteNonQuery();
                    _zmienna.polaczenie.Close();
                }
                if(!string.IsNullOrEmpty(wprowadz_cena_agencyjna_EKO_textBox.Text))
                {
                    sql = "UPDATE produkty SET cena_agencyjna_EKO = " + wprowadz_cena_agencyjna_EKO_textBox.Text.Replace(",", ".") + " WHERE id_produkty = '" + id + "'";
                    zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    zapytanie.ExecuteNonQuery();
                    _zmienna.polaczenie.Close();
                }
                if(!string.IsNullOrEmpty(wprowadz_cena_agencyjna_BEST_textBox.Text))
                {
                    sql = "UPDATE produkty SET cena_agencyjna_BEST = " + wprowadz_cena_agencyjna_BEST_textBox.Text.Replace(",", ".") + " WHERE id_produkty = '" + id + "'";
                    zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    zapytanie.ExecuteNonQuery();
                    _zmienna.polaczenie.Close();
                }

                odswiez.materialy_odswiezanie();
                this.Close();
            }
            else if(string.IsNullOrEmpty(wprowadz_material_textBox.Text))
            {
                MessageBox.Show("Wprowadź nazwę");
            }
            else
            {
                MessageBox.Show("Podany materiał już istnieje");
            }
            odswiez.materialy_odswiezanie();
            wprowadz_material_textBox.Clear();
            wprowadz_cena_detal_textBox.Clear();
            wprowadz_cena_agencyjna_EKO_textBox.Clear();
            wprowadz_cena_agencyjna_BEST_textBox.Clear();
            odswiez.Zgoda_na_odswiezenie_material = true;
        }

        private void Anuluj_BTN_Click(object sender, RoutedEventArgs e)
        {
            wprowadz_material_textBox.Clear();
            wprowadz_cena_detal_textBox.Clear();
            wprowadz_cena_agencyjna_EKO_textBox.Clear();
            wprowadz_cena_agencyjna_BEST_textBox.Clear();
            odswiez.IGN.IsEnabled = true;
            this.Close();
        }

        private void wprowadz_cena_agencyjna_EKO_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            tylko_liczby(sender, e);
        }

        private void wprowadz_cena_agencyjna_BEST_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            tylko_liczby(sender, e);
        }

        private void wprowadz_cena_detal_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void Dodawanie_produktow_Window_Loaded(object sender, RoutedEventArgs e)
        {
            string mojePolaczenie = "SERVER= xxx;" + "DATABASE= xxx;" + "UID= xxx;" + "PASSWORD= xxx;";
            _zmienna.polaczenie = new MySqlConnection(mojePolaczenie);
        }

        Zmienne _zmienna = new Zmienne();
        MainWindow odswiez = new MainWindow();
    }
}
