using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SR31_2023_POP2024.Model;
using static SR31_2023_POP2024.Repository.KorisnikRepository;

namespace SR31_2023_POP2024.Repository
{
    public class CarRepository : BaseRepository, ICarRepository
    {
        public const string CONNECTION_STRING = @"Data Source=localhost;Initial Catalog=POP;Integrated Security=True;Trust Server Certificate=True";


        public void PersistCars(List<Automobil> cars)
        {
            foreach (var car in cars)
            {
                var existingCar = GetCar(car.ID);

                if (existingCar == null)
                {

                    AddCar(car);
                }
                else
                {

                    EditCar(car.ID.ToString(), car);
                }
            }
        }

        public List<Automobil> GetAllCars()
        {
            string query = @"
            SELECT 
            a.ID, a.MarkaID, a.ModelID, a.Godiste, a.Snaga, a.PogonID, a.Deleted,
            ma.Naziv AS MarkaNaziv, ma.DrzavaNastanka AS MarkaDrzava,
            mo.NazivModela AS ModelNaziv,
            p.Naziv AS PogonNaziv
            FROM Automobil a
            INNER JOIN MarkaAutomobila ma ON a.MarkaID = ma.ID
            INNER JOIN ModelAutomobila mo ON a.ModelID = mo.ID
            INNER JOIN Pogon p ON a.PogonID = p.ID
            INNER JOIN PoslovneInfo pi ON a.ID = pi.AutomobilID
            WHERE a.Deleted = 0 and pi.Prodato = 0";

            return ExecuteQuery(query, reader => new Automobil(
                reader.GetInt32(reader.GetOrdinal("ID")),
                new MarkaAutomobila(
                    reader.GetInt32(reader.GetOrdinal("MarkaID")),
                    reader.GetString(reader.GetOrdinal("MarkaNaziv")),
                    reader.GetString(reader.GetOrdinal("MarkaDrzava"))
                ),
                new ModelAutomobila(
                    reader.GetInt32(reader.GetOrdinal("ModelID")),
                    reader.GetInt32(reader.GetOrdinal("MarkaID")),
                    reader.GetString(reader.GetOrdinal("ModelNaziv"))
                ),


                reader.GetInt32(reader.GetOrdinal("Godiste")),
                reader.GetInt32(reader.GetOrdinal("Snaga")),
               (Pogon)reader.GetInt32(reader.GetOrdinal("PogonID")),

            reader.GetBoolean(reader.GetOrdinal("Deleted"))
            ));
        }

        public List<Automobil> GetCarsByLoggedUser()
        {
            Korisnik ulogovaniKorisnik = SessionManager.GetTrenutniKorisnik();

            if (ulogovaniKorisnik == null)
            {
                return new List<Automobil>();
            }

            KorisnikRepository korisnikRepo = new KorisnikRepository();
            int prodavacID = korisnikRepo.GetKorisnikIdByJMBG(ulogovaniKorisnik.JMBG);

            if (prodavacID == 0)
            {
                return new List<Automobil>();
            }

            string query = @"
            SELECT 
            a.ID, a.MarkaID, a.ModelID, a.Godiste, a.Snaga, a.PogonID, a.Deleted,
            ma.Naziv AS MarkaNaziv, ma.DrzavaNastanka AS MarkaDrzava,
            mo.NazivModela AS ModelNaziv,
            p.Naziv AS PogonNaziv
            FROM Automobil a
            INNER JOIN MarkaAutomobila ma ON a.MarkaID = ma.ID
            INNER JOIN ModelAutomobila mo ON a.ModelID = mo.ID
            INNER JOIN Pogon p ON a.PogonID = p.ID
            INNER JOIN PoslovneInfo pi ON a.ID = pi.AutomobilID
            WHERE a.Deleted = 0 AND pi.ProdavacID = @ProdavacID";

            return ExecuteQuery(query, reader => new Automobil(
                reader.GetInt32(reader.GetOrdinal("ID")),
                new MarkaAutomobila(
                    reader.GetInt32(reader.GetOrdinal("MarkaID")),
                    reader.GetString(reader.GetOrdinal("MarkaNaziv")),
                    reader.GetString(reader.GetOrdinal("MarkaDrzava"))
                ),
                new ModelAutomobila(
                    reader.GetInt32(reader.GetOrdinal("ModelID")),
                    reader.GetInt32(reader.GetOrdinal("MarkaID")),
                    reader.GetString(reader.GetOrdinal("ModelNaziv"))
                ),
                reader.GetInt32(reader.GetOrdinal("Godiste")),
                reader.GetInt32(reader.GetOrdinal("Snaga")),
                (Pogon)reader.GetInt32(reader.GetOrdinal("PogonID")),
                reader.GetBoolean(reader.GetOrdinal("Deleted"))
            ), new SqlParameter("@ProdavacID", prodavacID));
        }

        public Automobil? GetCar(int id)
        {
            string query = @"
            SELECT 
            a.ID, a.MarkaID, a.ModelID, a.Godiste, a.Snaga, a.PogonID, a.Deleted,
            ma.Naziv AS MarkaNaziv, ma.DrzavaNastanka AS MarkaDrzava,
            mo.NazivModela AS ModelNaziv,
            p.Naziv AS PogonNaziv
            FROM Automobil a
            INNER JOIN MarkaAutomobila ma ON a.MarkaID = ma.ID
            INNER JOIN ModelAutomobila mo ON a.ModelID = mo.ID
            INNER JOIN Pogon p ON a.PogonID = p.ID
             WHERE  a.Deleted = 0";

            var result = ExecuteQuery(query, reader =>
            {
                var marka = new MarkaAutomobila(
                    reader.GetInt32(reader.GetOrdinal("MarkaID")),
                    reader.GetString(reader.GetOrdinal("MarkaNaziv")),
                    reader.GetString(reader.GetOrdinal("MarkaDrzava"))
                );

                var model = new ModelAutomobila(
                    reader.GetInt32(reader.GetOrdinal("ModelID")),
                    reader.GetInt32(reader.GetOrdinal("MarkaID")),
                    reader.GetString(reader.GetOrdinal("NazivModela"))
                );


                return new Automobil(
                    reader.GetInt32(reader.GetOrdinal("ID")),
                    marka,
                    model,
                    reader.GetInt32(reader.GetOrdinal("Godiste")),
                    reader.GetInt32(reader.GetOrdinal("Snaga")),
               (Pogon)reader.GetInt32(reader.GetOrdinal("PogonID")),
            reader.GetBoolean(reader.GetOrdinal("Deleted"))
                );
            });

            return result.FirstOrDefault();
        }



