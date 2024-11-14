using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR31_2023_POP2024
{
    public class MarkaAutomobila{
        public string Naziv { get; set; }
        public string DrzavaNastanka { get; set; }

        public MarkaAutomobila(string naziv, string drzavaNastanka)
        {
            Naziv = naziv;
            DrzavaNastanka = drzavaNastanka;
        }
    }
}
