using AutoMapper;
using SuccessAppraiser.BLL.Common.Exceptions.Validation;
using SuccessAppraiser.BLL.Goal.Contracts;
using SuccessAppraiser.BLL.Goal.Services.Interfaces;
using SuccessAppraiser.Data.Entities;
using SuccessAppraiser.Data.Repositories.Base;
using SuccessAppraiser.Data.Repositories.Interfaces;

namespace SuccessAppraiser.BLL.Goal.Services
{
    public class GoalService : IGoalService
    {
        private readonly IGoalRepository _goalRepository;
        private readonly IGoalTemplateRepotitory _goalTemplateRepotitory;
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;

        public GoalService(IGoalRepository goalRepository, IGoalTemplateRepotitory goalTemplateRepotitory,
            IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _goalRepository = goalRepository;
            _goalTemplateRepotitory = goalTemplateRepotitory;
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
        }

        public async Task<GoalItem> CreateGoalAsync(CreateGoalCommand createCommand, CancellationToken ct = default)
        {
            GoalTemplate? template = await _goalTemplateRepotitory.GetByIdAsync(createCommand.TemplateId, ct);
            if (template == null)
            {
                throw new InvalidIdException(nameof(GoalTemplate), createCommand.TemplateId);
            }

            GoalItem newGoal = _mapper.Map<GoalItem>(createCommand);
            newGoal.UserId = createCommand.UserId;
            await _goalRepository.AddAsync(newGoal, ct);
            await _repositoryWrapper.SaveChangesAsync(ct);
            return newGoal;
        }

        public async Task DeleteGoalAsync(Guid goalId, CancellationToken ct = default)
        {
            GoalItem? goal = await _goalRepository.GetByIdAsync(goalId, ct);
            if (goal != null)
            {
                _goalRepository.Delete(goal);
                await _repositoryWrapper.SaveChangesAsync(ct);
            }
            else
            {
                throw new InvalidIdException(nameof(GoalItem), goalId);
            }
        }

        public async Task<IEnumerable<GoalItem>> GetGoalsByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _goalRepository.GetGoalsByUserIdAsync(userId, ct);
        }

        public async Task UserhasGoalOrThrowAsync(Guid userId, Guid goalId, CancellationToken ct = default)
        {
            bool hasGoal = await _goalRepository.UserHasGoalAsync(userId, goalId, ct);

            if (!hasGoal)
            {
                throw new InvalidIdException(nameof(GoalItem), goalId);
            }
        }
    }
}
