namespace UsersModule.Domain.Events.EventsDtos;

public record EmailVerifiedEvent(string Email, string VerificationLink);