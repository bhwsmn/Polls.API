using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Polls.API.DbContexts;
using Polls.API.Entities;

namespace Polls.API.Services
{
    public class PollsRepository : IPollsRepository, IDisposable
    {
        private readonly PollsContext _context;

        public PollsRepository(PollsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Poll>> GetAllPollsAsync()
        {
            var pollsList = await _context.Polls.ToListAsync();

            return pollsList;
        }

        public async Task<Poll> GetPollBySlugAsync(string slug)
        {
            var poll = await _context.Polls.FirstOrDefaultAsync(p => p.Slug == slug);

            return poll;
        }

        public async Task<bool> PollExistsAsync(string slug)
        {
            var pollExists = await _context.Polls.AnyAsync(p => p.Slug == slug);

            return pollExists;
        }

        public async Task<bool> VoteExistsAsync(string slug, string voterIp)
        {
            var poll = await _context.Polls.FirstOrDefaultAsync(p => p.Slug == slug);

            return poll.Votes.Any(v => v.IpAddress == voterIp);
        }

        public async Task<Poll> CreatePollAsync(Poll poll)
        {
            if (string.IsNullOrWhiteSpace(poll.Slug))
            {
                do
                {
                    poll.Slug = Guid.NewGuid().ToString();
                } while (await _context.Polls.AnyAsync(p => p.Slug == poll.Slug));
            }

            await _context.Polls.AddAsync(poll);

            return poll;
        }

        public async Task<Poll> VoteAsync(string slug, ICollection<Guid> optionsIdList, string voterIp)
        {
            var poll = await _context.Polls.FirstOrDefaultAsync(p => p.Slug == slug);

            if (!poll.Options.Select(o => o.Id).Intersect(optionsIdList).Any())
            {
                return poll;
            }

            foreach (var option in poll.Options)
            {
                if (optionsIdList.Contains(option.Id))
                {
                    option.Votes += 1;
                }
            }

            poll.TotalVotes += 1;
            poll.Votes.Add(new Vote
            {
                IpAddress = voterIp,
                Poll = poll
            });

            return poll;
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }
        
        public void Dispose()
        {
            Save();
            _context?.Dispose();
        }
    }
}