using ActionIn.Core.Messages.Commands;

namespace ActionIn.Authentication.Application.Commands.RegisterAccount;

public sealed record RegisterAccountCommand(string Username, string Email, string Password) : Command;



