using Evacuations.Application.Dtos.Vehicles.Requests;
using FluentValidation;

namespace Evacuations.Application.Dtos.Vehicles.Validators;

public class VehiclesValidator : AbstractValidator<VehicleRequestDto>
{
    public VehiclesValidator()
    {
        RuleFor(v => v.Capacity)
            .GreaterThanOrEqualTo(1);
    }
}
