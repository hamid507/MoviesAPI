using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstractions.Services
{
    public interface ISchedulerService
    {
        void CheckWatchlistForEmails();
    }
}
