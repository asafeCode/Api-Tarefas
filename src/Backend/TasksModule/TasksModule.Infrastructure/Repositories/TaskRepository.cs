using Microsoft.EntityFrameworkCore;
using TarefasCrud.Infrastructure;
using TarefasCrud.Shared.SharedEntities;
using TasksModule.Domain.Dtos;
using TasksModule.Domain.Entities;
using TasksModule.Domain.Extensions;
using TasksModule.Domain.Repositories;

namespace TasksModule.Infrastructure.Repositories;

public class TaskRepository : ITaskReadOnlyRepository, ITaskWriteOnlyRepository, ITaskUpdateOnlyRepository
{
    private readonly TarefasCrudDbContext _dbContext;
    public TaskRepository(TarefasCrudDbContext dbContext) => _dbContext = dbContext;
    async Task<TaskEntity?> ITaskReadOnlyRepository.GetById(User user, long taskId) => await _dbContext
        .Tasks
        .AsNoTracking()
        .FirstOrDefaultAsync(task => task.Active && task.Id == taskId && task.UserId == user.Id);
    public async Task<IList<TaskEntity>> GetTasks(User user, FilterTasksDto filters) 
    {
        var query = _dbContext
            .Tasks
            .AsNoTracking()
            .Where(task => task.Active && task.UserId == user.Id);
        
        if (filters.Title.NotEmpty())
        {
            query = query.Where(task => task.Title.Contains(filters.Title!));
        }
        if (filters.Category.NotEmpty())
        {
            query = query.Where(task => task.Category.Contains(filters.Category!));
        }
        if (filters.WeeklyGoalMin.HasValue)
            query = query.Where(task => task.WeeklyGoal >= filters.WeeklyGoalMin.Value);        
        
        if (filters.IsCompleted.HasValue)
            query = query.Where(task => task.IsCompleted == filters.IsCompleted.Value);

        if (filters.WeeklyGoalMax.HasValue)
            query = query.Where(task => task.WeeklyGoal <= filters.WeeklyGoalMax.Value);

        if (filters.ProgressMin.HasValue)
            query = query.Where(task => task.Progress >= filters.ProgressMin.Value);

        if (filters.ProgressMax.HasValue)
            query = query.Where(task => task.Progress <= filters.ProgressMax.Value);
        
        if (filters.WeekOfMonth.HasValue)
            query = query.Where(task => task.WeekOfMonth == filters.WeekOfMonth.Value);

        if (filters.Month.HasValue)
            query = query.Where(task => task.StartDate.Month == filters.Month.Value);

        if (filters.Year.HasValue)
            query = query.Where(task => task.StartDate.Year == filters.Year.Value);

        return await query.ToListAsync();
    }
    public async Task Add(TaskEntity task) => await _dbContext.Tasks.AddAsync(task);
    public async Task Delete(long taskId)
    {
        var task = await _dbContext.Tasks.FindAsync(taskId);
        _dbContext.Tasks.Remove(task!);
    } 
    public void Update(TaskEntity task) => _dbContext.Tasks.Update(task); 
    public async Task<TaskEntity?> GetById(User user, long taskId) => await _dbContext
        .Tasks
        .FirstOrDefaultAsync(task => task.Active && task.Id == taskId && task.UserId == user.Id);
}