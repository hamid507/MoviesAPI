using Business.Dtos;
using Business.Models;
using System;
using System.Collections.Generic;

namespace Business.Abstractions.Services
{
    public interface IUserService
    {
        ServiceResult<IEnumerable<WatchItemDto>> GetUserWatchlist(Guid userId);
        ServiceResult<IEnumerable<UserDto>> GetUsers();
        ServiceResult<Guid> NewUser(string name, string surname, string email);
    }
}
