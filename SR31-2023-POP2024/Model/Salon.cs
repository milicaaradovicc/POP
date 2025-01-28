using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR31_2023_POP2024.Model
{
    public class Salon
    {
        public int ID { get; set; }
        public string Ime { get; set; }
        public string Adresa { get; set; }

        public Salon(int id, string ime, string adresa)
        {
            ID = id;
            Ime = ime;
            Adresa = adresa;
        }
    }

}
