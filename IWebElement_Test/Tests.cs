using IWebElement_Builder;
using IWebElement_Test.Helpers;
using OpenQA.Selenium;

namespace IWebElement_Test
{
    public class Tests
    {
        private SeleniumBuilder _builder;
        private string screenshotDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");


        [SetUp]
        public void Setup()
        {
            _builder = new SeleniumBuilder();

            if (!Directory.Exists(screenshotDirectory))
            {
                Directory.CreateDirectory(screenshotDirectory);
            }
        }

        [TearDown]
        public void Teardown()
        {
            _builder.Dispose();
        }

        [Test(Description = "Задание №1. Поиск элементов на странице")]
        public void FindElementsTest()
        {
            IWebDriver driver = _builder
                .WithURL("https://ib.psbank.ru/store/products/family-mortgage-program")
                .WithTimeout(TimeSpan.FromSeconds(10))
                .Build();

            IWebElement mortgageObjectDropdown = driver.FindElement(By.XPath("//mat-select[contains(@data-testid, 'calc-input-mortgageCreditType')]"));
            Assert.IsTrue(mortgageObjectDropdown.Displayed, "mortgageObjectDropdown not visible");

            IWebElement fillByGosuslugiButton = driver.FindElement(By.XPath("//span[contains(text(), 'Заполнить через Госуслуги')]/ancestor::button[contains(@class, 'mortgage-calculator-output-submit__button')]"));
            Assert.IsTrue(fillByGosuslugiButton.Displayed, "fillByGosuslugiButton not visible");

            IWebElement familyMortgageCard = driver.FindElement(By.XPath("//div[contains(text(), 'Семейная ипотека')]/parent::div[contains(@class, 'brands-cards__item')]"));
            Assert.IsTrue(familyMortgageCard.Displayed, "familyMortgageCard not visible");

            IWebElement lifeInsuranceSwitcher = driver.FindElement(By.XPath("//psb-text[contains(text(), 'Страхование жизни')]/ancestor::span/preceding-sibling::span[contains(@class, 'slider')]"));
            Assert.IsTrue(lifeInsuranceSwitcher.Displayed, "lifeInsuranceSwitcher not visible");

            IWebElement loanTermField = driver.FindElement(By.XPath("//label[contains(text(), 'Срок кредита')]/ancestor::rui-form-field-label/following-sibling::input"));
            Assert.IsTrue(loanTermField.Displayed, "loanTermField not visible");

        }

        [Test(Description = "Скрин при провальном тесте")]
        public void FailTestScreenshot()
        {
            IWebDriver driver = _builder
                .WithURL("https://ib.psbank.ru/store/products/military-family-mortgage-program")
                .WithTimeout(TimeSpan.FromSeconds(10))
                .Build();

            try
            {
                IWebElement nonExistElement = driver.FindElement(By.XPath("/*[@data-testid]"));
            }
            catch (NoSuchElementException)
            {
                ScreenshotHelper.CaptureScreenshot("Element not found", driver, screenshotDirectory);
                Assert.Fail("Test failed due to element not found.");
            }

        }
    }
}