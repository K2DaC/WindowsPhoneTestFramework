REM "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\CorFlags.exe" "C:\Program Files (x86)\NUnit 2.5.10\bin\net-2.0\nunit-console.exe" 
REM Needs admin rights "C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\CorFlags.exe" "C:\Program Files (x86)\NUnit 2.5.10\bin\net-2.0\nunit-console.exe" /32BIT+
"C:\Program Files (x86)\NUnit 2.5.10\bin\net-2.0\nunit-console.exe" .\..\ExampleApp.Spec\bin\debug\ExampleApp.Spec.dll /labels /out=TestResult.txt /xml=TestResult.xml
"C:\Program Files (x86)\TechTalk\SpecFlow\specflow.exe" nunitexecutionreport ExampleApp.csproj
