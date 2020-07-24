using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polls.API.Entities;
using Polls.API.Models.Input;
using Polls.API.Models.Output;
using Polls.API.Services;

namespace Polls.API.Controllers
{
    [ApiController]
    [Route("polls")]
    public class PollsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPollsRepository _pollsRepository;

        public PollsController(IPollsRepository pollsRepository, IMapper mapper)
        {
            _pollsRepository = pollsRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PollOutputDto>>> GetAllPollsAsync()
        {
            var pollsFromRepository = await _pollsRepository.GetAllPollsAsync();

            return _mapper.Map<List<PollOutputDto>>(pollsFromRepository);
        }

        [HttpHead("{slug}")]
        [HttpGet("{slug}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PollOutputDto>> GetPollBySlugAsync(string slug)
        {
            var pollFromRepository = await _pollsRepository.GetPollBySlugAsync(slug);

            if (pollFromRepository == null)
            {
                return NotFound();
            }

            return _mapper.Map<PollOutputDto>(pollFromRepository);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<PollOutputDto>> CreatePollAsync(PollInputDto pollInputDto)
        {
            if (await _pollsRepository.PollExistsAsync(pollInputDto.Slug))
            {
                return Conflict();
            }

            if (pollInputDto.Options.Count < 2)
            {
                return BadRequest("At least 2 options must be given.");
            }

            var createdPoll = await _pollsRepository.CreatePollAsync(_mapper.Map<Poll>(pollInputDto));

            return CreatedAtAction(
                actionName: nameof(GetPollBySlugAsync),
                routeValues: new {slug = createdPoll.Slug},
                value: _mapper.Map<PollOutputDto>(createdPoll)
            );
        }

        [HttpPut("{slug}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PollOutputDto>> VoteAsync(string slug, PollVoteInputDto pollVoteDto)
        {
            var pollFromRepository = await _pollsRepository.GetPollBySlugAsync(slug);
            var requestIp = Request.HttpContext.Request.Headers.GetOrDefault("X-Forwarded-For").ToString();

            if (pollFromRepository == null)
            {
                return NotFound();
            }

            if (requestIp == null || await _pollsRepository.VoteExistsAsync(slug, requestIp))
            {
                Console.WriteLine(requestIp);
                return StatusCode(403);
            }

            if (!pollFromRepository.MultiSelect && pollVoteDto.Votes.Count > 1)
            {
                return BadRequest("This poll does not allow selecting multiple options.");
            }

            var updatedPollFromRepository = await _pollsRepository.VoteAsync(
                slug: slug,
                optionsIdList: pollVoteDto.Votes,
                voterIp: requestIp
            );

            return _mapper.Map<PollOutputDto>(updatedPollFromRepository);
        }
    }
}