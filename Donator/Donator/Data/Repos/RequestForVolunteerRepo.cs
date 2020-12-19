using Donator.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Donator.Data.Repos
{
    public class RequestForVolunteerRepo : IRequestForVolunteersRepo
    {
        public readonly AppDbContext _dbContext;

        public RequestForVolunteerRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RequestForVolunteer> CreateRequestForVolunteer(RequestForVolunteer request)
        {
            request.CreatedOn = DateTime.Now;
            request.LastUpdatedOn = DateTime.Now;
            _dbContext.Add(request);
            await _dbContext.SaveChangesAsync();
            return request;
        }

        public async Task<bool> DeleteRequestForVolunteer(int id)
        {
            var request = await _dbContext.RequestForVolunteers.FirstOrDefaultAsync(c => c.Id == id);

            _dbContext.Remove(request);

            return (await _dbContext.SaveChangesAsync() > 0 ? true : false);
        }

        public async Task<RequestForVolunteer> GetRequestByEventName(string eventName)
        {
            return await _dbContext.RequestForVolunteers.FirstOrDefaultAsync(x => x.EventName == eventName);
        }

        public async Task<RequestForVolunteer> GetRequestForVolunteerById(int id)
        {
            return await _dbContext.RequestForVolunteers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<RequestForVolunteer>> GetRequestForVolunteersByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task<List<RequestForVolunteer>> GetRequestForVolunteersByNPOId(int npoId)
        {
            return await _dbContext.RequestForVolunteers.Where(x => x.NPOId == npoId).ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateRequestForVolunteer(RequestForVolunteer request)
        {
            request.LastUpdatedOn = DateTime.Now;
            _dbContext.Entry(request).Property(x => x.EventName).IsModified = true;
            _dbContext.Entry(request).Property(x => x.Date).IsModified = true;
            _dbContext.Entry(request).Property(x => x.Location).IsModified = true;
            _dbContext.Entry(request).Property(x => x.FromTime).IsModified = true;
            _dbContext.Entry(request).Property(x => x.ToTime).IsModified = true;
            _dbContext.Entry(request).Property(x => x.WaiverRequired).IsModified = true;
            _dbContext.Entry(request).Property(x => x.Status).IsModified = true;
            return (await _dbContext.SaveChangesAsync() > 0 ? true : false);
        }
    }
}
