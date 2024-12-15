using Bogus;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace E2E.Common;

public class BaseE2ETest : IDisposable
{
    protected IWebDriver _webDriver;
    protected Faker _faker;

    public BaseE2ETest()
    {
        _webDriver = new ChromeDriver();
        _faker = new Faker();
    }

    public void Dispose()
    {
        _webDriver.Quit();
        _webDriver.Dispose();
    }
}