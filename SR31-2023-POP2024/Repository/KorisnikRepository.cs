using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SR31_2023_POP2024.Model;


namespace SR31_2023_POP2024.Repository
{
    public class KorisnikRepository : BaseRepository
    {
        public Korisnik GetKorisnikByUsernameAndPassword(string korisnickoIme, string lozinka)
        {
            string query = "SELECT Ime, Prezime, KorisnickoIme, Lozinka, JMBG, Zarada FROM Korisnik WHERE KorisnickoIme = @korisnickoIme AND Lozinka = @lozinka";

            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@korisnickoIme", korisnickoIme);
                command.Parameters.AddWithValue("@lozinka", lozinka);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Korisnik(
                            reader["Ime"].ToString(),
                            reader["Prezime"].ToString(),
                            reader["KorisnickoIme"].ToString(),
                            reader["Lozinka"].ToString(),
                            reader["JMBG"].ToString(),
                            Convert.ToDecimal(reader["Zarada"])
                        );
                    }
                    else
                    {
                        return null; // Nema korisnika sa datim podacima
                    }
                }
            }
        }

        public void AddKorisnik(Korisnik korisnik)
        {
            string query = "INSERT INTO Korisnik (Ime, Prezime, KorisnickoIme, Lozinka, JMBG, Zarada) VALUES (@ime, @prezime, @korisnickoIme, @lozinka, @jmbg, @zarada)";

            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ime", korisnik.Ime);
                command.Parameters.AddWithValue("@prezime", korisnik.Prezime);
                command.Parameters.AddWithValue("@korisnickoIme", korisnik.KorisnickoIme);
                command.Parameters.AddWithValue("@lozinka", korisnik.Lozinka);
                command.Parameters.AddWithValue("@jmbg", korisnik.JMBG);
                command.Parameters.AddWithValue("@zarada", korisnik.Zarada);

                command.ExecuteNonQuery();
            }
        }
    }
}
