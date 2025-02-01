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

        public List<Automobil> GetAllCars()
        {
            return _carRepository.GetAllCars();
        }

        public Automobil? GetCar(int id)
        {
            return _carRepository.GetCar(id);
        }

        public void AddCar(Automobil automobil)
        {
            _carRepository.AddCar(automobil);
        }

        public void EditCar(string id, Automobil updatedAutomobil)
        {
            _carRepository.EditCar(id, updatedAutomobil);
        }

        public void DeleteCar(int id)
        {
            _carRepository.DeleteCar(id);
        }

        public void PersistCars(List<Automobil> cars)
        {
            _carRepository.PersistCars(cars);
        }
        public List<Automobil> GetCarsByLoggedUser()
        {
            return _carRepository.GetCarsByLoggedUser();
        }



        // public void SaveCarsToCsv()
        //{
        //  var cars = GetAllCars();
        // _carRepository.PersistCars(cars);
        //}

        // public void PersistCars(List<Automobil> model)
        // {
        //      _carRepository.PersistCars(model);
        //}

    }
}


