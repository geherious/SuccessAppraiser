using Bogus;
using E2E.Common;
using FluentAssertions;
using OpenQA.Selenium;

namespace E2E;

public class AuthTests : BaseE2ETest
{
    [Fact]
    public async Task Register_Success()
    {
        // arrange
        await _webDriver.Navigate().GoToUrlAsync("http://localhost/register");

        string email = CreateEmail();
        string password = CreatePassword();
        string username = CreateUsername();
        
        // act
        _webDriver
            .FindElement(By.Id("email"))
            .SendKeys(email);
        
        _webDriver
            .FindElement(By.Id("username"))
            .SendKeys(username);
        
        _webDriver
            .FindElement(By.Id("password"))
            .SendKeys(password);
        
        _webDriver
            .FindElement(By.Id("confirm_pwd"))
            .SendKeys(password);
        
        _webDriver.FindElement(By.TagName("button")).Click();
        
        await Task.Delay(1000);

        // assert
        _webDriver
            .FindElement(By.TagName("h1"))
            .Text
            .Should()
            .Be("Success");
    }
    
    private string CreateEmail() => _faker.Person.Email;
    
    private string CreatePassword() =>
        _faker.Lorem.Word() + _faker.Lorem.Word() + _faker.Random.Int(9000, 10000);

    private string CreateUsername()
    {
        string username = _faker.Random.Guid().ToString();
        return "new" + username.Substring(0, Math.Min(username.Length, 20));
    }

    [Fact]
    public async Task Register_WhenEmpty_Fail()
    {
        // arrange
        await _webDriver.Navigate().GoToUrlAsync("http://localhost/register");
        
        // act
        // assert
        _webDriver
            .FindElement(By.TagName("button"))
            .Enabled
            .Should()
            .BeFalse();
    }

    [Fact]
    public async Task Login_Success()
    {
        // arrange
        await _webDriver.Navigate().GoToUrlAsync("http://localhost/register");

        string email = CreateEmail();
        string password = CreatePassword();
        string username = CreateUsername();
        
        _webDriver
            .FindElement(By.Id("email"))
            .SendKeys(email);
        
        _webDriver
            .FindElement(By.Id("username"))
            .SendKeys(username);
        
        _webDriver
            .FindElement(By.Id("password"))
            .SendKeys(password);
        
        _webDriver
            .FindElement(By.Id("confirm_pwd"))
            .SendKeys(password);
        
        _webDriver.FindElement(By.TagName("button")).Click();
        
        await Task.Delay(1000);
        
        await _webDriver.Navigate().GoToUrlAsync("http://localhost/login");
        
        // act
        _webDriver
            .FindElement(By.Id("email"))
            .SendKeys(email);
        
        _webDriver
            .FindElement(By.Id("password"))
            .SendKeys(password);
        
        _webDriver.FindElement(By.TagName("button")).Click();
        
        await Task.Delay(1000);
        
        // assert
        _webDriver.Url.Should().EndWith("/home");
    }

    [Fact]
    public async Task Login_WhenNotRegistered_Fail()
    {
        // arrange
        await _webDriver.Navigate().GoToUrlAsync("http://localhost/login");

        string email = CreateEmail();
        string password = CreatePassword();
        
        // act
        _webDriver
            .FindElement(By.Id("email"))
            .SendKeys(email);
        
        _webDriver
            .FindElement(By.Id("password"))
            .SendKeys(password);
        
        _webDriver.FindElement(By.TagName("button")).Click();

        await Task.Delay(1000);
            
        // assert
        _webDriver.Url.Should().EndWith("/login");
    }
}