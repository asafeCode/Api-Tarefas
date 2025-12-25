using UsersModule.Domain.Repositories;
using UsersModule.Domain.Repositories.User;

namespace UsersModule.Application.EventConsumers;

public class UserDeletedConsumer
{
    private readonly IUserWriteOnlyRepository _writeRepository;
    private readonly IUserReadOnlyRepository _readRepository;
    private readonly IUnitOfWork _unitOfWork;
    //private readonly IEmailService _emailService;
    public UserDeletedConsumer(IUserWriteOnlyRepository repository, IUnitOfWork unitOfWork, IUserReadOnlyRepository readRepository)
    {
        _writeRepository = repository;
        _unitOfWork = unitOfWork;
        _readRepository = readRepository;
    }
    public async Task Handle(Guid userId)
    {
        var user = await _readRepository.GetByUserIdentifier(userId);
        if (user is not null) return;

        await _writeRepository.DeleteAccount(userId);
        await _unitOfWork.Commit();

        //await _emailService.SendAsync(user.Email, "Conta deletada", "Sua conta foi deletada com sucesso.");
    }
}