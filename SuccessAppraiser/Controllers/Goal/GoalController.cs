using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuccessAppraiser.Entities;
using SuccessAppraiser.Services.Goal.DTO;
using SuccessAppraiser.Services.Goal.Interfaces;
using System.Security.Claims;

namespace SuccessAppraiser.Controllers.Goal
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]
    public class GoalController : ControllerBase
    {
        private readonly IGoalService _goalService;
        private readonly IMapper _mapper;

        public GoalController(IGoalService goalService, IMapper mapper)
        {
            _goalService = goalService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Goals(CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var items = await _goalService.GetGoalsByUserIdAsync(userId, ct);



            return Ok(_mapper.Map<List<GoalItem>, List<GetUserGoalDto>>(items));
        }

        [HttpPost]
        public async Task<IActionResult> AddGoal([FromBody] AddGoalDto goalDto, CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            GoalItem newGoal = await _goalService.AddGoalAsync(userId, goalDto, ct);
            return Ok(_mapper.Map<GetUserGoalDto>(newGoal));
        }
    }
}
