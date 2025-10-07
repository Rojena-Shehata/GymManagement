using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class HealthRecordRepository:IHealthRecordRepository
    {
        private readonly GymDbContext _Context;

        public HealthRecordRepository(GymDbContext context)
        {
            _Context = context;
        }

        public int Add(HealthRecord healthRecord)
        {
            if (healthRecord is null)
                return 0;
            _Context.HealthRecords.Add(healthRecord);
            return _Context.SaveChanges();
        }

        public int Delete(int id)
        {
            HealthRecord healthRecord = GetById(id);
            if (healthRecord is null)
                return 0;

            _Context.Remove(healthRecord);
            return _Context.SaveChanges();

        }

        public IEnumerable<HealthRecord> GetAll()=> _Context.HealthRecords.ToList();



        public HealthRecord? GetById(int id) => _Context.HealthRecords.Find(id);
        
        

        public int Update(HealthRecord healthRecord)
        {
            if (healthRecord is null)
                return 0;
            _Context.Update(healthRecord);
            return _Context.SaveChanges();

        }
    }
}
