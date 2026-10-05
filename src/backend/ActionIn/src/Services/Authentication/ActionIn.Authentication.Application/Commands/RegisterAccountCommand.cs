using ActionIn.Authentication.Application.Dtos;
using ActionIn.Core.Messages.Commands;

namespace ActionIn.Authentication.Application.Commands;

public sealed record RegisterAccountCommand(RegisterAccountDto account) : ICommand;



