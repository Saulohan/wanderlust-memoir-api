using FluentValidation;
using WanderlustMemoir.Application.DTOs.VisitedPlaces;

namespace WanderlustMemoir.Application.Validators;

public class CreateVisitedPlaceValidator : AbstractValidator<CreateVisitedPlaceDto>
{
    public CreateVisitedPlaceValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome � obrigat�rio")
            .Length(1, 100).WithMessage("Nome deve ter entre 1 e 100 caracteres");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Pa�s � obrigat�rio")
            .Length(1, 100).WithMessage("Pa�s deve ter entre 1 e 100 caracteres");

        RuleFor(x => x.VisitDate)
            .NotEmpty().WithMessage("Data da visita � obrigat�ria")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Data da visita n�o pode ser no futuro");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Descri��o deve ter no m�ximo 1000 caracteres");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Avalia��o deve ser entre 1 e 5");
    }
}

public class UpdateVisitedPlaceValidator : AbstractValidator<UpdateVisitedPlaceDto>
{
    public UpdateVisitedPlaceValidator()
    {
        RuleFor(x => x.Name)
            .Length(1, 100).WithMessage("Nome deve ter entre 1 e 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Country)
            .Length(1, 100).WithMessage("Pa�s deve ter entre 1 e 100 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Country));

        RuleFor(x => x.VisitDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Data da visita n�o pode ser no futuro")
            .When(x => x.VisitDate.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Descri��o deve ter no m�ximo 1000 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Avalia��o deve ser entre 1 e 5")
            .When(x => x.Rating.HasValue);
    }
}