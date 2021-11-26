using AutoMapper;
using Business.Abstractions.Services;
using Business.Dtos;
using Business.Logging;
using Business.Models;
using Business.Validations;
using DataAccess.Abstractions;
using Domain.Entities.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.ServiceImplementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public ServiceResult<IEnumerable<WatchItemDto>> GetUserWatchlist(Guid userId)
        {
            try
            {
            bool userExists = _unitOfWork.UserRepository.Any(user => user.Id == userId);

            if (!userExists)
            {
                return ServiceResult<IEnumerable<WatchItemDto>>.NotFound($"No user with id = '{userId}' found in the database");
            }

            var watchList = _unitOfWork.WatchItemRepository.Get(w => w.UserId == userId).ToList();
            var result = _mapper.Map<IEnumerable<WatchItemDto>>(watchList);

            return ServiceResult<IEnumerable<WatchItemDto>>.Ok(result);
            }
            catch (Exception ex)
            {
                LogHelper.LogExceptionStackTrace(ex);
                return ServiceResult<IEnumerable<WatchItemDto>>.Error(ex);
            }
        }

        public ServiceResult<IEnumerable<UserDto>> GetUsers()
        {
            try
            {
                var dbUsers = _unitOfWork.UserRepository.GetAll();

                var result = _mapper.Map<IEnumerable<UserDto>>(dbUsers);

                return ServiceResult<IEnumerable<UserDto>>.Ok(result);
            }
            catch (Exception ex)
            {
                LogHelper.LogExceptionStackTrace(ex);
                return ServiceResult<IEnumerable<UserDto>>.Error(ex);
            }
        }

        public ServiceResult<Guid> NewUser(string name, string surname, string email)
        {
            try
            {
                var newUser = new UserDto()
                {
                    Name = name,
                    Surname = surname,
                    Email = email
                };

                UserValidator userValidator = new UserValidator();
                var validationResult = userValidator.Validate(newUser);

                if (!validationResult.IsValid)
                {
                    return ServiceResult<Guid>.Error($"Validation error: '{string.Join(Environment.NewLine, validationResult.Errors)}'");
                }

                var dbResult = _mapper.Map<User>(newUser);
                _unitOfWork.UserRepository.Add(dbResult);
                _unitOfWork.Commit();

                return ServiceResult<Guid>.Ok(dbResult.Id, $"New user with id = '{dbResult.Id}' has been created.");
            }
            catch (Exception ex)
            {
                LogHelper.LogExceptionStackTrace(ex);
                return ServiceResult<Guid>.Error(ex);
            }
        }
    }
}
