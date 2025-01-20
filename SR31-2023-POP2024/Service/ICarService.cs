using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR31_2023_POP2024.Service
{
    public interface ICarService
    {
        void PersistCars(List<Automobil> cars);
        List<Automobil> GetAllCars();
        Automobil? GetCar(int id);
        void AddCar(Automobil automobil);
        void EditCar(string id, Automobil updatedAutomobil);
        void DeleteCar(int id);



    }
}
