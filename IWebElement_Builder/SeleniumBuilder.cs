using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V130.DOMSnapshot;
using System.Net;

namespace IWebElement_Builder
{
    public class SeleniumBuilder : IDisposable
    {
        private IWebDriver WebDriver { get; set; }
        public int Port { get; private set; }
        public bool IsDisposed { get; private set; }
        public List<string> ChangedArguments { get; private set; }
        public Dictionary<string, object> ChangedUserOptions { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public string StartingURL { get; private set; }

        public bool IsHeadless { get; private set; } = false;

        public IWebDriver Build()
        {
            var options = new ChromeOptions();

            if (IsHeadless)
            {
                options.AddArgument("--headless=new");
            }

            var service = ChromeDriverService.CreateDefaultService();

            if (Port > 0)
            {
                service.Port = Port; 
            }

            WebDriver = new ChromeDriver(service, options);

            WebDriver.Manage().Timeouts().ImplicitWait = Timeout;

            if (!string.IsNullOrEmpty(StartingURL))
            {
                WebDriver.Navigate().GoToUrl(StartingURL);
            }

            return WebDriver;
        }

        public void Dispose()
        {
            if (!IsDisposed)
            {
                WebDriver.Quit();
                GC.SuppressFinalize(this);
                IsDisposed = true;
            }
        }

        public SeleniumBuilder WithURL(string url)
        {
            StartingURL = url;
            return this;
        }

        public SeleniumBuilder WithTimeout(TimeSpan timeout)
        {
            Timeout = timeout;
            return this;
        }

        public SeleniumBuilder WithHeadless(bool headless)
        {
            IsHeadless = headless;
            return this;
        }

        public SeleniumBuilder ChangePort(int port)
        {
            Port = port;
            return this;
        }

        public SeleniumBuilder SetArgument(string argument)
        {
            ChangedArguments.Add(argument);
            return this;
        }

        public SeleniumBuilder SetUserOption(string option, object value)
        {
            //Реализовать добавление пользовательской настройки.
            //Все изменения сохранить в свойстве ChangedUserOptions
            //Builder в данном методе должен возвращать сам себя
            ChangedUserOptions[option] = value;
            return this;
        }

    }
}
