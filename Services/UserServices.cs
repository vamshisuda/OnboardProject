using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UserAPI.Interfaces;
using UserAPI.DTOs;
using UserAPI.Utility;
using UserAPI.Models;

namespace UserAPI.Services
{
    public class UserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IUrlHelper _urlHelper;

        public UserServices( IUserRepository userRepository, IUrlHelper urlHelper)
        {
            _userRepository = userRepository;
            _urlHelper = urlHelper;
        }

        public async Task<UserOutputModel> GetpagedData(int PageSize, int PageNumber)
        {
            try
            {
                PagingParams pagingParams = new PagingParams();
                pagingParams.PageSize = PageSize > 0 ? PageSize : 10;
                pagingParams.PageNumber = PageNumber > 0 ? PageNumber : 1;
                var Data = await _userRepository.GetAll(pagingParams);
                var outputModel = new UserOutputModel
                {
                    Paging = Data.GetHeader(),
                    Links = GetLinks(Data),
                    Items = Data.List
                };
                return outputModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }
        private List<LinkInfo> GetLinks(PagedList<User> list)
        {
            var links = new List<LinkInfo>();

            if (list.HasPreviousPage)
                links.Add(CreateLink("", list.PreviousPageNumber, list.PageSize, "previousPage", "GET"));

            links.Add(CreateLink("", list.PageNumber, list.PageSize, "self", "GET"));

            if (list.HasNextPage)
                links.Add(CreateLink("", list.NextPageNumber, list.PageSize, "nextPage", "GET"));

            return links;
        }

        private LinkInfo CreateLink(
            string routeName, int pageNumber, int pageSize,
            string rel, string method)
        {
            return new LinkInfo
            {
                Href = _urlHelper.Action(routeName,
                            new { PageNumber = pageNumber, PageSize = pageSize }),
                Rel = rel,
                Method = method
            };
        }

        
    }
}
