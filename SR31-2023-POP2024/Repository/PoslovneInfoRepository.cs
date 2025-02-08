using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SR31_2023_POP2024.Model;


namespace SR31_2023_POP2024.Repository
{


    public class PoslovneInfoRepository : BaseRepository
    {
        public void AddPoslovneInfo(PoslovneInfo poslovneInfo)
        {
            var getUserIdQuery = "SELECT ID FROM Korisnik WHERE JMBG = @JMBG";
            int prodavacID = 0;

            using (var connection = GetConnection())
            {
                connection.Open();

                var command = new SqlCommand(getUserIdQuery, connection);
                command.Parameters.AddWithValue("@JMBG", poslovneInfo.Prodavac.JMBG);

                var result = command.ExecuteScalar();
                if (result != null)
                {
                    prodavacID = Convert.ToInt32(result);
                }
                else
                {
                    throw new Exception("Korisnik sa datim JMBG-om nije pronađen.");
                }
            }

            var query = "INSERT INTO PoslovneInfo (CenaNabavke, DatumNabavke, Prodato, CenaProdaje, DatumProdaje, ProdavacID, AutomobilID) " +
                        "VALUES (@CenaNabavke, @DatumNabavke, @Prodato, @CenaProdaje, @DatumProdaje, @ProdavacID, @AutomobilID)";

            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@CenaNabavke", poslovneInfo.CenaNabavke);
                command.Parameters.AddWithValue("@DatumNabavke", poslovneInfo.DatumNabavke);
                command.Parameters.AddWithValue("@Prodato", poslovneInfo.Prodato);
                command.Parameters.AddWithValue("@CenaProdaje", poslovneInfo.CenaProdaje);
                command.Parameters.AddWithValue("@DatumProdaje", poslovneInfo.DatumProdaje ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ProdavacID", prodavacID);
                command.Parameters.AddWithValue("@AutomobilID", poslovneInfo.Automobil.ID);

                command.ExecuteNonQuery();
            }
        }


