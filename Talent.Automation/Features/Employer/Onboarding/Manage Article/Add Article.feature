Feature: Add Article
	As a employer,
	I can add article.

Background: Login as employer
	Given I login as 'employer'
	Then I should be navigated to 'dashBoard' page

	Given I navigate to 'manage article' page
	Then I should be navigated to 'article management' page

#------------Positive Scenarios---------------------#
Scenario: Add an article
Given I am on Article Management page
When I I click on New Article button to add an article
Then I should be able to preview the new added article

Scenario: Edit an article
Given I am on Article Management page
When I I click on Edit button to edit an article
Then I should be able to preview the new edited article

Scenario: Delete an article
Given I am on Article Management page
When I I click on Delete button to delete an article
Then I should be able to verify that the article is deleted


#------------End Postive Scenario-------------------#







#------------Negative Scenarios---------------------#






#------------End Negative Scenario------------------#

