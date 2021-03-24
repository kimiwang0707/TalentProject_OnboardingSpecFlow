using MVPStudio.Framework.Extensions;
using MVPStudio.Framework.Helps.Excel;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Talent.Automation.Steps.BaseStep;

namespace Talent.Automation.Page.Employer.Onboarding
{
    public class ManageArticlePage : Base
    {
        private readonly ExcelData talentOnboarding;
        private string articleName;
        private string articleDescription;
        private string articleDefaultPoint;
        private string articleType;
        private string tags;
        private string article;
        private int qtyRecords;
        private int qtyRecordsAfterDeletion;


        public ManageArticlePage(IWebDriver driver) : base(driver)
        {
            // Set the excel
            ExcelUtil.SetDataSource("article.xlsx");
            talentOnboarding = ExcelUtil.DataSet.SelectSheet("manage_articles");
        }


        #region Initalize the WebElements 
        private IWebElement OnboardingTab => Driver.WaitForElement(By.LinkText("Onboarding"));
        private IWebElement ManageArticleTab => Driver.WaitForElement(By.LinkText("Manage Article"));
        private IWebElement NewArticleBtn => Driver.WaitForElement(By.ClassName("ant-btn-primary"));
        private IWebElement NewArticleCancelBtn => Driver.WaitForElement(By.XPath("//*[@id='root']//main//div[1]/div/button"));
        private IWebElement TypeName => Driver.WaitForElement(By.Name("name"));
        private IWebElement TypeDescription => Driver.WaitForElement(By.Name("description"));
        private IWebElement TypeDefaultPoint => Driver.WaitForElement(By.Name("defaultPoints"));
        private IWebElement ChooseLibraryType => Driver.WaitForElement(By.Id("articleForm_libraryType"));
        private IWebElement TypeTag => Driver.WaitForElement(By.CssSelector("input[placeholder='Press enter']"));
        private IWebElement TypeArticle => Driver.WaitForElement(By.CssSelector("div[data-placeholder='Write your article here...']"));
        private IWebElement PublishBtn => Driver.WaitForElement(By.ClassName("ant-btn-primary"));
        private IWebElement PreviewFirstArticleBtn => Driver.WaitForElement(By.XPath("(//*[text()='Preview'])[1]"));
        private IWebElement PreviewCloseBtn => Driver.WaitForElement(By.CssSelector("button[class='ant-btn']"));
        private IWebElement PreviewTitle => Driver.WaitForElement(By.XPath("//*[@id='rcDialogTitle0']/div/div[1]/h3"));
        private IWebElement PreviewDescription => Driver.WaitForElement(By.XPath("//*[@id='rcDialogTitle0']/div/div[1]/span"));
        private IWebElement PreviewDefaultPoints => Driver.WaitForElement(By.XPath("//*[@id='rcDialogTitle0']//div[2]/span"));
        private IWebElement PreviewTags => Driver.WaitForElement(By.ClassName("ant-tag"));
        private IWebElement PreviewArticleDetails => Driver.WaitForElement(By.XPath("//*[@class='ant-modal-body']//p"));
        private IWebElement EditFirstArticleBtn => Driver.WaitForElement(By.XPath("(//*[text()='Edit'])[1]"));
        private IWebElement DeleteTagsBtn => Driver.WaitForElement(By.XPath("//i[@class='anticon anticon-close']"));
        private IWebElement EditCancelBtn => Driver.WaitForElement(By.XPath("//*[@id='root']//main//div[1]/div/button"));
        private IWebElement DeleteFirstArticleBtn => Driver.WaitForElement(By.XPath("(//*[text()='Delete'])[1]"));
        private IWebElement DeleteConfirmBtnNo => Driver.WaitForElement(By.CssSelector("div[class='ant-popover-buttons']>button:nth-child(1)"));
        private IWebElement DeleteConfirmBtnYes => Driver.WaitForElement(By.CssSelector("div[class='ant-popover-buttons']>button:nth-child(2)"));

        #endregion


        public string ManageArticlePageTitle()
        {
            return Driver.Title;
        }



        #region Method for adding a new article
        public void AddArticle()
        {
            // Get value from excel
            var talentOnboardingAddArticle = talentOnboarding.GetRowByKey("create");

            // Declare the article details in Excel
            articleName = talentOnboardingAddArticle.GetValue("Name");
            articleDescription = talentOnboardingAddArticle.GetValue("Description");
            articleDefaultPoint = talentOnboardingAddArticle.GetValue("DefaultPoints");
            articleType = talentOnboardingAddArticle.GetValue("LibraryType");
            tags = talentOnboardingAddArticle.GetValue("Tag");
            article = talentOnboardingAddArticle.GetValue("Article");

            // Click on New Article Button
            Driver.WaitForClickable(By.ClassName("ant-btn-primary"));
            NewArticleBtn.Click();

            // Input Article details from Excel
            TypeName.WaitForClickable(Driver);
            TypeName.EnterText(articleName);

            TypeDescription.WaitForClickable(Driver);
            TypeDescription.EnterText(articleDescription);

            TypeDefaultPoint.WaitForClickable(Driver);
            TypeDefaultPoint.EnterText(articleDefaultPoint);

            ChooseLibraryType.WaitForClickable(Driver);
            ChooseLibraryType.Click();
            Driver.FindElement(By.XPath("//*[text()='" + articleType + "']")).WaitForClickable(Driver);
            Driver.FindElement(By.XPath("//*[text()='" + articleType + "']")).Click();

            EnterTags();

            TypeArticle.SendKeys(Keys.PageDown);
            TypeArticle.WaitForClickable(Driver);
            TypeArticle.EnterText(article);

            // Click on Publish
            PublishBtn.Click();

        }

