using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Xamarin.Forms;
using System.Web;
using Newtonsoft.Json;
using SQLite;

namespace Elektrichking
{
    public partial class MainPage : ContentPage
    {
        public List<string> Stations { get; set; }
        public string Status { get; set; }
        public RootObject result;
        public static string Vrs { get; set; }

        public MainPage()
        {
            InitializeComponent();
            filterStation.IsVisible = false;
            Vrs = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            prodVersion.Text = "Версия программы " + Vrs;
            Title = "Электричкинг";
            Status = "Загружаем";

            getData();
            this.BindingContext = this;
        }

        public async void getData()
        {
            HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://elektrichking.ru/index.php?option=com_railway2&task=railway2.mobile&version=" + Vrs);
                HttpContent responseContent = response.Content;
                try
                {
                    string json = await responseContent.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<RootObject>(json);
                    List<Station> Stations = new List<Station>();
                    for (var i = 0; i < result.Station.Count; i++)
                    {
                        Station st = new Station { name = result.Station[i].name, direction = result.Station[i].direction, detour = result.Station[i].detour, id = result.Station[i].id, color = result.Station[i].color };
                        try
                        {
                            App.Database.SaveItem(st);
                        }
                        catch (Exception e2)
                        {
                            dataStatus.Text = "Ошибка записи в базу: " + e2.Message;
                        }
                        finally
                        {
                            Stations.Add(st);
                        }
                    }

                    directionsList.ItemsSource = Stations;
                    //dataStatus.IsVisible = false;
                    filterStation.IsVisible = true;
                }
                catch (Exception e1)
                {
                    dataStatus.Text = "Не удалось распарсить данные: " + e1.Message;
                }
            }
            catch (Exception e)
            {
                dataStatus.Text = "Не удалось получить данные. Проверьте соединение с Интернетом";
            }
        }

        private void OnButtonClicked(object sender, System.EventArgs e)
        {
            Button button = (Button)sender;
            button.Text = "Загружаем...";
            button.BackgroundColor = Color.Red;
        }

        private async void ToWayoutPage(object sender, SelectedItemChangedEventArgs args)
        {
            Station selectedStation = args.SelectedItem as Station;
            if (selectedStation != null)
            {
                directionsList.SelectedItem = null;
                await Navigation.PushAsync(new TabPgStation(selectedStation));
            }

        }

        public void onFilterStation(object sender, TextChangedEventArgs e)
        {
            List<Station> items = new List<Station>();
            for (int i = 0; i < result.Station.Count; i++)
            {
                if ((result.Station[i].name.IndexOf(e.NewTextValue, StringComparison.OrdinalIgnoreCase) != -1 && e.NewTextValue != "") || (e.NewTextValue == ""))
                {
                    items.Add(
                        new Station { name = result.Station[i].name, direction = result.Station[i].direction, detour = result.Station[i].detour, id = result.Station[i].id, color = result.Station[i].color }
                    );
                }
            }
            directionsList.ItemsSource = items;
        }

        protected override async void OnAppearing()
        {
            // создание таблицы, если ее нет
            //await App.Database.CreateTable();

            base.OnAppearing();
        }
    }

}
