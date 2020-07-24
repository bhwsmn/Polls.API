using AutoMapper;
using Polls.API.Entities;
using Polls.API.Models;
using Polls.API.Models.Input;
using Polls.API.Models.Output;

namespace Polls.API.Profiles
{
    public class PollProfile : Profile
    {
        public PollProfile()
        {
            CreateMap<OptionInputDto, Option>();
            CreateMap<Option, OptionOutputDto>();
            CreateMap<PollInputDto, Poll>();
            CreateMap<Poll, PollOutputDto>();
        }
    }
}