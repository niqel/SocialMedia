using Microsoft.Extensions.Options;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly PaginationOptions paginationOptions;
        public PostService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> options)
        {
            this.unitOfWork = unitOfWork;
            this.paginationOptions = options.Value;
        }

        public PagedList<Post> GetPosts(PostQueryFilter filters)
        {
            filters.PageNumber = filters.PageNumber == 0 ? paginationOptions.DefaultPageNumber : filters.PageNumber;
            filters.PageSize = filters.PageSize == 0 ? paginationOptions.DefaultPageSize : filters.PageSize;

            var posts =  this.unitOfWork.PostRepository.GetAll();

            if (filters.UserId != null)
            {
                posts = posts.Where(x => x.UserId == filters.UserId);
            }

            if (filters.Date != null)
            {
                posts = posts.Where(x => x.Date.ToShortDateString() == filters.Date?.ToShortDateString());
            }

            if (filters.Description != null)
            {
                posts = posts.Where(x => x.Description.ToLower().Contains(filters.Description.ToLower()));
            }

            var pagesdPost = PagedList<Post>.Create(posts, filters.PageNumber, filters.PageSize);

            return pagesdPost;
        }

        public async Task CreatePost(Post post)
        {
            var user = await this.unitOfWork.UserRepository.GetById(post.UserId);
            if (user == null)
            {
                throw new BusinessException("El usuario no existe");
            }

            var userPost = await this.unitOfWork.PostRepository.GetPostsByUser(post.UserId);

            if (userPost.Count() < 10)
            {
                var lastPost = userPost.OrderByDescending(x => x.Date).FirstOrDefault();
                if ((DateTime.Now - lastPost.Date).TotalDays < 7)
                {
                    throw new BusinessException("you are not able to publish the post");
                }
            }

            if (post.Description.Contains("Sexo"))
            {
                throw new BusinessException("Contenido no permitido");
            }

            await this.unitOfWork.PostRepository.Add(post);
            await this.unitOfWork.SaveChangesAsync();
        }

        public async Task<Post> GetPost(int id)
        {
            return await this.unitOfWork.PostRepository.GetById(id);
        }

        public async Task<bool> DeletePost(int id)
        {
            await this.unitOfWork.PostRepository.Delete(id);
            return true;
        }

        public async Task<bool> UpdatePost(Post post)
        {
            this.unitOfWork.PostRepository.Update(post);
            await this.unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
