using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BasicWebApp.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        private static IWebDriver _browser;
        private static ICollection<Person> _people;
        
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            _people = new List<Person>(new []
            {
                new Person
                {
                    FirstName = "John",
                    LastName = "Adams"
                },
                new Person
                {
                    FirstName = "Grover",
                    LastName = "Cleveland"
                }
            });

            _browser = TestStartup.GetBrowserFor("/");

            new WebDriverWait(_browser, TimeSpan.FromSeconds(10)).Until(
                ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector("#peopleTable > tbody > tr > td")));
        }

        [TestMethod]
        public void TableHasValues()
        {
            var tableRows = _browser.FindElements(By.CssSelector("#peopleTable > tbody > tr"));

            var pairs = _people.Zip(tableRows, (person, row) => new { person, row });

            foreach (var pair in pairs)
            {
                var firstNameCell = pair.row.FindElement(By.CssSelector("td:first-child"));
                var lastNameCell = pair.row.FindElement(By.CssSelector("td:last-child"));

                Assert.AreEqual(pair.person.FirstName, firstNameCell.Text);
                Assert.AreEqual(pair.person.LastName, lastNameCell.Text);
            }
        }

        [TestMethod]
        public void CanAddPerson()
        {
            var firstNameField = _browser.FindElement(By.Id("firstname"));
            var lastNameField = _browser.FindElement(By.Id("lastname"));
            var submitButton = _browser.FindElement(By.Id("submitButton"));

            firstNameField.SendKeys("Rutherford");
            lastNameField.SendKeys("Hayes");
            submitButton.Click();

            // wait?

            var tableRows = _browser.FindElements(By.CssSelector("#peopleTable > tbody > tr"));

            var foundHim = tableRows.Any(row => row.FindElement(By.CssSelector("td:first-child")).Text == "Rutherford"
                                             && row.FindElement(By.CssSelector("td:last-child")).Text == "Hayes");

            Assert.IsTrue(foundHim, "Could not find expected person \"Rutherford Hayes\"");
        }

        [TestMethod]
        public void TableCanBeSorted()
        {
            var table = _browser.FindElement(By.Id("peopleTable"));
            
            table.FindElement(By.CssSelector("thead > tr > th:first-child")).Click();

            var tableFirstNames =
                table.FindElements(By.CssSelector("tbody > tr > td:first-child")).Select(cell => cell.Text).ToList();
            
            Assert.IsTrue(tableFirstNames.SequenceEqual(tableFirstNames.OrderBy(name => name)), "First Names were not in order.");
        }
    }

    internal class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