        #endregion

        #region Method for preview an article
        public void PreviewArticle()
        {
            // Click Preview of the first article
            Driver.WaitForElementsToBeClickable(By.XPath("//*[text()='Preview']"));
            //PreviewFirstArticleBtn.WaitForClickable(Driver); (Doesnt work with this syntax)
            PreviewFirstArticleBtn.Click();

            // Compare the first article details is same as the record in Excel
            PreviewTitle.WaitForTextLoaded(Driver);
            Assert.That(articleName, Is.EqualTo(PreviewTitle.GetInnerText(Driver)));

            PreviewDescription.WaitForTextLoaded(Driver);
            Assert.That(articleDescription, Is.EqualTo(PreviewDescription.GetInnerText(Driver)));

            PreviewTags.WaitForTextLoaded(Driver);
            Assert.That(tags, Contains.Substring(PreviewTags.GetInnerText(Driver)));

            PreviewDefaultPoints.WaitForTextLoaded(Driver);
            Assert.That(articleDefaultPoint, Is.EqualTo(PreviewDefaultPoints.GetInnerText(Driver).Remove(0, 8)));

            PreviewArticleDetails.WaitForTextLoaded(Driver);
            Assert.That(article, Contains.Substring(PreviewArticleDetails.GetInnerText(Driver)));
        }
        #endregion

        #region Method for editing an article
        public void EditArticle()
        {
            // Get value from excel
            var talentOnboardingAddArticle = talentOnboarding.GetRowByKey("edit");

            // Declare the article details in Excel
            articleName = talentOnboardingAddArticle.GetValue("Name");
            articleDescription = talentOnboardingAddArticle.GetValue("Description");
            articleDefaultPoint = talentOnboardingAddArticle.GetValue("DefaultPoints");
            articleType = talentOnboardingAddArticle.GetValue("LibraryType");
            tags = talentOnboardingAddArticle.GetValue("Tag");
            article = talentOnboardingAddArticle.GetValue("Article");

            // Click on Edit Button
            Driver.WaitForElementsToBeClickable((By)By.XPath("//*[text()='Edit']"), 20);
            EditFirstArticleBtn.Click();

            // Input Article details from Excel
            TypeName.WaitForClickable(Driver);
            TypeName.Clear();
            TypeName.WaitForClickable(Driver);
            TypeName.EnterText(articleName);

            TypeDescription.WaitForClickable(Driver);
            TypeDescription.Clear();
            TypeDescription.WaitForClickable(Driver);
            TypeDescription.EnterText(articleDescription);

            TypeDefaultPoint.WaitForClickable(Driver);
            TypeDefaultPoint.Clear();
            TypeDefaultPoint.WaitForClickable(Driver);
            TypeDefaultPoint.EnterText(articleDefaultPoint);

            ChooseLibraryType.WaitForClickable(Driver);
            ChooseLibraryType.Click();
            Driver.FindElement(By.XPath("//*[text()='" + articleType + "']")).WaitForClickable(Driver);
            Driver.FindElement(By.XPath("//*[text()='" + articleType + "']")).Click();

            DeleteTags();
            EnterTags();

            TypeArticle.SendKeys(Keys.PageDown);
            TypeArticle.Clear();
            TypeArticle.WaitForClickable(Driver);           
            TypeArticle.EnterText(article);

            // Click on Publish
            PublishBtn.Click();

        }
        #endregion

        #region Method for deleting an article
        public void DeleteArticle()
        {
            // Wait element
            Driver.WaitForElementsToBeClickable(By.XPath("//*[text()='Delete']"));

            // Record the quantity of records on papge
            var records = Driver.FindElements(By.XPath("//*[@class='ant-list-item-meta-title']"));
            qtyRecords = records.Count;

            // Delete
            DeleteFirstArticleBtn.Click();
            DeleteConfirmBtnYes.Click();

            // Record the updated quantity
            Driver.WaitForElements(By.XPath("//*[contains(text(), 'Removed article')]"));
            //Driver.WaitForElement(By.XPath("//*[contains(text(), 'Removed article')]"));
            var recordsAfterDeletion = Driver.FindElements(By.XPath("//*[@class='ant-list-item-meta-title']"));
            qtyRecordsAfterDeletion = recordsAfterDeletion.Count;

        }

        public void VerifyDeleteArticle()
        {
            Debug.WriteLine("***QTY RECORDS***:"+ qtyRecords + "***After deletion***:" + qtyRecordsAfterDeletion);
            Assert.AreEqual(qtyRecords-1,qtyRecordsAfterDeletion);

        }
        #endregion




        //Method for enter tags
        public void EnterTags()
        {
            string tagsExcel = talentOnboarding.GetValue("Tag");

            string[] tags = tagsExcel.Split(',');
            for (int count = 0; count < tags.Length; count++)
            {
                TypeTag.WaitForClickable(Driver);
                TypeTag.EnterText(tags[count]);
                TypeTag.SendKeys(Keys.Enter);
            }
        }

        //Method for delete tags
        public void DeleteTags()
        {
            var tags = Driver.FindElements(By.XPath("//i[@class='anticon anticon-close']"));
            int tagsQty = tags.Count();

            for (int count = 1; count < tagsQty; count++)
            {
                DeleteTagsBtn.Click();
            }

        }

    }
}