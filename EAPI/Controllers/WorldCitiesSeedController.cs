using EAPI.Data;
using EAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Security;

namespace EAPI.Controllers
{
    [Route("api/WorldCitiesSeed")]
    [ApiController]
    public class WorldCitiesSeedController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment env;

        public WorldCitiesSeedController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }


        [HttpGet]
        public async Task<ActionResult> Import()
        {
            if (!env.IsDevelopment())
                throw new SecurityException("Not Allowed");


            var path = Path.Combine(env.ContentRootPath, "Data/Source/worldcities.xlsx");
            using var stream = System.IO.File.OpenRead(path);

            using var excelPackage = new ExcelPackage(stream);
            var worksheet = excelPackage.Workbook.Worksheets[0];
            var nEndRow = worksheet.Dimension.End.Row;

            var numberOfCountriesAdded = 0;
            var numberOfCitiesAdded = 0;

            // Countries
            var countriesByName = context.Countries.AsNoTracking()
                .ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

            for (int nRow = 2; nRow <= nEndRow; nRow++)
            {
                var row = worksheet.Cells[nRow, 1, nRow, worksheet.Dimension.End.Column];
                var countryName = row[nRow, 5].GetValue<string>();
                var iso2 = row[nRow, 6].GetValue<string>();
                var iso3 = row[nRow, 7].GetValue<string>();

                if (countriesByName.ContainsKey(countryName))
                    continue;

                var country = new Country
                {
                    Name = countryName,
                    ISO2 = iso2,
                    ISO3 = iso3
                };

                await context.Countries.AddAsync(country);
                countriesByName.Add(countryName, country);

                numberOfCountriesAdded++;
            }

            if (numberOfCountriesAdded > 0)
                await context.SaveChangesAsync();

            // Cities

            var cities = context.Cities.AsNoTracking().ToDictionary(x => (
                Name: x.Name,
                cityLat: x.Lat,
                cityLon: x.Lon,
                CityCountryId: x.CountryId
                ));

            for (int nRow = 2; nRow <= nEndRow; nRow++)
            {
                var row = worksheet.Cells[nRow, 1, nRow, worksheet.Dimension.End.Column];

                var name = row[nRow, 1].GetValue<string>();
                var nameAscii = row[nRow, 2].GetValue<string>();
                var lat = row[nRow, 3].GetValue<decimal>();
                var lon = row[nRow, 4].GetValue<decimal>();
                var countryName = row[nRow, 5].GetValue<string>();

                var countryId = countriesByName[countryName].Id;

                if (cities.ContainsKey((
                    Name: name, cityLat: lat, cityLon: lon, CityCountryId: countryId)))
                    continue;

                var city = new City
                {
                    Name = name,
                    Lat = lat,
                    Lon = lon,
                    CountryId = countryId
                };

                context.Cities.Add(city);

                numberOfCitiesAdded++;
            }

            if (numberOfCitiesAdded > 0)
                await context.SaveChangesAsync();


            // Return

            return new JsonResult(new
            {
                Cities = numberOfCitiesAdded,
                Countries = numberOfCountriesAdded

            });


            //End
        }



    }
}
