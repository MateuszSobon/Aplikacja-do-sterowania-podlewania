using System;
using CommunityToolkit.Mvvm.ComponentModel;
using RestSharp;
    

namespace MyPlantApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            var lista = new List<string>();
            lista.Add("Skokowe mokre");
            lista.Add("Skokowe pośrednie");
            lista.Add("Skokowe suche");
            lista.Add("PID mokre");
            lista.Add("PID pośrednie");
            lista.Add("PID suche");

            picker.ItemsSource = lista;


        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            string helper;
            string url = "http://192.168.1.227/readData";

            var client = new RestClient(url);

            var request = new RestRequest();

            var response = client.Get(request);
            if (response != null) 
            {
                helper = response.Content.ToString();

                List<string> lista = (helper.Split(',', '\n', '\r' )).ToList() ;
                List<string> newLista = new List<string>() ;

                foreach ( var item in lista ) 
                {
                    if ( item != string.Empty )
                    {
                        newLista.Add(item);
                    }
                }

                if (newLista.Count == 7 )
                {
                    wilLabel.Text = ((decimal.Parse(newLista[1]) + decimal.Parse(newLista[2])) /2).ToString() + " °C"; 

                    tempLabel.Text = ((decimal.Parse(newLista[3]) + decimal.Parse(newLista[4])) / 2).ToString() + " %"; 

                    glebaLabel.Text = ((decimal.Parse(newLista[5]) + decimal.Parse(newLista[6])) / 2).ToString() + "";

                    trybPracyLabel.Text = WorkName(int.Parse(newLista[0]));
                }
                else
                {
                    DisplayAlert("Błąd odczytu", $"Błędna ilość parametrów: {newLista.Count}", "OK");
                }

            }

        }

        private void CounterBtn2_Clicked(object sender, EventArgs e)
        {
            string helper;
            int tryb;

            var request = new RestRequest();

            switch (picker.SelectedIndex)
            {
                case 0:
                    helper = "TRYB1"; tryb = 1; break;
                case 1:
                    helper = "TRYB2"; tryb = 1; break;
                case 2:
                    helper = "TRYB3"; tryb = 1; break;
                case 3:
                    helper = "TRYB4"; tryb = 2; break;
                case 4:
                    helper = "TRYB5"; tryb = 2; break;
                case 5:
                    helper = "TRYB6"; tryb = 2; break;
                default:
                    return;
            }

            string url = "http://192.168.1.227/";
            string hiturl = url + helper;
            var client = new RestClient(hiturl);

            var response = client.Get(request);

            if (response.IsSuccessStatusCode)
            {
                DisplayAlert("Zmiana trybu", "Tryb pracy został zmieniony", "OK");
                trybPracyLabel.Text = WorkName(tryb);
            }
        }

        private string WorkName(int indeks)
        {
            string nazwa;

            switch (indeks)
            {
                case 1:
                    nazwa = "Skokowe"; break;
                case 2:
                    nazwa = "PID"; break;

                default:
                    return null;
            }

            return nazwa;
        }
    }
}