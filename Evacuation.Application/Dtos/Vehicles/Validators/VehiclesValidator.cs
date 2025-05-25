using Evacuations.Application.Dtos.Vehicles.Requests;
using FluentValidation;

namespace Evacuations.Application.Dtos.Vehicles.Validators;

public class VehiclesValidator : AbstractValidator<VehicleRequestDto>
{
    public VehiclesValidator()
    {
        RuleFor(v => v.Capacity)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Capacity must more than or Equal 1");

        RuleFor(v => v.Type)
            .NotNull()
            .WithMessage("Type can not be Null")
            .NotEmpty()
            .WithMessage("Type can not be Empty");

        RuleFor(v => v.LocationCoordinates)
            .NotNull()
            .WithMessage("LocationCoordinates can not be Null");

        RuleFor(v => v.Speed)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Speed must more than or Equal 1");
    }
}
