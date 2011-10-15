Feature: ChildPage
    In order to use ExampleApp
    As a WP7 user
    I want to be enter text in the fields on the child pivot page

Background:
    Given my app is uninstalled
    Given my app is installed
    Given my app is not running
    Given my app is running
    Then I may see the text "Waiting..."
    Then I wait for the text "Go!" to appear
    Then I press the control "Go!"
    Then I wait for the text "item1" to appear

Scenario: Pivot Page fields are empty at start
    Then I see the control "TextBoxInput" contains ""
    And I see the control "TextBoxOutput" contains ""
    And take a picture

Scenario: The Pivot Page output field reverses what I type in input
    Then I see the control "TextBoxInput" contains ""
    And I see the control "TextBoxOutput" contains ""
    And take a picture
    Then I enter "Hello World" into the control "TextBoxInput"
    Then I see the control "TextBoxInput" contains "Hello World"
    And I see the control "TextBoxOutput" contains "dlroW olleH"
    Then take a picture

Scenario: An example broken test
    Then I see the control "TextBoxInput" contains ""
    And I see the control "TextBoxOutput" contains ""
    And take a picture
    Then I press the control "BreakThingsCheckBox"
    And take a picture
    Then I enter "Hello World" into the control "TextBoxInput"
    Then I see the control "TextBoxInput" contains "Hello World"
    Then take a picture
    And I see the control "TextBoxOutput" contains "dlroW olleH"

Scenario: The Pivot Page has a Pivot which responds to swipes
    Then I see the control "item1" is left of the control "item2"
    And I see the control "item2" is left of the control "item3"
    And I see "Input"
    #And I don't see "You are on pivot 2"
    #And I don't see "You are on pivot 3"
    Then I swipe "RightToLeft"
    And I wait 2 seconds
    Then I see the control "item2" is left of the control "item3"
    And I see the control "item3" is left of the control "item1"
    #And I don't see "Input"
    And I see the text "You are on pivot 2"
    #And I don't see "You are on pivot 3"
    Then I swipe "RightToLeft"
    And I wait 2 seconds
    Then I see the control "item3" is left of the control "item1"
    And I see the control "item1" is left of the control "item2"
    #And I don't see "Input"
    #And I don't see "You are on pivot 2"
    And I see the text "You are on pivot 3"
    Then I swipe "LeftToRight"
    And I wait 2 seconds
    Then I see the control "item2" is left of the control "item3"
    And I see the control "item3" is left of the control "item1"
    #And I don't see "Input"
    And I see the text "You are on pivot 2"
    #And I don't see "You are on pivot 3"
