using Mapster;
using TarefasCrud.Application.SharedValidators;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Communication.Responses;
using TarefasCrud.Domain.Entities;
using TarefasCrud.Domain.Enums;
using TarefasCrud.Domain.Extensions;
using TarefasCrud.Domain.Repositories;
using TarefasCrud.Domain.Repositories.Tasks;
using TarefasCrud.Domain.Services.LoggedUser;
using TarefasCrud.Domain.ValueObjects;

namespace TarefasCrud.Application.UseCases.Tasks.Register;

public class RegisterTaskUseCase : IRegisterTaskUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly ITaskWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public RegisterTaskUseCase(ILoggedUser loggedUser, 
        ITaskWriteOnlyRepository repository, 
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResponseRegisteredTaskJson> Execute(RequestTaskJson request)
    {
        Validate(request);
        var loggedUser = await _loggedUser.User();
        var task = request.Adapt<TaskEntity>();
        task.UserId = loggedUser.Id;
        task.WeekOfMonth = GetMonthWeek(task.StartDate);
        task.Progress = TarefasCrudRuleConstants.INITIAL_PROGRESS;

        await _repository.Add(task);
        await _unitOfWork.Commit();

        return new ResponseRegisteredTaskJson
        {
            Id = task.Id,
            Title = task.Title,
        };
    }

    private void Validate(RequestTaskJson request)
    {
        var validator = new TaskValidator();
        var result = validator.Validate(request);
        
        if (result.IsValid)
            return;
        
        HandleValidationResult.ThrowError(result);
    }
    
    private static int GetMonthWeek(DateOnly startDate)
    {
        var firstDayOfMonth = new DateOnly(startDate.Year, startDate.Month, 1);

        int offset = ((int)DayOfWeek.Monday - (int)firstDayOfMonth.DayOfWeek + 7) % 7;

        var firstMonday = firstDayOfMonth.AddDays(offset);
        
        int week = (startDate.DayNumber - firstMonday.DayNumber) / 7 + 1;

        return week;
    }
}