using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR31_2023_POP2024
{
    public class Automobil{
        public string ID { get; set; }
        public MarkaAutomobila Marka { get; set;}
        public ModelAutomobila Model { get; set; }
        public int Godiste { get; set; }
        public int Snaga { get; set;}
        public string Pogon { get; set; }
        public bool Deleted { get; set; }

        public Automobil(string id, MarkaAutomobila marka, ModelAutomobila model, int godiste, int snaga, string pogon, bool deleted = false) {
            ID = id;
            Marka = marka;
            Model = model;
            Godiste = godiste;
            Snaga = snaga;
            Pogon = pogon;
            Deleted = deleted;
        }
    }
}
