using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SR31_2023_POP2024;

using SR31_2023_POP2024.Repository;

namespace SR31_2023_POP2024
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Kreiraj instancu CarRepository za rad sa automobilima
            var carRepository = new CarRepository();

            // Učitaj sve automobile iz baze
            List<Automobil> allCars = carRepository.GetAllCars();

            // Prikaz broja učitanih automobila
            Console.WriteLine("Broj učitanih automobila: " + allCars.Count);

            // Održavanje konzole otvorenom kako bi se prikazale informacije
            Console.ReadLine();
        }
    }
}
