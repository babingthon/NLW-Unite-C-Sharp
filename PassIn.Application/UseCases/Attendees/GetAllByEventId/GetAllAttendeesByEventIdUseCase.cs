using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.Attendees.GetAllByEventId;
public class GetAllAttendeesByEventIdUseCase
{
    private readonly PassInDbContext _dbContext;
    public GetAllAttendeesByEventIdUseCase()
    {
        _dbContext = new PassInDbContext();
    }

    public ResponseAllAttendeesJson Execute(Guid eventId)
    {
        var eventEntity = _dbContext.Events.Include(ev => ev.Attendees).ThenInclude(attendee => attendee.CheckIn).FirstOrDefault(ev => ev.Id == eventId);

        if (eventEntity is null)
            throw new NotFoundException("An event with this id dont exist.");

        return new ResponseAllAttendeesJson
        {
            Attendees = eventEntity.Attendees.Select(attendees => new ResponseAttendeeJson
            {
                Id = attendees.Id,
                Name = attendees.Name,
                Email = attendees.Email,
                CreatedAt = attendees.Created_At,
                CheckedInAt = attendees.CheckIn?.Created_at,
            }).ToList()

        };
    }
}
