using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
       protected readonly GymDbContext _context;

        public GenericRepository(GymDbContext context)
        {
            _context = context;
        }

        public void Add(TEntity entity) =>_context.Add(entity);

        public bool Any(Expression<Func<TEntity, bool>> condition)
        {
            if(condition is  null)
                return false;
           return _context.Set<TEntity>().Any(condition);
        }
        

        public void Delete(TEntity entity) =>_context.Remove(entity);

        public void DeleteRange(IEnumerable<TEntity> range)
        {
            _context.RemoveRange(range);
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? condition = null)
        {
            if(condition is not null)
                return _context.Set<TEntity>().AsNoTracking().Where(condition).ToList();
            return _context.Set<TEntity>().AsNoTracking().ToList();
        }

        public TEntity? GetById(int id) =>_context.Set<TEntity>().Find(id);
        public TEntity? GetById(Expression<Func<TEntity,bool> >condition)
        {
            if (condition is null)
                return null;
          return  _context.Set<TEntity>().AsNoTracking().FirstOrDefault(condition);
        }

        public int GetCount(Func<TEntity, bool>? condition=null)
        {
            if (condition is null)
                return _context.Set<TEntity>().Count();
            return _context.Set<TEntity>().Count(condition);
        }



        //public void ExplicitLoading<Tproperty>(TEntity entity ,Expression<Func<TEntity,Tproperty?>> navigationProperty) where Tproperty : class
        //{
        //    _context.Entry(entity).Reference(navigationProperty).Load() ;
        //}
        public void Update(TEntity entity)
        {
            _context.Update(entity);
            
        }
    }
}
