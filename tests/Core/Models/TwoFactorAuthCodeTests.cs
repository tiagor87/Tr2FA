using System;
using FluentAssertions;
using TRTwoFactorAuth.Abstractions.Models;
using Xunit;

namespace TRTwoFactorAuth.Core.Tests.Models;

public class TwoFactorAuthCodeTests
{
    [Fact]
    public void GivenTwoFactorCode_WhenCreate_ThenShouldReturn()
    {
        var code = new TwoFactorAuthCode("123456");

        code.Should().NotBeNull();
        code.Value.Should().Be("123456");
    }
    
    [Fact]
    public void GivenTwoFactorCode_WhenReadAsString_ThenShouldReturnValue()
    {
        var code = new TwoFactorAuthCode("123456");

        code.Should().NotBeNull();
        code.ToString().Should().Be("123456");
    }
    
    [Theory]
    [InlineData(" ")]
    [InlineData("abcdef")]
    [InlineData("1234567")]
    public void GivenTwoFactorCode_WhenCreate_AndArgumentInvalid_ThenShouldThrow(string codeText)
    {
        var action = () => new TwoFactorAuthCode(codeText);

        action.Should().Throw<ArgumentException>();
    }
}