using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumDemo
{
    class Program
    {
        static void Main()
        {
            var browser = new FirefoxDriver(); // or InternetExplorerDriver() or ChromeDriver()

            browser.Navigate().GoToUrl("https://www.google.com/");

            var search = browser.FindElement(By.Name("q"));

            search.SendKeys("Propak Logistics");

            var form = browser.FindElement(
                By.CssSelector("form[role=\"search\"]"));
            form.Submit();

            new WebDriverWait(browser, TimeSpan.FromSeconds(10)).Until(
                ExpectedConditions.ElementExists(By.ClassName("g")));

            var link = browser.FindElement(
                By.CssSelector("a[href*=\"www.propak.com\"]"));
            link.Click();
        }
    }
}
