﻿using CinemaApp.Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Data.Repository
{
    public class BaseRepository<TType, TId> : IRepository<TType, TId> where TType : class
    {
        private readonly CinemaDbContext dbContext;
        private readonly DbSet<TType> dbSet;

        public BaseRepository(CinemaDbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = this.dbContext.Set<TType>();
        }
        public void Add(TType item)
        {
            dbSet.Add(item);
            dbContext.SaveChanges();
        }

        public async Task AddAsync(TType item)
        {
            await dbSet.AddAsync(item);
            await dbContext.SaveChangesAsync();
        }

		public void AddRange(TType[] items)
		{
            this.dbSet.AddRange(items);
            this.dbContext.SaveChanges();
		}

		public async Task AddRangeAsync(TType[] items)
		{
			await this.dbSet.AddRangeAsync(items);
            await this.dbContext.AddRangeAsync(items);

		}

		public bool Delete(TId id)
        {
            TType entity = GetById(id);
            if (entity == null)
            {
                return false;
            }
            dbSet.Remove(entity);
            dbContext.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteAsync(TId id)
        {
            TType entity = await GetByIdAsync(id);
            if (entity == null)
            {
                return false;
            }
            dbSet.Remove(entity);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public IEnumerable<TType> GetAll()
        {
            return dbSet.ToArray();
        }

        public async Task<IEnumerable<TType>> GetAllAsync()
        {
            return await dbSet.ToArrayAsync();
        }


        public IQueryable<TType> GetAllAttached()
        {
            return dbSet.AsQueryable();
        }

        public TType GetById(TId id)
        {
            TType entity = dbSet
               .Find(id);

            return entity;
        }

        public async Task<TType> GetByIdAsync(TId id)
        {
            TType entity = await dbSet
               .FindAsync(id);

            return entity;
        }

		public async Task<TType> GetByIdAsync(params TId[] id)
		{
			TType entity = await dbSet
              .FindAsync(id[0], id[1]);

			return entity;
		}

		public bool Update(TType item)
        {
            try
            {
                dbSet.Attach(item);
                dbContext.Entry(item)
                    .State = EntityState.Modified;
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(TType item)
        {
            try
            {
                dbSet.Attach(item);
                dbContext.Entry(item)
                    .State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
