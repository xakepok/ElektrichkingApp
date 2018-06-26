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
	public partial class PgWayout : ContentPage
	{
        public Desc result;

        public PgWayout (Station station)
		{
            if (station.detour != null)
            {
                Title = "Обходинг";
                var htmlSource = new HtmlWebViewSource();
                string detour = @station.detour.Replace("src=\"", "src=\"http://elektrichking.ru/");
                detour = detour.Replace("href=\"", "href=\"http://elektrichking.ru/");
                htmlSource.Html = detour;
                WebView web = new WebView
                {
                    Source = htmlSource,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                Content = new StackLayout
                {
                    Children =
                    {
                        web
                    }
                };
            }
            else
            {
                Title = "Кассы";
                getData(station.id.ToString());
            }
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
                HttpResponseMessage response = await client.GetAsync("http://elektrichking.ru/index.php?option=com_railway2&task=railway2.desc&id=" + id + "&version=" + MainPage.Vrs);
                HttpContent responseContent = response.Content;
                try
                {
                    string json = await responseContent.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Desc>(json);
                    dataStatus.Text = result.desc;
                }
                catch (Exception e1)
                {
                    dataStatus.Text = "К сожалению у нас нет данных о расписании работы касс на этой станции. Но и турникетов там нет.";
                    //tbFeedback.IsVisible = true;
                }
            }
            catch (Exception e)
            {
                dataStatus.Text = "К сожалению у нас нет данных о расписании работы касс на этой станции. Но и турникетов там нет.";
                //tbFeedback.IsVisible = true;
            }
            StackLayout layout = new StackLayout();
            layout.Children.Add(dataStatus);
            Content = layout;
        }
    }

}