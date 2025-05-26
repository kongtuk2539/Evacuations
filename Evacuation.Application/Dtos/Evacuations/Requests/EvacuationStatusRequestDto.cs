using Evacuations.Domain.Common;
using System.Text.Json.Serialization;

namespace Evacuations.Application.Dtos.Evacuations.Requests;

public class EvacuationStatusRequestDto
{
    public Guid Id { get; set; }
    public EnumStatuses Status { get; set; }
}
