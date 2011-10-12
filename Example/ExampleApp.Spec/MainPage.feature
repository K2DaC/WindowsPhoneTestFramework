Feature: MainPage
	In order to use ExampleApp
	As a WP7 user
	I want to be read the main page

Scenario: Main Page loads after a few seconds
	Given my app is uninstalled
	Given my app is installed
	Given my app is not running
	Given my app is running
	Then I wait for "Waiting..." to appear
	Then take a picture
	Then I wait for "Go!" to appear
	Then take a picture

Scenario: Main Page provides a Go button
	Given my app is uninstalled
	Given my app is installed
	Given my app is not running
	Given my app is running
	Then I wait for "Waiting..." to appear
	Then I wait for "Go!" to appear
	Then take a picture
	Then I press the "Go!" button
	Then I wait for "item1" to appear
	Then take a picture
