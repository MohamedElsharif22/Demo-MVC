using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGerericRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;

        public GenericRepository(AppDbContext context) // DI For AppDbContext
        {
            _context = context;
        }

        public void Add(T entity)
        {
            if (entity is not null)
            {
                _context.Add(entity);
            }
        }

        public void Delete(T entity)
        {
            if (entity is not null)
            {
                _context.Remove(entity);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var Result = await _context.Set<T>().ToListAsync();
            return Result;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var Result = await _context.Set<T>().FindAsync(id);
            return Result;
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}