Readme... details and wiki to follow...

There is an introduction on http://bit.ly/wp7-test

To get this to work, you need to install:

- wp7 mango dev tools (I've not tested with just the free Express versions)
- nunit
- specflow

Some possible gotchas:
- To get the nunit-console script to work, then you need to either be running on a 32-bit OS - or you need to change nunit-console to have the 32-big flag set - use CorFlags.exe to do this 
    "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\CorFlags.exe" "C:\Program Files (x86)\NUnit 2.5.10\bin\net-2.0\nunit-console.exe" /32BIT+
- The server part of the code opens a WCF service on http://localhost:8085 - it needs permission to do this - use:
     netsh http add urlacl url=http://+:8085/ user=<domain>\<user>

To start:

1 open and build the whole solution - Debug configuration
2 open a command prompt and run "cscript runspec.js" inside the Example directory - this will run all the specflow features
3 try running the emuHost command line tool, then try commands like:
    help
    install
    launch
    click Go!

To work out where to go next:

1. Try looking at the code for emuHost
2. Try looking at the code for ExampleApp
3. Try looking at the gherkin code in the ExampleApp.Spec features


That's all for now...

You can either wait for me to complete this readme... or you can dive on in and help :)