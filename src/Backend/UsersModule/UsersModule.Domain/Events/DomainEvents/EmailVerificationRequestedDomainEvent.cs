namespace UsersModule.Domain.Events.DomainEvents;

public record EmailVerificationRequestedDomainEvent(string Email, string VerificationLink);