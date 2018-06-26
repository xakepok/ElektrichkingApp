using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Elektrichking
{
	public partial class App : Application
	{
        public const string DATABASE_NAME = "elektrichking.db";
        public static StationRepository database;
        public static StationRepository Database
        {
            get
            {
                if (database == null)
                {
                    database = new StationRepository(DATABASE_NAME);
                }
                return database;
            }
        }
        public App ()
		{
			InitializeComponent();

            MainPage = new NavigationPage(new Elektrichking.MainPage());
        }

		protected override void OnStart ()
		{
            // Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
