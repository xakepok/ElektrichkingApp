using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Elektrichking
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PgFood : ContentPage
	{
        public FoodObj result;

        public PgFood (Station station)
		{
            Title = "Места рядом";
            getData(station.id.ToString());
            InitializeComponent ();
		}

        public async void getData(string id)
        {
            Label dataStatus = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Загружаем данные"
            };
            HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://elektrichking.ru/index.php?option=com_railway2&task=railway2.food&id=" + id + "&api=1&version=" + MainPage.Vrs);
                HttpContent responseContent = response.Content;
                try
                {
                    string json = await responseContent.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<FoodObj>(json);
                    List<Image> Logos = new List<Image>();
                    StackLayout layout = new StackLayout();
                    for (var i = 0; i < result.logos.Count; i++)
                    {
                        layout.Children.Add(new Image { Source = new System.Uri(result.logos[i]) });
                    }
                    Content = layout;
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
    }    
}