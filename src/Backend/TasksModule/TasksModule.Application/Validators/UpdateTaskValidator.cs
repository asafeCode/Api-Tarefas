using FluentValidation;
using TarefasCrud.Exceptions;
using TasksModule.Application.UseCases.Create;
using TasksModule.Application.UseCases.Update;
using TasksModule.Domain.Entities;

namespace TasksModule.Application.Validators;

public class UpdateTaskValidator : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskValidator(DateOnly dateNow, TaskEntity? task)
    {
        RuleFor(t => t.Title)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.TASK_TITLE_EMPTY);

        RuleFor(t => t.Description)
            .MaximumLength(150)
            .WithMessage(ResourceMessagesException.DESCRIPTION_EXCEEDS_LIMIT_CHARACTERS);

        RuleFor(t => t.WeeklyGoal)
            .GreaterThan(0)
            .LessThanOrEqualTo(t => DaysToSunday(t.StartDate))
            .WithMessage(ResourceMessagesException.WEEKLY_GOAL_INVALID);

        RuleFor(t => t.WeeklyGoal)
            .GreaterThanOrEqualTo(_ => task!.Progress)
            .WithMessage(ResourceMessagesException.WEEKLY_GOAL_LOWER_THAN_PROGRESS);
        
        RuleFor(t => t.StartDate)
            .GreaterThanOrEqualTo(dateNow)
            .WithMessage(ResourceMessagesException.START_DATE_IN_PAST);

        RuleFor(t => t.StartDate)
            .Must(date => date.DayOfWeek is >= DayOfWeek.Monday and <= DayOfWeek.Wednesday)
            .WithMessage(ResourceMessagesException.START_DATE_INVALID);
    }

    private static int DaysToSunday(DateOnly startDate)
    {
        var dayOfWeek = startDate.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)startDate.DayOfWeek;
        return 7 - dayOfWeek + 1;
    }
}