using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR31_2023_POP2024
{
    public class MarkaAutomobila{

        public int ID { get; set; }
        public string Naziv { get; set; }
        public string DrzavaNastanka { get; set; }

        public MarkaAutomobila(int id, string naziv, string drzavaNastanka)
        {
            ID = id;
            Naziv = naziv;
            DrzavaNastanka = drzavaNastanka;

        }
        public MarkaAutomobila(string naziv, string drzavaNastanka)
        {
            Naziv = naziv;
            DrzavaNastanka = drzavaNastanka;
        }


    }
}
