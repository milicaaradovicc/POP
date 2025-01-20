using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR31_2023_POP2024
{
    public class ModelAutomobila{
        public int ID { get; set; }
        public int MarkaID { get; set; }
        public string NazivModela { get; set; }

        public ModelAutomobila(int id, int markaID, string nazivModela)
        {
            ID = id;
            MarkaID = markaID;
            NazivModela = nazivModela;
        }

        public ModelAutomobila(MarkaAutomobila marka, string nazivModela)
        {
            MarkaID = marka.ID;
            NazivModela = nazivModela;
        }
    }
}
