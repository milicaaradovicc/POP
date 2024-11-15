using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR31_2023_POP2024.Service
{
    internal interface ICarService
    {
        void PersistCars(List<Automobil> model);
        public List<Automobil> GetAllCars();
        public Automobil? GetCar(string id);
        public void AddCar(Automobil automobil);
        public void EditCar(string id, Automobil updatedAutomobil);
        public void DeleteCar(string id);
        void SaveCarsToCsv();



    }
}
