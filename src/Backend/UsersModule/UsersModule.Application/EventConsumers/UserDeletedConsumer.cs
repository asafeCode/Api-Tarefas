using UsersModule.Domain.Events.EventsDtos;
using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services.Email;

namespace UsersModule.Application.EventConsumers;

public class UserDeletedConsumer
{
    private readonly IUserWriteOnlyRepository _writeRepository;
    private readonly IUserReadOnlyRepository _readRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    public UserDeletedConsumer(IUserWriteOnlyRepository repository, IUnitOfWork unitOfWork, IUserReadOnlyRepository readRepository, IEmailService emailService)
    {
        _writeRepository = repository;
        _unitOfWork = unitOfWork;
        _readRepository = readRepository;
        _emailService = emailService;
    }
    public async Task Handle(UserDeletedEvent @event)
    {
        var user = await _readRepository.GetByUserIdentifier(@event.UserId);
        if (user is not null) return;

        await _writeRepository.DeleteAccount(@event.UserId);
        await _unitOfWork.Commit();
        
        await _emailService.SendDeleteCompletedEmail(@event.Email);
    }
}