        public List<PoslovneInfo> GetAllPoslovneInfo()
        {
            string query = @"
            SELECT 
                p.ID, p.CenaNabavke, p.DatumNabavke, p.Prodato, p.CenaProdaje, p.DatumProdaje, p.ProdavacID,p.KupacID, p.AutomobilID,
                k.Ime AS Ime, k.Prezime AS Prezime, k.JMBG AS JMBG, kupac.Ime AS Ime, kupac.Prezime AS Prezime, kupac.BrojLicneKarte AS BrojLicneKarta,
                a.ID AS AutomobilID, a.MarkaID, a.ModelID, a.Godiste, a.Snaga, a.PogonID
            FROM 
                PoslovneInfo p
            INNER JOIN 
                Korisnik k ON p.ProdavacID = k.ID
            LEFT JOIN 
                Kupcac kupac ON p.KupacID = kupac.ID
            INNER JOIN 
                Automobil a ON p.AutomobilID = a.ID
            WHERE 
                a.Deleted = 0";

            return ExecuteQuery(query, reader =>
            {
                var prodavac = new Korisnik(
                    reader.GetString(reader.GetOrdinal("ProdavacIme")),
                    reader.GetString(reader.GetOrdinal("ProdavacPrezime")),
                    "",
                    "",
                    reader.GetString(reader.GetOrdinal("ProdavacJMBG")),
                    0,
                    false
                );

                var kupac = reader.IsDBNull(reader.GetOrdinal("KupacIme")) ? null : new Kupac(
                    reader.GetString(reader.GetOrdinal("KupacIme")),
                    reader.GetString(reader.GetOrdinal("KupacPrezime")),
                    reader.GetString(reader.GetOrdinal("KupacLicnaKarta"))
                );

                var automobil = new Automobil(
                 reader.GetInt32(reader.GetOrdinal("AutomobilID")),
                 new MarkaAutomobila(
                     reader.GetInt32(reader.GetOrdinal("MarkaID")),
                     reader.GetString(reader.GetOrdinal("MarkaNaziv")),
                     reader.GetString(reader.GetOrdinal("MarkaDrzava"))
                 ),
                 new ModelAutomobila(
                     reader.GetInt32(reader.GetOrdinal("ModelID")),
                     reader.GetInt32(reader.GetOrdinal("MarkaID")),
                     ""
                 ),
                 reader.GetInt32(reader.GetOrdinal("Godiste")),
                 reader.GetInt32(reader.GetOrdinal("Snaga")),
                 (Pogon)reader.GetInt32(reader.GetOrdinal("PogonID")),
                 false // Deleted
             );

                return new PoslovneInfo(
                    reader.GetDecimal(reader.GetOrdinal("CenaNabavke")),
                    reader.GetDateTime(reader.GetOrdinal("DatumNabavke")),
                    reader.GetBoolean(reader.GetOrdinal("Prodato")),
                    reader.GetDecimal(reader.GetOrdinal("CenaProdaje")),
                    reader.IsDBNull(reader.GetOrdinal("DatumProdaje")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DatumProdaje")),
                    prodavac,
                    kupac,
                    automobil
                );
            });
        }

        public PoslovneInfo? GetPoslovneInfo(int automobilId)
        {
            string query = @"
        SELECT 
            p.ID, p.CenaNabavke, p.DatumNabavke, p.Prodato, p.CenaProdaje, p.DatumProdaje, p.ProdavacID, p.KupacID, p.AutomobilID,
            k.Ime AS Ime, k.Prezime AS Prezime, k.JMBG AS JMBG, kupac.Ime AS KupacIme, kupac.Prezime AS KupacPrezime, kupac.BrojLicneKarte AS KupacLicnaKarta,
            a.ID AS AutomobilID, a.MarkaID, a.ModelID, a.Godiste, a.Snaga, a.PogonID,
            m.Naziv AS MarkaNaziv,  
            m.DrzavaNastanka AS MarkaDrzava 
        FROM 
            PoslovneInfo p
        INNER JOIN 
            Korisnik k ON p.ProdavacID = k.ID
        LEFT JOIN 
            Kupac kupac ON p.KupacID = kupac.ID
        INNER JOIN 
            Automobil a ON p.AutomobilID = a.ID
        INNER JOIN 
            MarkaAutomobila m ON a.MarkaID = m.ID 
        WHERE 
            p.AutomobilID = @AutomobilID;";

            using (var connection = GetConnection())
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AutomobilID", automobilId);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var prodavac = new Korisnik(
                            reader.GetString(reader.GetOrdinal("Ime")),
                            reader.GetString(reader.GetOrdinal("Prezime")),
                            "",
                            "",
                            reader.GetString(reader.GetOrdinal("JMBG")),
                            0,
                            false
                        );

                        var kupac = reader.IsDBNull(reader.GetOrdinal("KupacIme")) ? null : new Kupac(
                            reader.GetString(reader.GetOrdinal("KupacIme")),
                            reader.GetString(reader.GetOrdinal("KupacPrezime")),
                            reader.GetString(reader.GetOrdinal("KupacLicnaKarta"))
                        );

                        var automobil = new Automobil(
                            reader.GetInt32(reader.GetOrdinal("AutomobilID")),
                            new MarkaAutomobila(
                                reader.GetInt32(reader.GetOrdinal("MarkaID")),
                                reader.GetString(reader.GetOrdinal("MarkaNaziv")),
                                reader.GetString(reader.GetOrdinal("MarkaDrzava"))
                            ),
                            new ModelAutomobila(
                                reader.GetInt32(reader.GetOrdinal("ModelID")),
                                reader.GetInt32(reader.GetOrdinal("MarkaID")),
                                ""
                            ),
                            reader.GetInt32(reader.GetOrdinal("Godiste")),
                            reader.GetInt32(reader.GetOrdinal("Snaga")),
                            (Pogon)reader.GetInt32(reader.GetOrdinal("PogonID")),
                            false
                        );

                        return new PoslovneInfo(
                            reader.GetDecimal(reader.GetOrdinal("CenaNabavke")),
                            reader.GetDateTime(reader.GetOrdinal("DatumNabavke")),
                            reader.GetBoolean(reader.GetOrdinal("Prodato")),
                            reader.GetDecimal(reader.GetOrdinal("CenaProdaje")),
                            reader.IsDBNull(reader.GetOrdinal("DatumProdaje")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DatumProdaje")),
                            prodavac,
                            kupac,
                            automobil
                        );
                    }
                }
            }

            return null;
        }
        public void UpdatePoslovneInfo(PoslovneInfo poslovneInfo)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();

                int kupacID;
                string checkKupacQuery = "SELECT ID FROM Kupac WHERE BrojLicneKarte = @BrojLicneKarte";

                using (SqlCommand checkCommand = new SqlCommand(checkKupacQuery, conn))
                {
                    checkCommand.Parameters.AddWithValue("@BrojLicneKarte", poslovneInfo.Kupac.BrojLicneKarte);
                    var result = checkCommand.ExecuteScalar();

                    if (result != null)
                    {
                        kupacID = Convert.ToInt32(result);
                    }
                    else
                    {
                        throw new Exception("Kupac sa unetim brojem lične karte ne postoji.");
                    }
                }

                string query = @"
                UPDATE PoslovneInfo
                SET 
                DatumProdaje = @DatumProdaje,
                Prodato = 1,
                CenaProdaje = @CenaProdaje,
                KupacID = @KupacID
                WHERE AutomobilID = @AutomobilID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CenaProdaje", poslovneInfo.CenaProdaje);
                    cmd.Parameters.AddWithValue("@DatumProdaje", poslovneInfo.DatumProdaje ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@KupacID", kupacID);
                    cmd.Parameters.AddWithValue("@AutomobilID", poslovneInfo.Automobil.ID);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }

}
