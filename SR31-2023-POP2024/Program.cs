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
            var model = CreateModel();
            PersistModel(model);

            var ccm = new CarConsoleManager();
            ccm.ManageCars();
        }

        static List<Automobil> CreateModel()
        {
            List<Automobil> model = new List<Automobil>();

            var marka1 = new MarkaAutomobila("Audi", "Nemačka");
            var model1 = new ModelAutomobila(marka1, "A4");
            var car1 = new Automobil(
                "1", // id
                 marka1, // MarkaAutomobila
                 model1, // ModelAutomobila
                 2020, // Godiste
                 150, // Snaga
                 "Prednji" // Pogon
            );


            var marka2 = new MarkaAutomobila("BMW", "Nemačka");
            var model2 = new ModelAutomobila(marka2, "X5");
            var car2 = new Automobil(
                "2",
                 marka2,
                 model2, 
                 2022, 
                 250, 
                "Zadnji" 
            );

            model.Add(car1);
            model.Add(car2);

            return model;
        }

        static void PersistModel(List<Automobil> model)
        {
            var carRepository = new CarRepository();
            carRepository.PersistCars(model);
        }
    }
}