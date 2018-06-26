using System;
using System.IO;
using Xamarin.Forms;
using SQLite;

[assembly: Dependency(typeof(SQLite3))]
namespace Elektrichking.Droid
{
    class SQLite_Android : ISQLite
    {
        public SQLite_Android() { }
        public string GetDatabasePath(string sqliteFilename)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            return path;
        }
    }
}