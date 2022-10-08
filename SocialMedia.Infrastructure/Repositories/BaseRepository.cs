using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly SocialMediaContext context;
        protected readonly DbSet<T> entities;
        public BaseRepository(SocialMediaContext context)
        {
            this.context = context;
            this.entities = context.Set<T>();
        }
        public async Task Add(T entity)
        {
           await this.entities.AddAsync(entity);
        }

        public async Task Delete(int id)
        {
            T entity = await this.GetById(id);
            entities.Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }

        public async Task<T> GetById(int id)
        {
            return await entities.FindAsync(id);
        }

        public void Update(T entity)
        {
            this.entities.Update(entity);
        }
    }
}
