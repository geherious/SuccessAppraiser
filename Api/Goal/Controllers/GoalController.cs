using Api.Goal.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuccessAppraiser.BLL.Common.Exceptions;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.BLL.Goal.Services.Interfaces;
using SuccessAppraiser.Data.Entities;
using System;
using System.Security.Claims;
using Api.Filters;

namespace SuccessAppraiser.Controllers.Goal
{
    [ApiController]
    [Route("[action]")]
    [Authorize]
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
        public async Task<IActionResult> Goals(CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var items = await _goalService.GetGoalsByUserIdAsync(userId, ct);

            return Ok(_mapper.Map<List<GetUserGoalDto>>(items));
        }

        [HttpPost]
        [ValidationFilter]
        public async Task<IActionResult> Goals([FromBody] CreateGoalDto goalDto, CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            GoalItem newGoal;
            var command = _mapper.Map<CreateGoalCommand>(goalDto);
            command.UserId = userId;
            try
            {
                newGoal = await _goalService.CreateGoalAsync(command, ct);
            }
            catch (NotFoundException exception)
            {
                return BadRequest(exception.Message);
            }

            var result = _mapper.Map<GetUserGoalDto>(newGoal);
            return Ok(result);
        }

        [HttpPost]
        [ValidationFilter]
        public async Task<IActionResult> GoalDate([FromBody] CreateGoalDateDto goalDateDto, CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


            if (!await _goalService.UserhasGoalAsync(userId, goalDateDto.GoalId, ct))
            {
                return BadRequest("There is no such goal with provided ID");
            }

            GoalDate newGoalDate;
            var command = _mapper.Map<CreateGoalDateCommand>(goalDateDto);

            try
            {
                newGoalDate = await _goalDateService.CreateGoalDateAsync(command, ct);
            }
            catch (Exception ex) when (ex is NotFoundException || ex is ArgumentException)
            {
                return BadRequest(ex.Message);
            }

            var result = _mapper.Map<GetGoalDateDto>(newGoalDate);

            return Ok(result);
        }

        [HttpGet]
        [ValidationFilter]
        public async Task<IActionResult> GetGoalDates([FromQuery] GetGoalDatesByMonthDto dto, CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (!await _goalService.UserhasGoalAsync(userId, dto.GoalId))
            {
                return BadRequest("There is no such goal with provided ID");
            }

            var command = _mapper.Map<GetGoalDatesByMonthQuerry>(dto);

            var dates = await _goalDateService.GetGoalDatesByMonthAsync(command, ct);

            var result = _mapper.Map<List<GetGoalDateDto>>(dates);
            return Ok(result);
        }
    }
}
