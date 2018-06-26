using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Elektrichking
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabPgStation : TabbedPage
    {
        public TabPgStation (Station station)
        {
            Title = station.name;
            Children.Add(new PgWayout(station));
            Children.Add(new PgFood(station));
            InitializeComponent();
        }
    }
}