using FluentValidation;
using TarefasCrud.Communication.Requests;

namespace TarefasCrud.Application.UseCases.Tasks;

public class TaskValidator : AbstractValidator<RequestTaskJson>
{
    public TaskValidator()
    {
        RuleFor(t => t.Title)
            .NotEmpty()
            .WithMessage("Título não pode ser vazio.");

        RuleFor(t => t.WeeklyGoal)
            .GreaterThan(0)
            .LessThanOrEqualTo(7)
            .WithMessage("A meta semanal deve ser entre 1 e 7.");

        RuleFor(t => t.StartDate)
            .NotEmpty().WithMessage("A data de início é obrigatória.")
            .Must(BeMonday).WithMessage("A rotina semanal deve começar em uma segunda-feira.")
            .Must(NotBePastDay).WithMessage("A data de início não pode ser no passado.");

        RuleFor(t => t.Category)
            .NotEmpty()
            .WithMessage("A categoria não pode ser vazia.");
    }

    private bool BeMonday(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Monday;
    }

    private bool NotBePastDay(DateTime date)
    {
        return date.Date >= DateTime.Now.Date;
    }
}
