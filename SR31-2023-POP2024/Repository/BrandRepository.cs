using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR31_2023_POP2024.Repository
{
    public class BrandRepository : BaseRepository
    {
        public void PersistBrands(List<MarkaAutomobila> brands)
        {
            using (var streamWriter = new StreamWriter(BRANDS_LOCATION, false))
            {
                foreach (var brand in brands)
                {
                    streamWriter.WriteLine($"{brand.Naziv},{brand.DrzavaNastanka}");
                }
            }
        }

        public List<MarkaAutomobila> GetAllBrands()
        {
            var brands = new List<MarkaAutomobila>();
            if (!File.Exists(BRANDS_LOCATION)) return brands;

            using (var streamReader = new StreamReader(BRANDS_LOCATION))
            {
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var lineParts = line.Split(",");
                    var marka = new MarkaAutomobila(lineParts[0], lineParts[1]);
                    brands.Add(marka);
                }
            }
            return brands;
        }

        public MarkaAutomobila? GetBrand(string naziv)
        {
            return GetAllBrands().Find(brand => brand.Naziv == naziv);
        }

        public void AddBrand(MarkaAutomobila brand)
        {
            var brands = GetAllBrands();
            if (brands.All(b => b.Naziv != brand.Naziv))
            {
                brands.Add(brand);
                PersistBrands(brands);
            }
        }
    }
}
