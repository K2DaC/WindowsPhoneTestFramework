Feature: MainPage
	In order to use ExampleApp
	As a WP7 user
	I want to be read the main page

Scenario: Main Page loads after a few seconds
	Given my app is clean installed and running
	Then take a picture
	Then I may see the text "Waiting..."
	Then I wait for the text "Go!" to appear
	Then take a picture

Scenario: Fast Application Switching works on Main Page
	Given my app is clean installed and running
	Then take a picture
	Then I may see the text "Waiting..."
	Then I wait for the text "Go!" to appear
	Then take a picture
	Then I go home
	Then I wait 1 seconds
	Then I press the back button for 2 seconds
	Then I wait 1 seconds
	Then I flick "LeftToRight"
	Then I wait 1 seconds
	Then I tap on the middle of the screen
	Then I wait 1 seconds
	Then take a picture
	Then my app is running

Scenario: Main Page provides a Go button that provides access to the ChildPivotPage
	Given my app is clean installed and running
	Then take a picture
	Then I may see the text "Waiting..."
	Then I wait for the text "Go!" to appear
	Then take a picture
	Then I press the control "Go!"
	Then take a picture
	Then I wait for the text "item1" to appear
	Then take a picture

Scenario: Back button works on Main Page
	Given my app is clean installed and running
	Then take a picture
	Then I may see the text "Waiting..."
	Then I wait for the text "Go!" to appear
	Then take a picture
	Then I go back
	Then I wait 2 seconds
	Then my app is not running

