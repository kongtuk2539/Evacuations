using Evacuations.Application.Dtos.Evacuations.Requests;
using FluentValidation;

namespace Evacuations.Application.Dtos.Evacuations.Validators;

public class EvacuationZonesValidator : AbstractValidator<EvacuationZoneRequestDto>
{
    public EvacuationZonesValidator()
    {
        RuleFor(ez => ez.NumberOfPeople)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Number of People must more than or Equal 0");
    }
}
