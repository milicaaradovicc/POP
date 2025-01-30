using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

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

    }

}
