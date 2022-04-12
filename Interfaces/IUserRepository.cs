
using System.Collections.Generic;
using System.Threading.Tasks;
using UserAPI.Models;
using UserAPI.DTOs;
using UserAPI.Utility;

namespace UserAPI.Interfaces
{
    public interface IUserRepository
    {
        public Task<PagedList<User>> GetAll(PagingParams pagingParams);

        public Task<User> GetById(int id);
        public Task<int> Create(CreateUserDTO createUserDTO);

        public Task<User> Update(GeneralUserDTO updateDTO, int id);

        public Task<User> Delete(int id);

        public Task<IEnumerable<User>> Search(GeneralUserDTO searchDTO);
        string QueryFormat(GeneralUserDTO GeneralDTO);
    }
}
