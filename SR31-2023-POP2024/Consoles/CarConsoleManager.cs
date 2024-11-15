using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SR31_2023_POP2024.Service;
using SR31_2023_POP2024.Repository;

namespace SR31_2023_POP2024.Consoles
{
    public class CarConsoleManager
    {
        private readonly ICarService _carService = new CarService();

        public void ManageCars()
        {
            string command;
            do
            {
                command = PrintMenuGetCommand();
                HandleCommand(command);
            }
            while (command != "x");

        }

        private string PrintMenuGetCommand()
        {
            Console.WriteLine("Odaberite opciju nad automobilima");
            Console.WriteLine();
            Console.WriteLine("1. Ispis svih automobila");
            Console.WriteLine("2. Ispis jednog automobila");
            Console.WriteLine("3. Dodavanje automobila");
            Console.WriteLine("4. Izmena automobila");
            Console.WriteLine("5. Brisanje automobila");
            Console.WriteLine();
            Console.WriteLine("x. Izadji");

            var command = Console.ReadLine();
            return command;
        }

        private void HandleCommand(string command)
        {
            switch (command)
            {
                case "1":
                    PrintCars();
                    break;
                case "2":
                    PrintCar();
                    break;
                case "3":
                    AddCar();
                    break;
                case "4":
                    EditCar();
                    break;
                case "5":
                    DeleteCar();
                    break;
                case "x":
                    Console.WriteLine("Izlazak iz programa.");
                    break;
                default:
                    break;
            }
        }

        private void PrintCars()
        {
            var cars = _carService.GetAllCars();
            Console.WriteLine("ID, Marka, Drzava nastanka, Model, Godiste, Snaga, Pogon");

            foreach (var car in cars)
            {
                Console.WriteLine($"{car.ID},{car.Marka.Naziv},{car.Marka.DrzavaNastanka},{car.Model.NazivModela},{car.Godiste},{car.Snaga},{car.Pogon}");
            }
        }

        private void PrintCar()
        {
            Console.WriteLine("Unesite ID automobila:");
            var id = Console.ReadLine();
            var car = _carService.GetCar(id);

            if (car == null)
            {
                Console.WriteLine("Automobil sa unetim id-jem nije pronadjen");
            }
            else
            {
                Console.WriteLine("ID, Marka, Drzava nastanka, Model, Godiste, Snaga, Pogon");
                Console.WriteLine($"{car.ID},{car.Marka.Naziv},{car.Marka.DrzavaNastanka},{car.Model.NazivModela},{car.Godiste},{car.Snaga},{car.Pogon}");
            }
        }

        private void AddCar()
        {
            Console.WriteLine("Unesite ID automobila: ");
            var id = Console.ReadLine();

            Console.WriteLine("Unesite naziv marke: ");
            var markaNaziv = Console.ReadLine();

            Console.WriteLine("Unesite državu nastanka automobila:");
            var drzavaNastanka = Console.ReadLine();

            Console.WriteLine("Unesite naziv modela: ");
            var nazivModela = Console.ReadLine();

            Console.WriteLine("Unesite godiste automobila: ");
            var godiste = int.Parse(Console.ReadLine());

            Console.WriteLine("Unesite snagu automobila: ");
            var snaga = int.Parse(Console.ReadLine());

            Console.WriteLine("Unesite tip pogona: ");
            var pogon = Console.ReadLine();

            var automobil = new Automobil(
                id, 
                new MarkaAutomobila(markaNaziv, drzavaNastanka),
                new ModelAutomobila(new MarkaAutomobila(markaNaziv, drzavaNastanka), nazivModela), 
                godiste,
                snaga,
                pogon
            );

            _carService.AddCar(automobil);
            _carService.SaveCarsToCsv();
        }

        private void EditCar()
        {
            Console.WriteLine("Unesite ID automobila koji želite da izmenite:");
            var id = Console.ReadLine();
            var car = _carService.GetCar(id);

            if (car == null)
            {
                Console.WriteLine("Automobil sa unetim ID-jem nije pronađen.");
                return;
            }

            Console.WriteLine($"Marka: {car.Marka.Naziv}. Unesite novu marku ili pritisnite Enter za preskakanje:");
            var novaMarka = Console.ReadLine();
            if (!string.IsNullOrEmpty(novaMarka))
            {
                car.Marka.Naziv = novaMarka;
            }

            Console.WriteLine($" Država nastanka: {car.Marka.DrzavaNastanka}. Unesite novu državu nastanka ili pritisnite Enter za preskakanje:");
            var novaDrzava = Console.ReadLine();
            if (!string.IsNullOrEmpty(novaDrzava))
            {
                car.Marka.DrzavaNastanka = novaDrzava;
            }

            Console.WriteLine($"Model: {car.Model.NazivModela}. Unesite novi model ili pritisnite Enter za preskakanje:");
            var noviModel = Console.ReadLine();
            if (!string.IsNullOrEmpty(noviModel))
            {
                car.Model.NazivModela = noviModel;
            }

            Console.WriteLine($"Godište: {car.Godiste}. Unesite novo godište ili pritisnite Enter za preskakanje:");
            var novoGodisteStr = Console.ReadLine();
            if (!string.IsNullOrEmpty(novoGodisteStr))
            {
                car.Godiste = int.Parse(novoGodisteStr);
            }

            Console.WriteLine($"Snaga: {car.Snaga}. Unesite novu snagu ili pritisnite Enter za preskakanje:");
            var novaSnagaStr = Console.ReadLine();
            if (!string.IsNullOrEmpty(novaSnagaStr))
            {
                car.Snaga = int.Parse(novaSnagaStr);
            }

            Console.WriteLine($"Pogon: {car.Pogon}. Unesite novi pogon ili pritisnite Enter za preskakanje:");
            var noviPogon = Console.ReadLine();
            if (!string.IsNullOrEmpty(noviPogon))
            {
                car.Pogon = noviPogon;
            }

            

            _carService.EditCar(id, car);
            _carService.SaveCarsToCsv();
            Console.WriteLine("Automobil je uspešno izmenjen.");
        }

        private void DeleteCar()
        {
            Console.WriteLine("Unesite ID automobila koji želite da obrišete:");
            var id = Console.ReadLine();
            _carService.DeleteCar(id);
            _carService.SaveCarsToCsv();
            Console.WriteLine("Automobil je uspešno obrisan.");
        }
    }
}

