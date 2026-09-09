namespace ActionIn.Authentication.Application.Dtos;

public record RegisterAccountDto(
        string Username,
        string Email,
        string Password);

