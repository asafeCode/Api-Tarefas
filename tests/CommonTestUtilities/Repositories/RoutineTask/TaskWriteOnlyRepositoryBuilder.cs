using Moq;
using TarefasCrud.Domain.Repositories.Tasks;
using TasksModule.Domain.Repositories;

namespace CommonTestUtilities.Repositories.RoutineTask;

public class TaskWriteOnlyRepositoryBuilder
{
    public static ITaskWriteOnlyRepository Build() => new Mock<ITaskWriteOnlyRepository>().Object;
}