using Donator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Data.Repos
{
    public interface IRequestForVolunteersRepo
    {
        Task<RequestForVolunteer> GetRequestForVolunteerById(int id);
        Task<RequestForVolunteer> GetRequestByEventName(string eventName);
        Task<List<RequestForVolunteer>> GetRequestForVolunteersByDate(DateTime date);
        Task<List<RequestForVolunteer>> GetRequestForVolunteersByNPOId(int npoId);
        Task<RequestForVolunteer> CreateRequestForVolunteer(RequestForVolunteer request);
        Task<bool> UpdateRequestForVolunteer(RequestForVolunteer request);
        Task<bool> DeleteRequestForVolunteer(int id);
        Task<bool> SaveAll();

    }
}
