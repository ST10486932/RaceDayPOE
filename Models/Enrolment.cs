namespace RaceDay2.Models;

using RaceDay2.Data;
using RaceDay2.Models;

// Links Participant + Event + Category
public class Enrolment
{
    public int Id { get; set; }

    public int EventId { get; set; }
    public Event? Event { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public string ParticipantId { get; set; } = string.Empty;
    public ApplicationUser? Participant { get; set; }

    // ADD THIS LINE!
    public string Status { get; set; } = "Pending";

    public DateTime EnrolledOn { get; set; } = DateTime.UtcNow;

    public Result? Result { get; set; }
}