using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR31_2023_POP2024
{
    public class ModelAutomobila{
        public MarkaAutomobila Marka { get; set; }
        public string NazivModela { get; set; }

        public ModelAutomobila(MarkaAutomobila marka, string nazivModela)
        {
            Marka = marka;
            NazivModela = nazivModela;
        }
    }
}
