namespace UsersModule.Domain.Events.EventsDtos;

public record AccountRecoveredEvent(string Email, string VerificationLink);