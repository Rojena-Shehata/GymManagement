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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _context;
        private readonly Dictionary<string,object> _repositories = [];

        public UnitOfWork(GymDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            var entityName=typeof(TEntity).Name;//Member
            if(_repositories.TryGetValue(entityName,out object? repository))
            {
                return (IGenericRepository<TEntity>)repository;
            }
            var newRepository=new GenericRepository<TEntity>(_context);
            _repositories.Add(entityName, newRepository);
            return newRepository;
        }

        public int SaveChanges()
                    =>_context.SaveChanges();
        
    }
}
