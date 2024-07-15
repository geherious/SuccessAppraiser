﻿using Api.Goal.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.BLL.Goal.Services.Interfaces;
using SuccessAppraiser.Data.Entities;
using System;
using System.Security.Claims;
using Api.Filters;
using FluentValidation;

namespace SuccessAppraiser.Controllers.Goal
{
    [ApiController]
    [Authorize]
    [ValidationExceptionFilter]
    public class GoalController : ControllerBase
    {
        private readonly IGoalService _goalService;
        private readonly IGoalDateService _goalDateService;
        private readonly IMapper _mapper;

        public GoalController(IGoalService goalService, IGoalDateService goalDateService, IMapper mapper)
        {
            _goalService = goalService;
            _goalDateService = goalDateService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("goals")]
        public async Task<IActionResult> Goals(CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var items = await _goalService.GetGoalsByUserIdAsync(userId, ct);

            return Ok(_mapper.Map<List<GetUserGoalDto>>(items));
        }

        [HttpPost]
        [DtoValidationFilter]
        [Route("goals")]
        public async Task<IActionResult> Goals([FromBody] CreateGoalDto goalDto, CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var command = _mapper.Map<CreateGoalCommand>(goalDto);
            command.UserId = userId;

            GoalItem newGoal = await _goalService.CreateGoalAsync(command, ct);

            var result = _mapper.Map<GetUserGoalDto>(newGoal);
            return Ok(result);
        }

        [HttpPost]
        [DtoValidationFilter]
        [Route("dates")]
        public async Task<IActionResult> GoalDate([FromBody] CreateGoalDateDto goalDateDto, CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _goalService.UserhasGoalOrThrowAsync(userId, goalDateDto.GoalId, ct);

            var command = _mapper.Map<CreateGoalDateCommand>(goalDateDto);

            GoalDate newGoalDate = await _goalDateService.CreateGoalDateAsync(command, ct);

            var result = _mapper.Map<GetGoalDateDto>(newGoalDate);

            return Ok(result);
        }

        [HttpGet]
        [DtoValidationFilter]
        [Route("dates")]
        public async Task<IActionResult> GetGoalDates([FromQuery] GetGoalDatesByMonthDto dto, CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _goalService.UserhasGoalOrThrowAsync(userId, dto.GoalId, ct);

            var command = _mapper.Map<GetGoalDatesByMonthQuerry>(dto);

            var dates = await _goalDateService.GetGoalDatesByMonthAsync(command, ct);

            var result = _mapper.Map<List<GetGoalDateDto>>(dates);
            return Ok(result);
        }
    }
}
