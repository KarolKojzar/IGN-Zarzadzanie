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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Edycja_Produktow : Window
    {
        public Edycja_Produktow()
        {
            InitializeComponent();
            _zmiennaWindow.IGN.IsEnabled = false;
        }

        private void Zatwierdz_BTN_Click(object sender, RoutedEventArgs e)
        {
            string sprawdzenie = "0";
            string sql;
            MySqlCommand zapytanie;
            if (poprzednia_Nazwa != edytuj_material_textBox.Text)
            {
                sql = "SELECT COUNT(*) FROM produkty WHERE nazwa = '" + edytuj_material_textBox.Text + "'";
                zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                _zmienna.polaczenie.Open();
                sprawdzenie = zapytanie.ExecuteScalar().ToString();
                _zmienna.polaczenie.Close();
            }

            if (sprawdzenie == "0" && !string.IsNullOrEmpty(edytuj_material_textBox.Text))
            {
                sql = "UPDATE produkty SET nazwa = '" + edytuj_material_textBox.Text + "' WHERE id_produkty = '" + _zmienna.Row + "'";
                zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                _zmienna.polaczenie.Open();
                zapytanie.ExecuteNonQuery();
                _zmienna.polaczenie.Close();

                if (!string.IsNullOrEmpty(edytuj_cena_detal_textBox.Text))
                {
                    sql = "UPDATE produkty SET cena_detaliczna = " + edytuj_cena_detal_textBox.Text.Replace(",", ".") + " WHERE id_produkty = '" + _zmienna.Row + "'";
                    zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    zapytanie.ExecuteNonQuery();
                    _zmienna.polaczenie.Close();
                }
                if (!string.IsNullOrEmpty(edytuj_cena_agencyjna_EKO_textBox.Text))
                {
                    sql = "UPDATE produkty SET cena_agencyjna_EKO = " + edytuj_cena_agencyjna_EKO_textBox.Text.Replace(",", ".") + " WHERE id_produkty = '" + _zmienna.Row + "'";
                    zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    zapytanie.ExecuteNonQuery();
                    _zmienna.polaczenie.Close();
                }
                if (!string.IsNullOrEmpty(edytuj_cena_agencyjna_BEST_textBox.Text))
                {
                    sql = "UPDATE produkty SET cena_agencyjna_BEST = " + edytuj_cena_agencyjna_BEST_textBox.Text.Replace(",", ".") + " WHERE id_produkty = '" + _zmienna.Row + "'";
                    zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    zapytanie.ExecuteNonQuery();
                    _zmienna.polaczenie.Close();
                }
                if (string.IsNullOrEmpty(edytuj_cena_detal_textBox.Text))
                {
                    sql = "UPDATE produkty SET cena_detaliczna = NULL WHERE id_produkty = '" + _zmienna.Row + "'";
                    zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    zapytanie.ExecuteNonQuery();
                    _zmienna.polaczenie.Close();
                }
                if (string.IsNullOrEmpty(edytuj_cena_agencyjna_EKO_textBox.Text))
                {
                    sql = "UPDATE produkty SET cena_agencyjna_EKO = NULL  WHERE id_produkty = '" + _zmienna.Row + "'";
                    zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    zapytanie.ExecuteNonQuery();
                    _zmienna.polaczenie.Close();
                }
                if (string.IsNullOrEmpty(edytuj_cena_agencyjna_BEST_textBox.Text))
                {
                    sql = "UPDATE produkty SET cena_agencyjna_BEST = NULL WHERE id_produkty = '" + _zmienna.Row + "'";
                    zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);
                    _zmienna.polaczenie.Open();
                    zapytanie.ExecuteNonQuery();
                    _zmienna.polaczenie.Close();
                }

                _zmiennaWindow.IGN.IsEnabled = true;
                this.Close();
            }
            else if (string.IsNullOrEmpty(edytuj_material_textBox.Text))
            {
                MessageBox.Show("Wprowadź nazwę");
            }
            else
            {
                MessageBox.Show("Podany materiał już istnieje");
            }

            _zmiennaWindow.materialy_odswiezanie();
            _zmiennaWindow.Zgoda_na_odswiezenie_material = true;
        }

        public void edytowanie(int e)
        {

            InitializeComponent();
            string mojePolaczenie = "SERVER= xxx;" + "DATABASE= xxx;" + "UID= xxx;" + "PASSWORD= xxx;";
            _zmienna.polaczenie = new MySqlConnection(mojePolaczenie);
            _zmienna.Row = e;
            _zmienna.polaczenie.Open();
            string sql = "SELECT * FROM produkty WHERE id_produkty = '" + _zmienna.Row + "'";
            MySqlCommand zapytanie = new MySqlCommand(sql, _zmienna.polaczenie);

            using (MySqlDataReader reader = zapytanie.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (!reader.IsDBNull(1))
                    edytuj_material_textBox.Text = reader.GetString(1);
                    if (!reader.IsDBNull(3))
                    edytuj_cena_agencyjna_BEST_textBox.Text = reader.GetString(3);
                    if (!reader.IsDBNull(2))
                    edytuj_cena_agencyjna_EKO_textBox.Text = reader.GetString(2);
                    if (!reader.IsDBNull(4))
                    edytuj_cena_detal_textBox.Text = reader.GetString(4);
                }
            }
            _zmienna.polaczenie.Close();
            poprzednia_Nazwa = edytuj_material_textBox.Text;
            
            this.Show();
        }

        private void edytuj_cena_agencyjna_EKO_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            tylko_liczby(sender, e);
        }

        private void edytuj_cena_agencyjna_BEST_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            tylko_liczby(sender, e);
        }

        private void edytuj_cena_detal_textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
                if (!(((TextBox)sender).Text.Contains('.')) && !(((TextBox)sender).Text.Contains(',')))
                {
                    if (((TextBox)sender).Text.Length == 0) 
                    { 
                        ((TextBox)sender).Text = "0."; 
                        ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length; 
                    }
                    else { 
                        ((TextBox)sender).Text += "."; 
                        ((TextBox)sender).CaretIndex = ((TextBox)sender).Text.Length; 
                    }
                }
            }
        }

        private void Anuluj_BTN_Click(object sender, RoutedEventArgs e)
        {
            edytuj_material_textBox.Clear();
            edytuj_cena_detal_textBox.Clear();
            edytuj_cena_agencyjna_EKO_textBox.Clear();
            edytuj_cena_agencyjna_BEST_textBox.Clear();
            _zmiennaWindow.IGN.IsEnabled = true;
            this.Close();
        }

        string poprzednia_Nazwa;
        Zmienne _zmienna = new Zmienne();
        MainWindow _zmiennaWindow = new MainWindow();
    }
}
