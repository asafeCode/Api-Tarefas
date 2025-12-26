namespace UsersModule.Domain.ValueObjects;

public static class TarefasCrudRuleConstants
{
    public static DateTime TimeToRecoverAccount()
    {
        return DateTime.UtcNow.AddMinutes(2);
    }
}