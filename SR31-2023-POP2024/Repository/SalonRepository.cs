using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SR31_2023_POP2024.Model;

namespace SR31_2023_POP2024.Repository
{
    public class SalonRepository : BaseRepository
    {
        public Salon GetSalon()
        {
            string query = "SELECT Ime, Adresa FROM Salon WHERE ID = 1"; // Pretpostavljamo da je ID 1 za salon
            var salons = ExecuteQuery(query, reader =>
            {
                return new Salon(
                    1, // Pretpostavljamo da je ID 1
                    reader["Ime"].ToString(),
                    reader["Adresa"].ToString()
                );
            });

            // Vraćamo prvi salon jer očekujemo samo jedan salon
            return salons.FirstOrDefault();
        }
    }


}
