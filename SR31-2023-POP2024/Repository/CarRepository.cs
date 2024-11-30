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
            using (var streamWriter = new StreamWriter(CARS_LOCATION, false))
            {
                foreach (var automobil in model)
                {
                    streamWriter.WriteLine($"{automobil.ID},{automobil.Marka.Naziv},{automobil.Model.NazivModela},{automobil.Godiste},{automobil.Snaga},{automobil.Pogon},{automobil.Deleted}");
                }
            }
        }

        public List<Automobil> GetAllCars()
        {
            var cars = new List<Automobil>();
            var brandRepository = new BrandRepository();
            var allBrands = brandRepository.GetAllBrands();

            if (!File.Exists(CARS_LOCATION))
            {
                Console.WriteLine("Fajl sa automobilima ne postoji.");
                return cars;
            }

            using (var streamReader = new StreamReader(CARS_LOCATION))
            {
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var lineParts = line.Split(",");
                    var marka = allBrands.Find(b => b.Naziv == lineParts[1]) ?? new MarkaAutomobila(lineParts[1], "Nepoznata");
                    var automobil = new Automobil(
                        lineParts[0],
                        marka,
                        new ModelAutomobila(marka, lineParts[2]),
                        int.Parse(lineParts[3]),
                        int.Parse(lineParts[4]),
                        lineParts[5],
                        bool.Parse(lineParts[6])
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
            var brandRepository = new BrandRepository();
            brandRepository.AddBrand(automobil.Marka);

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
                cars[index].Deleted = true; 
                PersistCars(cars); 
            }
        }


    }
}
   
