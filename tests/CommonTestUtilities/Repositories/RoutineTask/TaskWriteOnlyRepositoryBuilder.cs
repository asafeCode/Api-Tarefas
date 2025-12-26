using Moq;
using TasksModule.Domain.Repositories;

namespace CommonTestUtilities.Repositories.RoutineTask;

public class TaskWriteOnlyRepositoryBuilder
{
    public static ITaskWriteOnlyRepository Build() => new Mock<ITaskWriteOnlyRepository>().Object;
}