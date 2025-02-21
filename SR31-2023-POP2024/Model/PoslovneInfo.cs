using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR31_2023_POP2024
{
    public class PoslovneInfo{
        public decimal CenaNabavke { get; set; }
        public DateTime DatumNabavke { get; set; }
        public bool Prodato { get; set; }
        public decimal CenaProdaje { get; set; }    
        public DateTime? DatumProdaje { get; set; }
        public Korisnik Prodavac { get; set; }
        public Kupac Kupac { get; set; }
        public Automobil Automobil { get; set; }

        public decimal Profit => (CenaProdaje - CenaNabavke) * 0.25m;

        public PoslovneInfo(decimal cenaNabavke, DateTime datumNabavke, bool prodato,decimal cenaProdaje, 
                            DateTime? datumProdaje, Korisnik prodavac, Kupac kupac, Automobil automobil)

        {
            CenaNabavke = cenaNabavke;
            DatumNabavke = datumNabavke;
            Prodato = prodato;
            CenaProdaje = cenaProdaje;
            DatumProdaje = datumProdaje;
            Prodavac = prodavac;
            Kupac = kupac;
            Automobil = automobil;
        }
    }
}
