namespace PracaInzynierska_RentIt.Server.Models.Enums;

public enum PasswordResetStatus
{
    Success,
    UserNotFound,
    IncorrectOldPassword,
    PasswordChangeFailed
}