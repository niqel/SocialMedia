using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Infrastructure.Services
{
    public class UriService : IUriService
    {
        private readonly string baseUri;

        public UriService(string baseUri)
        {
            this.baseUri = baseUri; 
        }

        public Uri GetPostPaginationUri(PostQueryFilter filter, string actionUrl)
        {
            string url = $"{this.baseUri}{actionUrl}";
            return new Uri(url);
        }
    }
}
