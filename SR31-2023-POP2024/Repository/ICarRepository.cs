using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR31_2023_POP2024.Repository
{
    internal interface ICarRepository
    {
        public void PersistCars(List<Automobil> model);
        public List<Automobil> GetAllCars();
        public Automobil? GetCar(int id);
        public void AddCar(Automobil automobil);
        public void EditCar(string id, Automobil automobil);
        public void DeleteCar(int id);
    }
}
