using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SR31_2023_POP2024;
using SR31_2023_POP2024.Consoles;
using SR31_2023_POP2024.Repository;

namespace SR31_2023_POP2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BaseRepository.EnsureDataDirExists();

            var ccm = new CarConsoleManager();
            ccm.ManageCars();


            static void PersistModel(List<Automobil> model)
            {
                var carRepository = new CarRepository();
                carRepository.PersistCars(model);
            }
        }
    }
}