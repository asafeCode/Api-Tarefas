using FluentValidation;
using TarefasCrud.Communication.Requests;
using TarefasCrud.Domain.Entities;

namespace TarefasCrud.Application.UseCases.Tasks;

public class TaskValidator : AbstractValidator<RequestTaskJson>
{
    public TaskValidator(TaskEntity? task = null)
    {
        RuleFor(t => t.Title)
            .NotEmpty()
            .WithMessage("Título não pode ser vazio.");

        RuleFor(t => t.Description)
            .MaximumLength(150)
            .WithMessage("A descrição deve ter no máximo 150 caracteres.");

        RuleFor(t => t.WeeklyGoal)
            .GreaterThan(0)
            .LessThanOrEqualTo(7)
            .WithMessage("Meta da semana precisa ser entre 1 e 7.");

        RuleFor(t => t.WeeklyGoal)
            .GreaterThanOrEqualTo(_ => task!.Progress)
            .WithMessage("A meta deve ser maior ou igual ao progresso.")
            .When(_ => task is not null);
        
        RuleFor(t => t.StartDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("A data não pode ser no passado.");

        RuleFor(t => t.StartDate)
            .Must(date => date.DayOfWeek == DayOfWeek.Monday)
            .WithMessage("A rotina deve começar em uma segunda-feira.");
    }
}