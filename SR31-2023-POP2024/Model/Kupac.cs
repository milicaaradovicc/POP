using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR31_2023_POP2024
{
    public class Kupac{
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string BrojLicneKarte  { get; set; }

        public Kupac(string ime,string prezime, string brojLicneKarte){
            Ime = ime;
            Prezime = prezime;
            BrojLicneKarte = brojLicneKarte;
        }
    }
}
