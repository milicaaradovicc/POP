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
        public Korisnik GetKorisnikByUsername(string korisnickoIme)
        {
            string query = "SELECT Ime, Prezime, KorisnickoIme, Lozinka, JMBG, Zarada FROM Korisnik WHERE KorisnickoIme = @korisnickoIme";

            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@korisnickoIme", korisnickoIme);

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
                        return null; 
                    }
                }
            }
        }


        public List<Korisnik> GetAllKorisnici()
        {
            List<Korisnik> korisnici = new List<Korisnik>();
            string query = "SELECT Ime, Prezime, KorisnickoIme, Lozinka, JMBG FROM Korisnik WHERE Deleted = 0"; 

            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(query, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        korisnici.Add(new Korisnik(
                            reader["Ime"].ToString(),
                            reader["Prezime"].ToString(),
                            reader["KorisnickoIme"].ToString(),
                            reader["Lozinka"].ToString(),
                            reader["JMBG"].ToString(),
                            0 
                        ));
                    }
                }
            }

            return korisnici;
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

        public static class SessionManager
        {
            private static Korisnik _prijavljeniKorisnik = null;

            public static bool JePrijavljen()
            {
                return _prijavljeniKorisnik != null;
            }

            public static void PrijaviKorisnika(Korisnik korisnik)
            {
                _prijavljeniKorisnik = korisnik;
            }

            public static void OdjavitiKorisnika()
            {
                _prijavljeniKorisnik = null;
            }

            public static Korisnik GetTrenutniKorisnik()
            {
                return _prijavljeniKorisnik;
            }

        }

        public bool JMBGExists(string jmbg)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(*) FROM Korisnik WHERE JMBG = @JMBG";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@JMBG", jmbg);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        public void DeleteKorisnik(string jmbg)
        {
            var query = $"UPDATE Korisnik SET Deleted = 1 WHERE JMBG = @JMBG";
            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@JMBG", jmbg);
                command.ExecuteNonQuery();
            }
        }
    }
}
