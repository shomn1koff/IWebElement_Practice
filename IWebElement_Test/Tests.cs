using IWebElement_Builder;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace IWebElement_Test
{
    public class Tests
    {
        private SeleniumBuilder _builder;

        [SetUp]
        public void Setup()
        {
            _builder = new SeleniumBuilder();
        }

        [TearDown]
        public void Teardown()
        {
            _builder.Dispose();
        }

        [Test(Description = "Задание №1. Поиск элементов на странице")]
        public void Test1()
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
    }
}