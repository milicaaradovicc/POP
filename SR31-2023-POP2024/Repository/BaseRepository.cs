using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR31_2023_POP2024.Repository
{
    public abstract class BaseRepository
    {
        public static string DATA_LOCATION = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
        protected string CARS_LOCATION = Path.Combine(DATA_LOCATION, "cars.csv");


        public static void EnsureDataDirExists()
        {
            if (!Directory.Exists(DATA_LOCATION))
            {
                Directory.CreateDirectory(DATA_LOCATION);
            }

            if (!File.Exists(Path.Combine(DATA_LOCATION, "cars.csv")))
            {
                using (var stream = new StreamWriter(Path.Combine(DATA_LOCATION, "cars.csv"), true))
                {
                    stream.Close();
                }
            }
        }
    }
}
