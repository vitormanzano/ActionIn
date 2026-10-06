using ActionIn.Authentication.Application.Dtos;
using ActionIn.Core.Messages.Commands;

namespace ActionIn.Authentication.Application.Commands.RegisterAccount;

public sealed record RegisterAccountCommand(RegisterAccountDto Account) : ICommand;



