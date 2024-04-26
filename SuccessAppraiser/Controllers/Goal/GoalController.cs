using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SuccessAppraiser.Contracts.Goal;
using SuccessAppraiser.Entities;
using SuccessAppraiser.Services.Goal.Errors;
using SuccessAppraiser.Services.Goal.Interfaces;
using System;
using System.Security.Claims;

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



            return Ok(_mapper.Map<List<GoalItem>, List<GetUserGoalDto>>(items));
        }

        [HttpPost]
        public async Task<IActionResult> Goals([FromBody] AddGoalDto goalDto, CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            GoalItem newGoal;
            try
            {
                newGoal = await _goalService.AddGoalAsync(userId, goalDto, ct);
            }
            catch (InvalidTemplateException exception)
            {
                return BadRequest(exception.Message);
            }
            return Ok(_mapper.Map<GetUserGoalDto>(newGoal));
        }

        [HttpPost]
        public async Task<IActionResult> GoalDate([FromBody] AddGoalDateDto goalDateDto, CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);


            if (!await _goalService.UserhasGoalAsync(userId, goalDateDto.GoalId, ct))
            {
                return BadRequest("There is no such goal with provided ID");
            }

            GetGoalDateDto newGoalDate;

            try
            {
                newGoalDate = await _goalDateService.AddGoalDateAsync(goalDateDto, ct);
            }
            catch (Exception ex) when (ex is GoalNotFoundException || ex is GoalDateAlreadyExistsException ||
                ex is InvalidStateException || ex is InvalidDateException)
            {
                return BadRequest(ex.Message);
            }


            return Ok(newGoalDate);
        }

        [HttpPost]
        public async Task<IActionResult> GetGoalDates([FromBody] GetGoalDatesByMonth dto, CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (!await _goalService.UserhasGoalAsync(userId, dto.GoalId))
            {
                return BadRequest("There is no such goal with provided ID");
            }

            var dates = await _goalDateService.GetGoalDatesByMonthAsync(dto);
            return Ok(dates);
        }
    }
}
