using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Polls.API.Entities;

namespace Polls.API.Services
{
    public interface IPollsRepository
    {
        Task<IEnumerable<Poll>> GetAllPollsAsync();
        Task<Poll> GetPollBySlugAsync(string slug);
        Task<bool> PollExistsAsync(string slug);

        Task<bool> VoteExistsAsync(string slug, string voterIp);
        Task<Poll> CreatePollAsync(Poll poll);
        Task<Poll> VoteAsync(string slug, ICollection<Guid> optionsIdList, string voterIp);
        bool Save();
    }
}