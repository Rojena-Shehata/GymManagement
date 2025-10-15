using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class GymDataSeeding
    {
        public static bool SeedData(GymDbContext context)
        {
            try
            {
                if (!context.Categories.Any())
                {
                    var categories= LoadDataFromJsonFile<Category>("categories.json");

                    context.AddRange(categories);                   
                } 
                if(!context.Plans.Any())
                {
                    var plans = LoadDataFromJsonFile<Plan>("plans.json");
                    context.AddRange(plans);
                }
               return context.SaveChanges()>0;  
            }
            catch(Exception ex)
            {
                return false; //could add logger????
            }
        }

        #region Helper Method
        private static List<T>LoadDataFromJsonFile<T>(string fileName)
        {
            var filePath=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\Files", fileName);

            if (!File.Exists(filePath)) 
                throw new FileNotFoundException(filePath);

            var jsonData=File.ReadAllText(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            options.Converters.Add(new JsonStringEnumConverter());

            return JsonSerializer.Deserialize<List<T>>(jsonData, options);
        }

        #endregion
    }
}
