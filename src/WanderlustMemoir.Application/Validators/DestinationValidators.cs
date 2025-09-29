using FluentValidation;
using WanderlustMemoir.Application.DTOs.Destinations;

namespace WanderlustMemoir.Application.Validators;

public class CreateDestinationValidator : AbstractValidator<CreateDestinationDto>
{
    public CreateDestinationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .Length(1, 100).WithMessage("Nome deve ter entre 1 e 100 caracteres");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("País é obrigatório")
            .Length(1, 100).WithMessage("País deve ter entre 1 e 100 caracteres");

        RuleFor(x => x.Priority)
            .Must(BeValidPriority).WithMessage("Prioridade deve ser 'high', 'medium' ou 'low'");
    }

    private static bool BeValidPriority(string priority)
    {
        var validPriorities = new[] { "high", "medium", "low" };
        return validPriorities.Contains(priority?.ToLower());
    }
}

public class UpdateDestinationValidator : AbstractValidator<UpdateDestinationDto>
{
    public UpdateDestinationValidator()
    {
        RuleFor(x => x.Name)
            .Length(1, 100).WithMessage("Nome deve ter entre 1 e 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Country)
            .Length(1, 100).WithMessage("País deve ter entre 1 e 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Country));

        RuleFor(x => x.Priority)
            .Must(BeValidPriority).WithMessage("Prioridade deve ser 'high', 'medium' ou 'low'")
            .When(x => !string.IsNullOrEmpty(x.Priority));
    }

    private static bool BeValidPriority(string? priority)
    {
        if (string.IsNullOrEmpty(priority)) return true;
        var validPriorities = new[] { "high", "medium", "low" };
        return validPriorities.Contains(priority.ToLower());
    }
}