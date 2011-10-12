Feature: ChildPage
	In order to use ExampleApp
	As a WP7 user
	I want to be enter text in the fields on the child pivot page

Background:
	Given my app is uninstalled
	Given my app is installed
	Given my app is not running
	Given my app is running
	Then I wait for "Waiting..." to appear
	Then I wait for "Go!" to appear
	Then I press the "Go!" button
	Then I wait for "item1" to appear

Scenario: Pivot Page fields are empty at start
	Then I see the "TextBoxInput" field contains ""
	And I see the "TextBoxOutput" field contains ""
	And take a picture

Scenario: The Pivot Page output field reverses what I type in input
	Then I see the "TextBoxInput" field contains ""
	And I see the "TextBoxOutput" field contains ""
	And take a picture
	Then I enter "Hello World" into "TextBoxInput"
	Then I see the "TextBoxInput" field contains "Hello World"
	And I see the "TextBoxOutput" field contains "dlroW olleH"
	Then take a picture

Scenario: The Pivot Page has a Pivot which responds to swipes
	Then I see "item1" is left of "item2"
	And I see "item2" is left of "item3"
	And I see "Input"
	#And I don't see "You are on pivot 2"
	#And I don't see "You are on pivot 3"
	Then I swipe "RightToLeft"
	And I wait 2 seconds
	Then I see "item2" is left of "item3"
	And I see "item3" is left of "item1"
	#And I don't see "Input"
	And I see "You are on pivot 2"
	#And I don't see "You are on pivot 3"
	Then I swipe "RightToLeft"
	And I wait 2 seconds
	Then I see "item3" is left of "item1"
	And I see "item1" is left of "item2"
	#And I don't see "Input"
	#And I don't see "You are on pivot 2"
	And I see "You are on pivot 3"
	Then I swipe "LeftToRight"
	And I wait 2 seconds
	Then I see "item2" is left of "item3"
	And I see "item3" is left of "item1"
	#And I don't see "Input"
	And I see "You are on pivot 2"
	#And I don't see "You are on pivot 3"
