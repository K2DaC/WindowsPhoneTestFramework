WindowsPhoneTestFramework
=========================

There is an introduction video on 

             http://bit.ly/wp7-test

There are some Wiki Pages now on

			https://github.com/Expensify/WindowsPhoneTestFramework/wiki
			

Prerequisites
-------------

To get this to work, you need to install:

- wp7 7.1 mango dev tools - so far not tested on the free Express versions

- nunit

- specflow


Some possible problems:

- For some script runners, then you may need to change script runner to have the 32-big flag set - try to find a 32-bit alternative (e.g. nunit-console-x86.exe) - or (at worst) use CorFlags.exe to change your test-runner.

    "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\CorFlags.exe" "your target.exe" /32BIT+

- The server part of the code opens a WCF service on http://localhost:8085 - it needs permission to do this - use:

     netsh http add urlacl url=http://+:8085/ user=<domain>\<user>

	 
QuickStart
----------

To start:

1. Open and build the whole solution - Debug configuration

2. Open a command prompt and run "cscript runspec.js" inside the Example directory - this will run all the specflow features

3. Try running the emuHost command line tool, then try commands like:

    help
    install
    launch
    click Go!
	setText TextBoxInput=hello world
	getText TextBoxOutput
	doSwipe LeftToRight

	
Using the test platform
-----------------------

To work out how to use the test platform in your own apps:

1. Try looking at the code for ExampleApp - there's only one line that's added to enable testing - `Automation.Instance.Initialise();` in App.xaml.cs

2. Try looking at the gherkin code in the ExampleApp.Spec features


Questions
-----------

Please ask them on http://www.stackoverflow.com

Contributing
-----------

Please do dive on in and help :)