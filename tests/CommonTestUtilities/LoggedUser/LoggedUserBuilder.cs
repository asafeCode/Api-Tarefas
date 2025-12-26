using Moq;
using TarefasCrud.Shared.Services;
using TarefasCrud.Shared.SharedEntities;

namespace CommonTestUtilities.LoggedUser;

public class LoggedUserBuilder
{
    public static ILoggedUser Build(User user)
    {
        var mock = new Mock<ILoggedUser>();
        mock.Setup(x => x.User()).ReturnsAsync(user);
        
        return mock.Object;
    }
}