using AutoMapper;
using SipayTestCase.Contract.Dtos;
using SipayTestCase.Domain.Entities;

namespace SipayTestCase.Application.Mapping;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
    }
}