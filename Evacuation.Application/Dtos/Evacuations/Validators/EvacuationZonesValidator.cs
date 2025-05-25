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

        RuleFor(ez => ez.UrgencyLevel)
            .InclusiveBetween(1, 5)
            .WithMessage("UrgencyLevel must be entered as a number between 1 and 5, " +
            "with 1 being the lowest and 5 being the highest.");

        RuleFor(ez => ez.LocationCoordinates)
            .NotNull()
            .WithMessage("LocationCoordinates can not be Null");

    }
}
