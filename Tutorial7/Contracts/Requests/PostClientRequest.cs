namespace Tutorial7.Contracts.Requests;

public record struct PostClientRequest(
    string FirstName,
    string LastName,
    string Email,
    string? Telephone,
    string? Pesel
    );