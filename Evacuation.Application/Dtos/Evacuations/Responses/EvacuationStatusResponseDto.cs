using Evacuations.Domain.Common;
using System.Text.Json.Serialization;

namespace Evacuations.Application.Dtos.Evacuations.Responses;

public class EvacuationStatusResponseDto
{
    public Guid Id { get; set; }
    public Guid ZoneId { get; set; }
    public int TotalEvacuated { get; set; }
    public int RemainingPeople { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EnumStatuses Status { get; set; }
}
