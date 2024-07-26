using SuccessAppraiser.Api.Goal.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.BLL.Goal.Services.Interfaces;
using SuccessAppraiser.Data.Entities;
using System.Security.Claims;
using SuccessAppraiser.Api.Filters;

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
        public async Task<IActionResult> GetGoals(CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var items = await _goalService.GetGoalsByUserIdAsync(userId, ct);

            return Ok(_mapper.Map<List<GetUserGoalDto>>(items));
        }

        [HttpPost]
        [DtoValidationFilter]
        [Route("goals")]
        public async Task<IActionResult> CreateGoal([FromBody] CreateGoalDto goalDto, CancellationToken ct)
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
        [Route("goals/{goalId:guid}/dates")]
        public async Task<IActionResult> CreateGoalDate([FromBody] CreateGoalDateDto goalDateDto,
            [FromRoute] Guid goalId, CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _goalService.UserhasGoalOrThrowAsync(userId, goalId, ct);

            var command = _mapper.Map<CreateGoalDateCommand>(goalDateDto);
            command.GoalId = goalId;

            GoalDate newGoalDate = await _goalDateService.CreateGoalDateAsync(command, ct);

            var result = _mapper.Map<GetGoalDateDto>(newGoalDate);

            return Ok(result);
        }

        [HttpGet]
        [DtoValidationFilter]
        [Route("goals/{goalId:guid}/dates")]
        public async Task<IActionResult> GetGoalDates([FromQuery] GetGoalDatesByMonthDto dto,
            [FromRoute] Guid goalId, CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _goalService.UserhasGoalOrThrowAsync(userId, goalId, ct);

            var command = _mapper.Map<GetGoalDatesByMonthQuerry>(dto);
            command.GoalId = goalId;

            var dates = await _goalDateService.GetGoalDatesByMonthAsync(command, ct);

            var result = _mapper.Map<List<GetGoalDateDto>>(dates);
            return Ok(result);
        }

        [HttpPut]
        [Route("goals/{goalId:guid}/dates")]
        public async Task<IActionResult> UpdateGoalDate([FromBody] UpdateGoalDateDto dto,
            [FromRoute] Guid goalId, CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _goalService.UserhasGoalOrThrowAsync(userId, goalId, ct);

            var command = _mapper.Map<UpdateGoalDateCoomand>(dto);
            command.GoalId = goalId;
            var goalDate = await _goalDateService.UpdateGoaldateAsync(command, ct);

            var result = _mapper.Map<GetGoalDateDto>(goalDate);
            return Ok(result);
        }
    }
}
