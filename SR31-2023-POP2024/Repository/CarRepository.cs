using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR31_2023_POP2024.Repository
{
    public class CarRepository : BaseRepository, ICarRepository
    {
        public void PersistCars(List<Automobil> model)
        {
            using (var streamWriter = new StreamWriter(CARS_LOCATION))
            {
                foreach (var automobil in model)
                {
                    streamWriter.WriteLine($"{automobil.ID},{automobil.Marka.Naziv}, {automobil.Marka.DrzavaNastanka},{automobil.Model.NazivModela},{automobil.Godiste},{automobil.Snaga},{automobil.Pogon}");

                }
            }
        }
        public List<Automobil> GetAllCars()
        {
            var cars = new List<Automobil>();

            using (var streamReader = new StreamReader(CARS_LOCATION))
            {
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    var lineParts = line.Split(",");
                    var automobil = new Automobil(
                        lineParts[0], 
                        new MarkaAutomobila(lineParts[1], lineParts[2]), 
                        new ModelAutomobila(new MarkaAutomobila(lineParts[1], lineParts[2]), lineParts[3]), 
                        int.Parse(lineParts[4]), 
                        int.Parse(lineParts[5]), 
                        lineParts[6] 
                    );
                    cars.Add(automobil);
                }
            }

            return cars;
        }

        public Automobil? GetCar(string id)
        {
            var cars = GetAllCars();
            return cars.Find(car => car.ID == id);
        }

        public void AddCar(Automobil automobil)
        {
            var cars = GetAllCars();
            cars.Add(automobil);
            PersistCars(cars);
        }

        public void EditCar(string id, Automobil updatedAutomobil)
        {
            var cars = GetAllCars();
            var index = cars.FindIndex(car => car.ID == id);

            if (index != -1)
            {
                cars[index] = updatedAutomobil;
                PersistCars(cars);
            }
        }

        public void DeleteCar(string id)
        {
            var cars = GetAllCars();
            var index = cars.FindIndex(car => car.ID == id);

            if (index != -1)
            {
                cars.RemoveAt(index);
                PersistCars(cars);
            }
        }


    }
}
   
