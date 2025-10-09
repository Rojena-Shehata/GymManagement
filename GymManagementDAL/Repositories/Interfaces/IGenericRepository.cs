using GymManagementDAL.Entities;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity? GetById(int id);
        TEntity? GetById(Expression<Func<TEntity,bool>> condition);
        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? condition = null);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> range);
        bool Any(Expression<Func<TEntity, bool>> condition);
    }
}
