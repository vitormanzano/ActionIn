using System.Reflection;
using ActionIn.Authentication.Application.Commands.RegisterAccount;
using ActionIn.Authentication.Data;
using ActionIn.Authentication.Domain;
using NetArchTest.Rules;

namespace ActionIn.Authentication.Tests.ArchTests.Modules;

public class LayerTests
{
    private static readonly Assembly DomainAssembly = typeof(Account).Assembly;
    private static readonly Assembly ApplicationAssembly = typeof(RegisterAccountCommandHandler).Assembly;
    private static readonly Assembly DataAssembly = typeof(AuthenticationContext).Assembly;

    [Fact]
    public void DomainLayer_DoesNotHaveDependency_InAnyLayer()
    {
        var result = Types
            .InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny("ActionIn.Authentication.Application", "ActionIn.Authentication.Data")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void ApplicationLayer_DoesNotHaveDependency_OnInfrastructure()
    {
        var result = Types
            .InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn("ActionIn.Authentication.Data")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void DataLayer_DoesHaveDependency_OnDomain()
    {
        var result = Types
            .InAssembly(DataAssembly)
            .That()
            .DoNotResideInNamespaceEndingWith("Migrations")
            .And()
            .DoNotHaveNameStartingWith("<")
            .Should()
            .HaveDependencyOn("ActionIn.Authentication.Domain")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
}


