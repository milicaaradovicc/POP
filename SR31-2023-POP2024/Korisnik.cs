using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR31_2023_POP2024
{
    public class Korisnik{
        public string Ime { get; set; }
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string JMBG { get; set; }
        public decimal Zarada { get; set;}

        public Korisnik(string ime, string korisnickoIme, string lozinka, string jmbg, decimal zarada)
        {
            Ime = ime;
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            JMBG = jmbg;
            Zarada = zarada;
        }
    }
}
