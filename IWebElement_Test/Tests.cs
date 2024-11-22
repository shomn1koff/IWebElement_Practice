using IWebElement_Builder;
using IWebElement_Test.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace IWebElement_Test
{
    public class Tests
    {
        private SeleniumBuilder _builder;
        private string _screenshotDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");


        [SetUp]
        public void Setup()
        {
            _builder = new SeleniumBuilder();
            if (!Directory.Exists(_screenshotDirectory))
            {
                Directory.CreateDirectory(_screenshotDirectory);
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
                ScreenshotHelper.CaptureScreenshot("Element not found", driver, _screenshotDirectory);
                Assert.Fail("Test failed due to element not found.");
            }

        }

        [Test(Description = "Get attributes test")]
        public void GetAttributesTest()
        {
            IWebDriver driver = _builder
                .WithURL("https://ib.psbank.ru/store/products/military-family-mortgage-program-refinancing")
                .WithTimeout(TimeSpan.FromSeconds(10))
                .Build();

            IWebElement mortgageObjectDropdown = driver.FindElement(By.XPath("//mat-select[contains(@data-testid, 'calc-input-mortgageCreditType')]"));
            Assert.IsTrue(mortgageObjectDropdown.Enabled, "mortgageObjectDropdown not active");
            Assert.IsTrue(mortgageObjectDropdown.Displayed, "mortgageObjectDropdown not visible");

            var objectDropdownValue = mortgageObjectDropdown.Text;
            Console.WriteLine($"Selected: {objectDropdownValue}");
            Assert.That(objectDropdownValue, Is.EqualTo("Квартира в строящемся доме"), "not equals");

            var isMortgageDropdownExpanded = mortgageObjectDropdown.GetAttribute("aria-expanded");
            Assert.That(isMortgageDropdownExpanded, Is.EqualTo("false"), "Dropdown expanded");

            IWebElement familyMilitaryMortgageCard = driver.FindElement(By.XPath("//div[contains(text(), 'Семейная военная ипотека')]/parent::div[contains(@class, 'brands-cards__item')]"));
            Assert.IsTrue(familyMilitaryMortgageCard.Enabled, "mortgageObjectDropdown not active");
            Assert.IsTrue(familyMilitaryMortgageCard.Displayed, "familyMilitaryMortgageCard not visible");

            var isFamilyMilitaryCardEnabled = familyMilitaryMortgageCard.GetAttribute("class").Contains("_active");
            Assert.IsTrue(isFamilyMilitaryCardEnabled, "card not enabled");
        }

        [Test(Description = "Explicit wait test")]
        public void ExplicitWaitTest()
        {
            IWebDriver driver = _builder
                .WithURL("https://ib.psbank.ru/store/products/military-family-mortgage-program-refinancing")
                .WithTimeout(TimeSpan.FromSeconds(10))
                .Build();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//h3[contains(text(), 'Расчёт военной ипотеки')]/preceding-sibling::psb-loader[contains(@class, 'mortgage-calculator__loader')]")));

            var fillWithoutGosuslugiButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[contains(text(), 'Заполнить без Госуслуг')]/ancestor::button[contains(@class, 'mortgage-calculator-output-submit__button')]")));

            fillWithoutGosuslugiButton.Click();

            var notAllFieldsFilledAlert = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(text(), 'Оформление заявки станет доступным после заполнения обязательных полей')][contains(@class, 'mortgage-calculator-output__alert_show')]")));
            Assert.That(notAllFieldsFilledAlert?.Text.Contains("Оформление заявки станет доступным после заполнения обязательных полей"), Is.True, "Сообщение не соответствует ожидаемому");

            // != null для приведения типов, Invisible возвращает bool, Visible возвращает IWebElement
            bool isFillWithoutGosuslugiVisibleAndErrorHidden =
                wait.Until(
                    ExpectedConditions.ElementIsVisible(
                        By.XPath("//span[contains(text(), 'Заполнить без Госуслуг')]/ancestor::button[contains(@class, 'mortgage-calculator-output-submit__button')]"))
                    ) != null &&
                wait.Until(
                    ExpectedConditions.InvisibilityOfElementLocated(
                        By.XPath(
                            "//div[contains(text(), 'Оформление заявки станет доступным после заполнения обязательных полей')][contains(@class, 'mortgage-calculator-output__alert_show')]"
                        )));
            Assert.That(isFillWithoutGosuslugiVisibleAndErrorHidden, Is.True, "Ошибка не скрылась / кнопка не появилась");
        }
    }
}