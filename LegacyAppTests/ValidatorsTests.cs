using LegacyApp.Interfaces;
using LegacyApp.Utils;

namespace LegacyAppTests;

public class ValidatorsTests
{
    [Fact]
    public void NameValidator_ShouldReturnFalse_WhenNameIsEmpty()
    {
        string input = "";
        
        bool result = NameValidator.Validate(input);
        
        Assert.False(result);
    }
    
    [Fact]
    public void NameValidator_ShouldReturnTrue_WhenNameIsNotEmpty()
    {
        string input = "John";
        
        bool result = NameValidator.Validate(input);
        
        Assert.True(result);
    }
    
    [Fact]
    public void EmailValidator_ShouldReturnFalse_WhenEmailDoesntContainAtSymbol()
    {
        string input = "test.com";
        
        bool result = EmailValidator.Validate(input);
        
        Assert.False(result);
    }
    
    [Fact]
    public void EmailValidator_ShouldReturnFalse_WhenEmailDoesntContainDot()
    {
        string input = "test@test";
        
        bool result = EmailValidator.Validate(input);
        
        Assert.False(result);
    }

    [Fact]
    public void EmailValidator_ShouldReturnTrue_WhenEmailHasBothAtSymbolAndDot()
    {
        string input = "test@test.com";
        
        bool result = EmailValidator.Validate(input);
        
        Assert.True(result);
    }
    
    [Fact]
    public void AgeValidator_ShouldReturnTrue_WhenAgeIsMoreThan21()
    {
        DateTime input = DateTime.Now.AddYears(-22);
        
        bool result = AgeValidator.Validate(input);
        
        Assert.True(result);
    }
    
    [Fact]
    public void AgeValidator_ShouldReturnFalse_WhenAgeIsLessThan21()
    {
        DateTime input = DateTime.Now.AddYears(-20);
        
        bool result = AgeValidator.Validate(input);
        
        Assert.False(result);
    }
    
    [Fact]
    public void AgeValidator_ShouldReturnTrue_WhenAgeIsExactly21()
    {
        DateTime input = DateTime.Now.AddYears(-21);
        
        bool result = AgeValidator.Validate(input);
        
        Assert.True(result);
    }
}