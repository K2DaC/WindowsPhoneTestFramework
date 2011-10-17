#WindowsPhoneTestFramework

There is an introduction video on http://bit.ly/wp7-test

There are some Wiki Pages now on https://github.com/Expensify/WindowsPhoneTestFramework/wiki

			
##General setup

Before you try the general steps, consider using NuGet!

For adding BDD to a class library project, see https://github.com/Expensify/WindowsPhoneTestFramework/wiki/Writing-a-new-SpecFlow-test-project-for-WindowsPhoneTestFramework
			
For adding the test client to a WP7 project, see https://github.com/Expensify/WindowsPhoneTestFramework/wiki/Adding-testing-to-an-application

			
##NuGet setup - wp7 app

Use the "App - Windows Phone Test Framework" installer on nuget.org/List/Packages/WP7TestClient

Once you have installed from NuGet into your WP7 App, then:

1. in the App.xaml.cs constructor, add

	```
            #if DEBUG
            WindowsPhoneTestFramework.AutomationClient.Automation.Instance.Initialise();
            #endif // DEBUG
	```


##NuGet setup - Class Library test project (BDD)

Use the "BDD - Windows Phone Test Framework" installer on nuget.org/List/Packages/WP7Test

Once you have installed from NuGet into your test class library, then:

1. Change the project Build from "Any CPU" to "x86" only

2. Edit app.config to provide the necessary configuration values for your app

    - Be especially careful about the paths
	
	- For finding the ProductId, see the WMAppManifest.xml file.
	
3. Add a new feature:

	```
	Feature: App Test
		In order to test my app
		As a WP7 Developer
		I want to see it start and take a picture of it

	Scenario: Start the app
		Given my app is uninstalled
		And my app is installed
		And my app is running
		Then I wait 5 seconds
		Then take a picture
	```
	
4. Run the tests


#Prerequisites

To get this to work, you need to install:

- wp7 7.1 mango dev tools - so far not tested on the free Express versions

- nunit

- specflow


#Some possible problems:

- For non-English setups (outside of US, UK, AU, etc) you may find you are not able to create the emulator - this is due to internationalised emulator device names (currently I'm looking for a list).

	- There is now some code to help work around this - but if the emulator does not start in your SDK, then please get in touch.
	
	- If you want to try fixing this yourselves, take a look at the device name in DriverBase.cs and EmulatorDriver.cs

- For some script runners, then you may need to change script runner to have the 32-big flag set - try to find a 32-bit alternative (e.g. nunit-console-x86.exe) - or (at worst) use CorFlags.exe to change your test-runner.

	```
		"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\CorFlags.exe" "your target.exe" /32BIT+
	```

- The server part of the code opens a WCF service on http://localhost:8085 - it needs permission to do this - use:

	```
		 netsh http add urlacl url=http://+:8085/ user=<domain>\<user>
	```
	 
#Source code build

To start:

1. Open and build the whole solution - Debug configuration

2. Open a command prompt and run "cscript runspec.js" inside the Example directory - this will run all the specflow features

3. Try running the emuHost command line tool, then try commands like:

	```
        help
		install
		launch
		click Go!
		setText TextBoxInput=hello world
		getText TextBoxOutput
		doSwipe LeftToRight
	```


#Source code - using the test platform

To work out how to use the test platform in your own apps:

1. Try looking at the code for ExampleApp - there's only one line that's added to enable testing - `Automation.Instance.Initialise();` in App.xaml.cs

2. Try looking at the gherkin code in the ExampleApp.Spec features


#Questions

Please ask them on http://www.stackoverflow.com


#Contributing

Please do dive on in and help :)