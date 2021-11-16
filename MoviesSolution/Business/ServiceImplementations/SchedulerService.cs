using AutoMapper;
using Business.Abstractions.Services;
using Business.Dtos;
using Business.Utility;
using DataAccess.Abstractions;
using Domain.Entities.Data;
using Infrastructure.Services;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Business.ServiceImplementations
{
    public class SchedulerService : ISchedulerService
    {
        IUnitOfWork _unitOfWork;
        IMapper _mapper;

        public SchedulerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void CheckWatchlistForEmails()
        {
            try
            {
                var notWatched = _unitOfWork.WatchItemRepository.Get(w =>
                !w.Watched &&
                (!w.LastEmailSendDate.HasValue || (w.LastEmailSendDate.Value.Year != DateTime.UtcNow.Year) && (w.LastEmailSendDate.Value.Month != DateTime.UtcNow.Month)));

                int minimumUnwatchedToSendEmail = 4;
                var enumerator = notWatched.GetEnumerator();
                bool hasEnoughToSendEmail = true;

                for (int i = 0; i < minimumUnwatchedToSendEmail; i++)
                {
                    if (!enumerator.MoveNext())
                    {
                        hasEnoughToSendEmail = false;
                        break;
                    }
                }

                enumerator.Dispose();

                if (hasEnoughToSendEmail)
                {
                    var mostlyRated = notWatched.OrderByDescending(w => w.Movie.ImdbRating).First();

                    var user = _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Id == mostlyRated.UserId);

                    string from = AppSettings.GetProperty("SmtpMailFrom");
                    string to = user.Email;

                    var movie = _unitOfWork.MovieRepository.GetFirstOrDefault(m => m.Id == mostlyRated.MovieId);
                    var movieDto = _mapper.Map<MovieDto>(movie);

                    bool sendResult = MailHelper.SendMail(from, to, movieDto);

                    if (sendResult)
                    {
                        Expression<Func<WatchItem, bool>> predicate = (w => w.UserId == mostlyRated.UserId && w.MovieId == mostlyRated.MovieId);

                        if(_unitOfWork.WatchItemRepository.Any(predicate))
                        {
                            var item = _unitOfWork.WatchItemRepository.GetFirstOrDefault(predicate);

                            item.LastEmailSendDate = DateTime.UtcNow;
                            _unitOfWork.WatchItemRepository.Update(item);
                            _unitOfWork.Commit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: Log
            }
        }
    }
}
