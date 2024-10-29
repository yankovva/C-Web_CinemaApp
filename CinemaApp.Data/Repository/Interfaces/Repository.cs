using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Repository.Interfaces
{
    public class Repository<TType, TId> : IRepository<TType, TId> where TType : class
    {
        private readonly CinemaDbContext _dbContext;
        private readonly DbSet<TType> dbSet;

        public Repository(CinemaDbContext dbContext)
        {
                this._dbContext = dbContext;
               this.dbSet = this._dbContext.Set<TType>();
        }
        public void Add(TType item)
        {
            
        }

        public Task AddAsync(TType item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(TId id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(TId id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TType> GetAll()
        {
            return this.dbSet.All(); 
        }

        public Task<IEnumerable<TType>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public TType GetById(TId id)
        {
            TType entity = this.dbSet
               .Find(id);

            return entity;
        }

        public async Task<TType> GetByIdAsync(TId id)
        {
            TType entity = await  this.dbSet
               .FindAsync(id);

            return entity;
        }

        public bool SoftDelete(TId id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SoftDeleteAsync(TId id)
        {
            throw new NotImplementedException();
        }

        public bool Update(TType item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(TType item)
        {
            throw new NotImplementedException();
        }
    }
}
