﻿using AutoMapper;
using SuccessAppraiser.BLL.Auth.Contracts;

namespace Api.Auth.Contracts
{
    public class AuthApiMapper : Profile
    {
        public AuthApiMapper()
        {
            CreateMap<RegisterDto, RegisterCommand>().ReverseMap();
        }
    }
}