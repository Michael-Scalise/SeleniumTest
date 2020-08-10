using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace sellib
{
    [TestFixture]
    public class Test1
    {
        private IWebDriver driver;
        public string testPage;

        [Test(Description = "Verify The Front Page.")]
        public void VerifyFrontPage()
        {
            driver.Navigate().GoToUrl(testPage);
            Assert.AreEqual(driver.Title, "ToolsQA");
        }

        //Elements
        [Test(Description = "Verify The Elements Link.")]
        public void VerifyElementsLink()
        {
            driver.Navigate().GoToUrl(testPage);
            driver.FindElement(By.XPath("//div[@class='category-cards']/div[1]")).Click();
            Assert.AreEqual(driver.Url, testPage + "/elements");
        }

        [Test(Description = "Verify The Text Boxes.")]
        public void VerifyTextBox()
        {
            driver.Navigate().GoToUrl(testPage + "/elements");
            driver.FindElement(By.Id("item-0")).Click();
            Assert.AreEqual(driver.Url, testPage + "/text-box");

            string TestName = "O'Shea O'Malley";
            string TestMail = "test@test.org";
            string TestAdd1 = "1234 O'Park Place";
            String TestAdd2 = "4321 O'Hara Drive";

            driver.FindElement(By.Id("userName")).SendKeys(TestName);
            driver.FindElement(By.Id("userEmail")).SendKeys(TestMail);
            driver.FindElement(By.Id("currentAddress")).SendKeys(TestAdd1);
            driver.FindElement(By.Id("permanentAddress")).SendKeys(TestAdd2);
            driver.FindElement(By.Id("submit")).Click();

            Assert.AreEqual(driver.FindElement(By.Id("name")).Text.Substring(5), TestName);
            Assert.AreEqual(driver.FindElement(By.Id("email")).Text.Substring(6), TestMail);
            Assert.AreEqual(driver.FindElement(By.Id("output")).FindElement(By.Id("currentAddress")).Text.Substring(17), TestAdd1); ;
            Assert.AreEqual(driver.FindElement(By.Id("output")).FindElement(By.Id("permanentAddress")).Text.Substring(20), TestAdd2);
        }

        [Test(Description = "Verify The Text Boxes Error Check.")]
        public void VerifyTextBoxError()
        {
            driver.Navigate().GoToUrl(testPage + "/elements");
            driver.FindElement(By.Id("item-0")).Click();
            Assert.AreEqual(driver.Url, testPage + "/text-box");

            string TestMail = "test";

            Assert.AreEqual(driver.FindElement(By.Id("userEmail")).GetAttribute("Class"), "mr-sm-2 form-control");
            driver.FindElement(By.Id("userEmail")).SendKeys(TestMail);
            driver.FindElement(By.Id("submit")).Click();
            Assert.AreEqual(driver.FindElement(By.Id("userEmail")).GetAttribute("Class"), "mr-sm-2 field-error form-control");

            driver.Navigate().Refresh();
            TestMail += "@";
            Assert.AreEqual(driver.FindElement(By.Id("userEmail")).GetAttribute("Class"), "mr-sm-2 form-control");
            driver.FindElement(By.Id("userEmail")).SendKeys(TestMail);
            driver.FindElement(By.Id("submit")).Click();
            Assert.AreEqual(driver.FindElement(By.Id("userEmail")).GetAttribute("Class"), "mr-sm-2 field-error form-control");

            driver.Navigate().Refresh();
            TestMail += "test";
            Assert.AreEqual(driver.FindElement(By.Id("userEmail")).GetAttribute("Class"), "mr-sm-2 form-control");
            driver.FindElement(By.Id("userEmail")).SendKeys(TestMail);
            driver.FindElement(By.Id("submit")).Click();
            Assert.AreEqual(driver.FindElement(By.Id("userEmail")).GetAttribute("Class"), "mr-sm-2 field-error form-control");

            driver.Navigate().Refresh();
            TestMail += ".com";
            Assert.AreEqual(driver.FindElement(By.Id("userEmail")).GetAttribute("Class"), "mr-sm-2 form-control");
            driver.FindElement(By.Id("userEmail")).SendKeys(TestMail);
            driver.FindElement(By.Id("submit")).Click();
            Assert.AreEqual(driver.FindElement(By.Id("userEmail")).GetAttribute("Class"), "mr-sm-2 form-control");

        }

        [Test(Description = "Verify The Check Boxes.")]
        public void VerifyCheckBox()
        {
            driver.Navigate().GoToUrl(testPage + "/elements");
            driver.FindElement(By.Id("item-1")).Click();
            Assert.AreEqual(driver.Url, testPage + "/checkbox");
        }

        [Test(Description = "Verify The Check Boxes Expand All.")]
        public void VerifyCheckBoxExpandAll()
        {
            driver.Navigate().GoToUrl(testPage + "/elements");
            driver.FindElement(By.Id("item-1")).Click();
            //The Desktop Node is note present before hitting Expand all, is after hitting it, and then disappears again after hitting Collapse All. 
            Assert.Zero(driver.FindElements(By.Id("tree-node-desktop")).Count());
            driver.FindElement(By.XPath("//button[@title='Expand all']")).Click();
            Assert.AreEqual(driver.FindElements(By.Id("tree-node-desktop")).Count(), 1);
            driver.FindElement(By.XPath("//button[@title='Collapse all']")).Click();
            Assert.Zero(driver.FindElements(By.Id("tree-node-desktop")).Count());
        }

        [Test(Description = "Verify The Check Boxes Actual Checking.")]
        public void VerifyCheckBoxChecking()
        {
            driver.Navigate().GoToUrl(testPage + "/elements");
            driver.FindElement(By.Id("item-1")).Click();

            //We're going to Expand All to test the actual check boxes. 
            driver.FindElement(By.XPath("//button[@title='Expand all']")).Click();  
           Assert.Zero(driver.FindElements(By.Id("result")).Count());

            //Excel File is unchecked by default, so verify that we can check it. 
            Assert.AreEqual("rct-icon rct-icon-uncheck", driver.FindElement(By.XPath("//label[@for='tree-node-excelFile']")).FindElement(By.ClassName("rct-checkbox")).FindElement(By.TagName("svg")).GetAttribute("class"));
            driver.FindElement(By.XPath("//label[@for='tree-node-excelFile']")).FindElement(By.ClassName("rct-checkbox")).Click();
            Assert.AreEqual("rct-icon rct-icon-check", driver.FindElement(By.XPath("//label[@for='tree-node-excelFile']")).FindElement(By.ClassName("rct-checkbox")).FindElement(By.TagName("svg")).GetAttribute("class"));
            Assert.AreEqual(driver.FindElement(By.Id("result")).FindElement(By.ClassName("text-success")).Text,"excelFile");

            //Verify that while Excel File is checked, Desktop displays the correct icon.
            Assert.AreEqual("rct-icon rct-icon-half-check", driver.FindElement(By.XPath("//label[@for='tree-node-downloads']")).FindElement(By.ClassName("rct-checkbox")).FindElement(By.TagName("svg")).GetAttribute("class"));
            //Click Downloads and verify that works correctly. 
            driver.FindElement(By.XPath("//label[@for='tree-node-downloads']")).FindElement(By.ClassName("rct-checkbox")).Click();
            Assert.AreEqual("rct-icon rct-icon-check", driver.FindElement(By.XPath("//label[@for='tree-node-downloads']")).FindElement(By.ClassName("rct-checkbox")).FindElement(By.TagName("svg")).GetAttribute("class"));
            Assert.AreEqual("rct-icon rct-icon-check", driver.FindElement(By.XPath("//label[@for='tree-node-wordFile']")).FindElement(By.ClassName("rct-checkbox")).FindElement(By.TagName("svg")).GetAttribute("class"));
            Assert.AreEqual(driver.FindElement(By.Id("result")).FindElements(By.ClassName("text-success"))[0].Text, "downloads");
            Assert.AreEqual(driver.FindElement(By.Id("result")).FindElements(By.ClassName("text-success"))[1].Text, "wordFile");
            Assert.AreEqual(driver.FindElement(By.Id("result")).FindElements(By.ClassName("text-success"))[2].Text, "excelFile");

            //While we're in this state, let's Collapse All and verify that the results are still correct.
            driver.FindElement(By.XPath("//button[@title='Collapse all']")).Click();
            Assert.AreEqual(driver.FindElement(By.Id("result")).FindElements(By.ClassName("text-success"))[0].Text, "downloads");
            Assert.AreEqual(driver.FindElement(By.Id("result")).FindElements(By.ClassName("text-success"))[1].Text, "wordFile");
            Assert.AreEqual(driver.FindElement(By.Id("result")).FindElements(By.ClassName("text-success"))[2].Text, "excelFile");
            Assert.AreEqual(driver.FindElement(By.Id("result")).FindElements(By.ClassName("text-success")).Count, 3);

            //Expand all, then verify that unchecking Downloads works correctly. 
            driver.FindElement(By.XPath("//button[@title='Expand all']")).Click();
            driver.FindElement(By.XPath("//label[@for='tree-node-downloads']")).FindElement(By.ClassName("rct-checkbox")).Click();
            Assert.Zero(driver.FindElements(By.Id("result")).Count());
            Assert.AreEqual("rct-icon rct-icon-uncheck", driver.FindElement(By.XPath("//label[@for='tree-node-excelFile']")).FindElement(By.ClassName("rct-checkbox")).FindElement(By.TagName("svg")).GetAttribute("class"));
            Assert.AreEqual("rct-icon rct-icon-uncheck", driver.FindElement(By.XPath("//label[@for='tree-node-downloads']")).FindElement(By.ClassName("rct-checkbox")).FindElement(By.TagName("svg")).GetAttribute("class"));
            Assert.AreEqual("rct-icon rct-icon-uncheck", driver.FindElement(By.XPath("//label[@for='tree-node-wordFile']")).FindElement(By.ClassName("rct-checkbox")).FindElement(By.TagName("svg")).GetAttribute("class"));

        }


        [Test(Description = "Verify The Forms Link.")]
        public void VerifyFormsLink()
        {
            driver.Navigate().GoToUrl(testPage);
            driver.FindElement(By.XPath("//div[@class='category-cards']/div[2]")).Click();
            Assert.AreEqual(driver.Url, testPage + "/forms");
        }

        [Test(Description = "Verify The Alerts Link.")]
        public void VerifyAlertsLink()
        {
            driver.Navigate().GoToUrl(testPage);
            driver.FindElement(By.XPath("//div[@class='category-cards']/div[3]")).Click();
            Assert.AreEqual(driver.Url, testPage + "/alertsWindows");
        }

        [Test(Description = "Verify The Widgets Link.")]
        public void VerifyWidgetsLink()
        {
            driver.Navigate().GoToUrl(testPage);
            driver.FindElement(By.XPath("//div[@class='category-cards']/div[4]")).Click();
            Assert.AreEqual(driver.Url, testPage + "/widgets");
        }

        [Test(Description = "Verify The Interaction Link.")]
        public void VerifyInteractionLink()
        {
            driver.Navigate().GoToUrl(testPage);
            driver.FindElement(By.XPath("//div[@class='category-cards']/div[5]")).Click();
            Assert.AreEqual(driver.Url, testPage + "/interaction");
        }


        [Test(Description = "Verify The Books Link.")]
        public void VerifyBooksLink()
        {
            driver.Navigate().GoToUrl(testPage);
            driver.FindElement(By.XPath("//div[@class='category-cards']/div[6]")).Click();
            Assert.AreEqual(driver.Url, testPage + "/books");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Close();
        }

        [SetUp]
        public void Setup()
        {
            testPage = "https://demoqa.com";
            driver = new ChromeDriver();
        }

    }
}