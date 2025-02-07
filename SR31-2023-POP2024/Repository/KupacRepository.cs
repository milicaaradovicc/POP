using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SR31_2023_POP2024.Repository
{
    public class KupacRepository : BaseRepository
    {
        public int AddKupac(Kupac kupac)
        {
            var query = "INSERT INTO Kupac (Ime, Prezime, BrojLicneKarte) VALUES (@Ime, @Prezime, @BrojLicneKarte); SELECT SCOPE_IDENTITY();";

            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Ime", kupac.Ime);
                command.Parameters.AddWithValue("@Prezime", kupac.Prezime);
                command.Parameters.AddWithValue("@BrojLicneKarte", kupac.BrojLicneKarte);

                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public Kupac? GetKupac(string brojLicneKarte)
        {
            var query = "SELECT ID, Ime, Prezime, BrojLicneKarte FROM Kupac WHERE BrojLicneKarte = @BrojLicneKarte";

            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BrojLicneKarte", brojLicneKarte);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Kupac(
                            reader["Ime"].ToString(),
                            reader["Prezime"].ToString(),
                            reader["BrojLicneKarte"].ToString()
                        );
                    }
                }
            }

            return null;
        }

        public int GetKupacByBrojLicneKarte(string brojLicneKarte)
        {
            var query = "SELECT ID FROM Kupac WHERE BrojLicneKarte = @BrojLicneKarte";

            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@BrojLicneKarte", brojLicneKarte);
                var result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }
    }
}
