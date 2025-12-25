namespace UsersModule.Domain.Events.Queues;

public record EmailVerificationRequestedDomainEvent(string Email, string VerificationLink);