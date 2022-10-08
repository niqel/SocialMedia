using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class UnityOfWork : IUnitOfWork
    {
        protected readonly SocialMediaContext context;
        private readonly IPostRepository postRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Comment> commentRepository;

        public IPostRepository PostRepository => this.postRepository ?? new PostRepository(this.context);
        public IRepository<User> UserRepository => this.userRepository ?? new BaseRepository<User>(this.context);
        public IRepository<Comment> CommentRepository => this.commentRepository ?? new BaseRepository<Comment>(this.context);

        public UnityOfWork(SocialMediaContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            if (this.context != null)
            {
                context.Dispose();
            }
        }

        public void SaveChanges()
        {
            this.context.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return this.context.SaveChangesAsync();
        }
    }
}
