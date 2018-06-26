using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using SQLite;
using Xamarin.Forms;

namespace Elektrichking
{
    public class StationRepository
    {
        SQLiteConnection database;

        public StationRepository(string filename)
        {
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(filename);
            database = new SQLiteConnection(databasePath);
            database.CreateTable<Station>();
        }
        /*public IEnumerable<Station> GetItems()
        {
            return (from i in database.Table<Station>() select i).ToList();

        }*/
        public Station GetItem(int id)
        {
            return database.Get<Station>(id);
        }
        public int DeleteItem(int id)
        {
            return database.Delete<Station>(id);
        }
        public int SaveItem(Station item)
        {
            if (item.id != 0)
            {
                database.Update(item);
                return item.id;
            }
            else
            {
                return database.Insert(item);
            }
        }
    }

        public class RootObject
    {
        public List<Station> Station { get; set; }
    }

    [Table("stations")]
    public class Station
    {
        [PrimaryKey, Column("id")]
        public int id { get; set; }

        public string name { get; set; }
        public string direction { get; set; }
        public int directionID { get; set; }
        public string detour { get; set; }
        public string turnstiles { get; set; }
        public string color { get; set; }
    }

    public class Desc
    {
        public string desc { get; set; }
    }

    public class FoodObj
    {
        public List<string> logos { get; set; }
        public List<string> reviews { get; set; }
    }
}
