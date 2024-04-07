using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;
using System.Net.Mail;

namespace PassIn.Api.UseCases.Events.RegisterAttendee;

public class RegisterAttendeeOnEventUseCase
{
    private readonly PassInDbContext _dbContext;
    public RegisterAttendeeOnEventUseCase()
    {
        _dbContext = new PassInDbContext();
    }

    public ResponseRegisteredJson Execute(Guid idEvent, RequestRegisterEventJson request)
    {
        Validate(idEvent, request);

        var entity = new Attendee
        {
            Name = request.Name,
            Email = request.Email,
            Event_Id = idEvent,
            Created_At = DateTime.UtcNow,

        };

        _dbContext.Attendees.Add(entity);
        _dbContext.SaveChanges();

        return new ResponseRegisteredJson
        {
            Id = entity.Id,
        };
    }

    private void Validate(Guid idEvent, RequestRegisterEventJson request)
    {
        var eventEntity = _dbContext.Events.Find(idEvent);

        if (eventEntity is null)
            throw new NotFoundException("An event with this id dont exist.");

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ErrorOnValidationException("The name is invalid.");

        if (EmailIsValid(request.Email) == false)
            throw new ErrorOnValidationException("The email is invalid.");

        var attendeeAlreadyRegister = _dbContext.Attendees.Any(att => att.Email.Equals(request.Email) && att.Event_Id == idEvent);

        if(attendeeAlreadyRegister)
            throw new ConflictException("You can not register twice on the same event.");

        var attendeeForEvent = _dbContext.Attendees.Count(att => att.Event_Id ==  idEvent);
        
        if(attendeeForEvent > eventEntity.Maximum_Attendees)
            throw new ConflictException("There is no room for this event.");

    }

    private bool EmailIsValid(string email)
    {
        try
        {
            new MailAddress(email);
            return true;
        }catch
        {
            return false;
        }
    }
}

