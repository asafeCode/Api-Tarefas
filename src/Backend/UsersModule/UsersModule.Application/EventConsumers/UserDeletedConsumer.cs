using TarefasCrud.Shared.Repositories;
using UsersModule.Domain.Events.EventsDtos;
using UsersModule.Domain.Repositories.User;
using UsersModule.Domain.Services.Email;

namespace UsersModule.Application.EventConsumers;

public class UserDeletedConsumer
{
    private readonly IUserWriteOnlyRepository _writeRepository;
    private readonly IUserInternalRepository _internalRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public UserDeletedConsumer(IUserWriteOnlyRepository writeRepository, 
        IUserInternalRepository internalRepository, 
        IUnitOfWork unitOfWork, 
        IEmailService emailService)
    {
        _writeRepository = writeRepository;
        _internalRepository = internalRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task Handle(UserDeletedEvent @event)
    {
        var user = await _internalRepository.GetUserById(@event.UserId);

        if (user is null || user.Active) return;
        if (user.DeletionScheduledAt is null || user.DeletionScheduledAt.Value > DateTime.UtcNow) return;
        
        await _writeRepository.DeleteAccount(@event.UserId);
        await _unitOfWork.Commit();

        await _emailService.SendDeleteCompletedEmail(@event.Email);
    }
}