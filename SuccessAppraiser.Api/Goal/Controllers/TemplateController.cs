using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuccessAppraiser.Api.Filters;
using SuccessAppraiser.Api.Goal.Contracts;
using SuccessAppraiser.Data.Repositories.Interfaces;
using System.Security.Claims;

namespace SuccessAppraiser.Api.Goal.Controllers
{
    [ApiController]
    [Authorize]
    [ValidationExceptionFilter]
    public class TemplateController : ControllerBase
    {
        private readonly IGoalTemplateRepotitory _goalTemplateRepotitory;
        private readonly IMapper _mapper;

        // TODO: create template service
        public TemplateController(IGoalTemplateRepotitory goalTemplateRepotitory, IMapper mapper)
        {
            _goalTemplateRepotitory = goalTemplateRepotitory;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("/templates")]
        public async Task<IActionResult> GetUserTemplates()
        {
            Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var userAndSystemTemplates = await _goalTemplateRepotitory
                .FindAsync(t => t.UserId == userId || t.UserId == null);

            return Ok(_mapper.Map<List<GetRawTemplateDto>>(userAndSystemTemplates));
        }
    }
}
