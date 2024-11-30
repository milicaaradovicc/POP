using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SR31_2023_POP2024.Repository;

namespace SR31_2023_POP2024.Service
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository = new CarRepository();
        private readonly BrandRepository _brandRepository = new BrandRepository(); 


        public List<Automobil> GetAllCars()
        {
            return _carRepository.GetAllCars();
        }

        public Automobil? GetCar(string id)
        {
            return _carRepository.GetCar(id);
        }

        public void AddCar(Automobil automobil)
        {
            
            if (_brandRepository.GetBrand(automobil.Marka.Naziv) == null)
            {
                _brandRepository.AddBrand(automobil.Marka);
            }

            _carRepository.AddCar(automobil);
        }

        public void EditCar(string id, Automobil updatedAutomobil)
        {
            if (_brandRepository.GetBrand(updatedAutomobil.Marka.Naziv) == null)
            {
                _brandRepository.AddBrand(updatedAutomobil.Marka);
            }

            
            _carRepository.EditCar(id, updatedAutomobil);
        }

        public void DeleteCar(string id)
        {
            _carRepository.DeleteCar(id);
        }

        public void SaveCarsToCsv()
        {
            var cars = GetAllCars();
            _carRepository.PersistCars(cars);
        }

        public void PersistCars(List<Automobil> model)
        {
            _carRepository.PersistCars(model);
        }

    }
}


