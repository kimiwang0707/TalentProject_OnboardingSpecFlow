using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Talent.Automation.Page.Employer.Onboarding;
using Talent.Automation.Steps.BaseStep;
using TechTalk.SpecFlow;

namespace Talent.Automation.Steps
{
    [Binding]
    public sealed class ManageArticleSteps : Base
    {
        private readonly ScenarioContext context;

        public ManageArticleSteps(IWebDriver driver, ScenarioContext injectedContext) : base(driver)
        {
            context = injectedContext;
        }

        [Given(@"I am on Article Management page")]
        public void GivenIAmOnArticleManagementPage()
        {
            CurrentPage = GetInstance<ManageArticlePage>(Driver);
        }



        [When(@"I I click on New Article button to add an article")]
        public void WhenIIClickOnNewArticleButtonToAddAnArticle()
        {
            CurrentPage.As<ManageArticlePage>().AddArticle();
        }


        [Then(@"I should be able to preview the new added article")]
        public void ThenIShouldBeAbleToPreviewTheNewAddedArticle()
        {
            CurrentPage.As<ManageArticlePage>().PreviewArticle();
        }


        [When(@"I I click on Edit button to edit an article")]
        public void WhenIIClickOnEditButtonToEditAnArticle()
        {
            CurrentPage.As<ManageArticlePage>().EditArticle();
        }

        [Then(@"I should be able to preview the new edited article")]
        public void ThenIShouldBeAbleToPreviewTheNewEditedArticle()
        {
            CurrentPage.As<ManageArticlePage>().PreviewArticle();
        }

        [When(@"I I click on Delete button to delete an article")]
        public void WhenIIClickOnDeleteButtonToDeleteAnArticle()
        {
            CurrentPage.As<ManageArticlePage>().DeleteArticle();
        }


        [Then(@"I should be able to verify that the article is deleted")]
        public void ThenIShouldBeAbleToVerifyThatTheArticleIsDeleted()
        {
            CurrentPage.As<ManageArticlePage>().VerifyDeleteArticle();
        }



    }
}