        public void AddCar(Automobil car)
        {
            int markaID = GetMarkaID(car.Marka.Naziv, car.Marka.DrzavaNastanka);
            int modelID = GetModelID(car.Model.NazivModela, markaID);
            int pogonID = (int)car.Pogon;


            var query = @"
            INSERT INTO Automobil (MarkaID, ModelID, Godiste, Snaga, PogonID, Deleted) 
            VALUES (@MarkaID, @ModelID, @Godiste, @Snaga, @PogonID, @Deleted);" +
            "SELECT SCOPE_IDENTITY();";

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MarkaID", markaID);
                    command.Parameters.AddWithValue("@ModelID", modelID);
                    command.Parameters.AddWithValue("@Godiste", car.Godiste);
                    command.Parameters.AddWithValue("@Snaga", car.Snaga);
                    command.Parameters.AddWithValue("@PogonID", pogonID);
                    command.Parameters.AddWithValue("@Deleted", car.Deleted);

                    var result = command.ExecuteScalar();
                    car.ID = Convert.ToInt32(result);
                }
            }
        }


        public void EditCar(string id, Automobil car)
        {
            int markaID = GetMarkaID(car.Marka.Naziv, car.Marka.DrzavaNastanka);
            int modelID = GetModelID(car.Model.NazivModela, markaID);
            int pogonID = (int)car.Pogon;

            var query = @"
            UPDATE Automobil
            SET MarkaID = @MarkaID, ModelID = @ModelID, Godiste = @Godiste, Snaga = @Snaga, PogonID = @PogonID, Deleted = @Deleted
            WHERE ID = @ID";

            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MarkaID", markaID);
                    command.Parameters.AddWithValue("@ModelID", modelID);
                    command.Parameters.AddWithValue("@Godiste", car.Godiste);
                    command.Parameters.AddWithValue("@Snaga", car.Snaga);
                    command.Parameters.AddWithValue("@PogonID", pogonID);
                    command.Parameters.AddWithValue("@Deleted", car.Deleted);
                    command.Parameters.AddWithValue("@ID", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }




        public void DeleteCar(int id)
        {
            var query = "UPDATE Automobil SET Deleted = 1 WHERE ID = @ID";
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }
            }
        }


        // public string GetNextCarId()
        // {
        //     var cars = GetAllCars();
        //    int maxId = cars.Any() ? cars.Max(car => int.Parse(car.ID)) : 0;
        //    return (maxId + 1).ToString();
        // }

        public int GetMarkaID(string nazivMarke, string drzavaNastanka)
        {

            int markaID = 0;
            var queryCheck = "SELECT ID FROM MarkaAutomobila WHERE Naziv = @Naziv AND DrzavaNastanka = @DrzavaNastanka";
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new SqlCommand(queryCheck, connection))
                {
                    command.Parameters.AddWithValue("@Naziv", nazivMarke);
                    command.Parameters.AddWithValue("@DrzavaNastanka", drzavaNastanka);
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        markaID = Convert.ToInt32(result);
                    }
                }
            }

            if (markaID == 0)
            {
                var queryInsert = "INSERT INTO MarkaAutomobila (Naziv, DrzavaNastanka) VALUES (@Naziv, @DrzavaNastanka); SELECT SCOPE_IDENTITY();";
                using (var connection = new SqlConnection(CONNECTION_STRING))
                {
                    connection.Open();
                    using (var command = new SqlCommand(queryInsert, connection))
                    {
                        command.Parameters.AddWithValue("@Naziv", nazivMarke);
                        command.Parameters.AddWithValue("@DrzavaNastanka", drzavaNastanka);
                        markaID = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }

            return markaID;
        }


        public int GetModelID(string nazivModela, int markaID)
        {

            int modelID = 0;
            var queryCheck = "SELECT ID FROM ModelAutomobila WHERE NazivModela = @NazivModela AND MarkaID = @MarkaID";
            using (var connection = new SqlConnection(CONNECTION_STRING))
            {
                connection.Open();
                using (var command = new SqlCommand(queryCheck, connection))
                {
                    command.Parameters.AddWithValue("@NazivModela", nazivModela);
                    command.Parameters.AddWithValue("@MarkaID", markaID);
                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        modelID = Convert.ToInt32(result);
                    }
                }
            }


            if (modelID == 0)
            {
                var queryInsert = "INSERT INTO ModelAutomobila (NazivModela, MarkaID) VALUES (@NazivModela, @MarkaID); SELECT SCOPE_IDENTITY();";
                using (var connection = new SqlConnection(CONNECTION_STRING))
                {
                    connection.Open();
                    using (var command = new SqlCommand(queryInsert, connection))
                    {
                        command.Parameters.AddWithValue("@NazivModela", nazivModela);
                        command.Parameters.AddWithValue("@MarkaID", markaID);
                        modelID = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }

            return modelID;
        }


    }
}