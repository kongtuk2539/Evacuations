using Evacuations.Application.Dtos.Evacuations.Requests;
using Evacuations.Domain.Common;
using FluentValidation;

namespace Evacuations.Application.Dtos.Evacuations.Validators;

public class EvacuationStatusesValidator : AbstractValidator<EvacuationStatusRequestDto>
{
    private readonly List<string> statuses = GetEnumStatues.GetAll();
    public EvacuationStatusesValidator()
    {
        RuleFor(es => es.Status)
            .Must(status => Enum.IsDefined(typeof(EnumStatuses), status))
            .WithMessage($"Status only can be [{string.Join(", ", statuses)}]");
    }


}
