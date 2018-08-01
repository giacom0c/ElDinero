﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dinero.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Contanti : ContentPage
	{
		public Contanti ()
		{
			InitializeComponent ();
		}

        //Viene richiamato al caricamento della schermata
        protected override void OnAppearing()
        {
            Inizializza(App.whatDo);            
        }

        //Viene richiamato quando l'utente seleziona un oggetto dal Picker
        private void OnPicker(object sender, EventArgs e)
        {
            var pick = picker.Items[picker.SelectedIndex];

            switch (pick)
            {
                case "All":
                    App.whatDo = 0;
                    Inizializza(App.whatDo);
                    break;

                case "Last Week":
                    App.whatDo = 1;
                    Inizializza(App.whatDo);
                    break;

                case "Last Month":
                    App.whatDo = 2;
                    Inizializza(App.whatDo);
                    break;

                default:
                    Console.WriteLine("ERROREEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
                    break;
            }
        }

        //Viene richiamato quando l'utente selezione un oggetto dalla Listview
        async private void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem;
            if (item == null)
            {
                return;
            }
            var scelta = await DisplayAlert("Selection", "Do you wish to delete this transaction?", "YES", "NO");
            if (scelta)  //SI
            {
                /*using (SQLite.SQLiteConnection conn = new SQLite.SQLiteConnection(App.DB_CONTANTI))
                {
                    conn.Delete(item);
                }*/
                App.DatabaseContanti.DeleteTransazione(item);

                Inizializza(App.whatDo);                
            }
            ((ListView)sender).SelectedItem = null;
        }

        //Entrata per contanti
        private void Button_Clicked_Contanti1(object sender, EventArgs e)
        {
            App.addType = 1;
            Navigation.PushAsync(new Nuovo());
        }

        //Uscita per contanti
        private void Button_Clicked_Contanti2(object sender, EventArgs e)
        {
            App.addType = 2;
            Navigation.PushAsync(new Nuovo());
        }

        //Funzione per aggiornare i dati in base alle azioni dell'utente
        private void Inizializza(int filtro)
        {
            var entrate = App.DatabaseContanti.QueryEntrata(filtro);
            entrateLabel.Text = entrate.ToString();

            var uscite = App.DatabaseContanti.QueryUscita(filtro);
            usciteLabel.Text = uscite.ToString();

            var saldo = Convert.ToDouble(usciteLabel.Text) + Convert.ToDouble(entrateLabel.Text);
            saldo = Math.Round(saldo, 2);
            saldoLabel.Text = saldo.ToString();

            var trlistcont = App.DatabaseContanti.GetTransazioni(filtro);
            contantiLw.ItemsSource = trlistcont.ToList();            
        }
    }
}