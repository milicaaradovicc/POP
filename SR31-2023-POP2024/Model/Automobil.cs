using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SR31_2023_POP2024.Model;

namespace SR31_2023_POP2024
{
    public class Automobil{
        public int ID { get; set; }
        public int MarkaID { get; set; }
        public int ModelID { get; set; }  
        public MarkaAutomobila Marka { get; set; }  
        public ModelAutomobila Model { get; set; }  
        public int Godiste { get; set; }
        public int Snaga { get; set; }
        public  Pogon Pogon { get; set; }
        public bool Deleted { get; set; }


        public Automobil(int id, MarkaAutomobila marka, ModelAutomobila model, int godiste, int snaga, Pogon pogon, bool deleted = false)
        {
            ID = id;
            Marka = marka; 
            Model = model; 
            MarkaID = marka.ID; 
            ModelID = model.ID; 
            Godiste = godiste;
            Snaga = snaga;
            Pogon = pogon;
            Deleted = deleted;
        }
    }
}
