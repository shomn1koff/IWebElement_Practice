using OpenQA.Selenium;

namespace IWebElement_Test.Helpers
{
    public static class ScreenshotHelper
    {
        public static void CaptureScreenshot(string testName, IWebDriver driver, string screenshotDirectory)
        {
            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            string filePath = Path.Combine(screenshotDirectory, $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
            screenshot.SaveAsFile(filePath);
            Console.WriteLine($"Screenshot saved: {filePath}");
        }
    }
}
