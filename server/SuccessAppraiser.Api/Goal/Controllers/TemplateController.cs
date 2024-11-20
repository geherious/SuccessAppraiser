using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuccessAppraiser.Api.Filters;
using SuccessAppraiser.Api.Goal.Contracts;
using SuccessAppraiser.Data.Repositories.Interfaces;
using System.Security.Claims;
using SuccessAppraiser.Data.Entities;
using SuccessAppraiser.Data.Repositories.Base;

namespace SuccessAppraiser.Api.Goal.Controllers
{
    [ApiController]
    [Route("api/")]
    [Authorize]
    [ValidationExceptionFilter]
    public class TemplateController : ControllerBase
    {
        private readonly IGoalTemplateRepotitory _goalTemplateRepotitory;
        private readonly IDayStateRepository _stateRepository;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        // TODO: create template service
        // TODO: create test for templates
        public TemplateController(
            IGoalTemplateRepotitory goalTemplateRepotitory,
            IDayStateRepository stateRepository,
            IMapper mapper,
            IRepositoryWrapper repositoryWrapper)
        {
            _goalTemplateRepotitory = goalTemplateRepotitory;
            _stateRepository = stateRepository;
            _mapper = mapper;
            _repositoryWrapper = repositoryWrapper;
        }

        [HttpGet]
        [Route("templates")]
        public async Task<IActionResult> GetUserTemplates()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var userAndSystemTemplates = await _goalTemplateRepotitory
                .FindAsync(t => t.UserId == userId || t.UserId == null);

            return Ok(_mapper.Map<List<GetRawTemplateDto>>(userAndSystemTemplates));
        }

        [HttpPost]
        [Route("templates")]
        public async Task<IActionResult> CreateUserTemplate([FromBody] CreateTemplateDto dto)
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (dto.States.Count == 0)
                return BadRequest("Number of states should be more then zero");

            List<DayState> states = new List<DayState>(capacity: dto.States.Count);

            foreach(var stateDto in dto.States)
            {
                var state = new DayState()
                {
                    Name = stateDto.Name,
                    Color = stateDto.Color
                };
                await _stateRepository.AddAsync(state);
                states.Add(state);
            }

            var createdTemplate = new GoalTemplate()
            {
                Name = dto.Name,
                States = states,
                UserId = userId
            };
            await _goalTemplateRepotitory.AddAsync(createdTemplate);

            await _repositoryWrapper.SaveChangesAsync();

            return Ok(new GetRawTemplateDto()
            {
                Id = createdTemplate.Id,
                Name = createdTemplate.Name
            });
        }
    }
}