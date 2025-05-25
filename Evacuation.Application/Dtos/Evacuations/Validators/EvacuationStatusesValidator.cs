using Evacuations.Application.Dtos.Evacuations.Requests;
using Evacuations.Domain.Common;
using FluentValidation;

namespace Evacuations.Application.Dtos.Evacuations.Validators;

public class EvacuationStatusesValidator : AbstractValidator<EvacuationStatusRequestDto>
{
    private readonly List<string> statuses = 
        [EnumStatus.PROGRESS.ToString(), EnumStatus.SUCCEED.ToString(), EnumStatus.CANCEL.ToString()];
    public EvacuationStatusesValidator()
    {
        RuleFor(es => es.Status)
            .NotEmpty()
            .WithMessage("Status can not be Empty")
            .Must(status => statuses.Contains(status!.ToUpper()))
            .WithMessage($"Status only can be [{string.Join(", ", statuses)}]");
    }
}
