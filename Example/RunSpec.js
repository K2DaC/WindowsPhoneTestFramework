// ----------------------------------------------------------------------
// <copyright file="RunSpec.js" company="Expensify">
//     (c) Copyright Expensify. http://www.expensify.com
//     This source is subject to the Microsoft Public License (Ms-PL)
//     Please see license.txt on https://github.com/Expensify/WindowsPhoneTestFramework
//     All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com
// ------------------------------------------------------------------------

WScript.echo("Warning - if you are running this using WScript.exe instead of CScript.exe then expect a lot of message boxes!");

WScript.echo("Loading helpers...");

/* helper methods */

function twoDigits(input) {
	input = '' + input;
	while (input.length < 2)
		input = '0' + input;
	return input;
}

String.prototype.startsWith = function(str) {
	return (this.match("^"+str)==str);
}

String.prototype.endsWith = function(str) {
	return (this.match(str+"$")==str);
}

Date.prototype.prettyPrint = function () {
	return this.getFullYear() + '-' +
		twoDigits(this.getMonth()+1) + '-' +
		twoDigits(this.getDate())  + '-' +
		twoDigits(this.getHours())  + '-' +
		twoDigits(this.getMinutes())  + '-' +
		twoDigits(this.getSeconds());
}

function safeCreateFolder(folderPath) {
	if (!fso.FolderExists(folderPath))
		fso.CreateFolder(folderPath);
}

// define file constants
// Note: if a file exists, using forWriting will set
// the contents of the file to zero before writing to
// it. 
var forReading = 1, forWriting = 2, forAppending = 8;

function writeStringIntoFile(f, text) {
	// Open the file 
	os = f.OpenAsTextStream( forWriting, 0 );
	
	// write the text
	os.Write(text);
	
	//close the file
	os.Close();
}

function readFileIntoString(f) {
	// define array to store lines. 
	rline = new Array();

	// Open the file 
	is = f.OpenAsTextStream( forReading, 0 );
	
	// start and continue to read until we hit
	// the end of the file. 
	var count = 0;
	while( !is.AtEndOfStream ){
	   rline[count] = is.ReadLine();
	   count++;
	}
	// Close the stream 
	is.Close();
	// Place the contents of the array into 
	// a variable. 
	var msg = "";
	for(i = 0; i < rline.length; i++){
	   msg += rline[i] + "\n";
	}
	return msg;
}

/* end helpers */

WScript.echo("");
WScript.echo("======================");
WScript.echo("ExampleApp Test Runner");
WScript.echo("======================");
WScript.echo("");

var shell = WScript.CreateObject("WScript.Shell");
var baseDirectory = shell.CurrentDirectory;

WScript.echo("Setting up test results folder");

var fso = WScript.CreateObject("Scripting.FileSystemObject");
var testDirectory = baseDirectory + '/Test';
safeCreateFolder(testDirectory);

shell.CurrentDirectory = testDirectory;

var date =  new Date();
var datedTestDirectory = testDirectory + '/' + date.prettyPrint();
safeCreateFolder(datedTestDirectory);					
shell.CurrentDirectory = datedTestDirectory;

WScript.echo("Test results folder is " + datedTestDirectory);

WScript.echo('');
WScript.echo("======================");
WScript.echo('Running tests...');
WScript.echo('');
WScript.echo("======================");
WScript.echo('Now really running tests...');
WScript.echo('');

var batchCommand = '"' + baseDirectory + '/../packages/NUnit.2.5.10.11092/tools/nunit-console-x86.exe"'
                    + ' "' + baseDirectory + '/ExampleApp.Spec/bin/debug/ExampleApp.Spec.dll"'
					+ ' /labels /out=TestResult.txt /xml=TestResult.xml';
WScript.echo(batchCommand);
shell.run(batchCommand, 1 /* show normal */, true /* wait for this to finish*/);

WScript.echo('Test complete.');
WScript.echo('');
WScript.echo("======================");
WScript.echo(' Generating report...');
WScript.echo('');
var reportCommand = '"' + baseDirectory + '/../packages/SpecFlow.1.7.1/tools/specflow.exe"'
                    + ' nunitexecutionreport' 
					+ ' "' + baseDirectory + "/ExampleApp/ExampleApp.csproj";
shell.run(reportCommand, 1 /* show normal */, true /* wait for this to finish*/);

WScript.echo('Report complete.');
WScript.echo('');
WScript.echo("======================");
WScript.echo('Moving images...');
WScript.echo('');

var imageSpec = baseDirectory + "/ExampleApp.Spec/bin/debug/_EmuShot_*.png";
fso.MoveFile(imageSpec, datedTestDirectory);

WScript.echo('Images moved.');
WScript.echo('');
WScript.echo("======================");
WScript.echo('Manipulating image references in report...');
WScript.echo('');

var reportPath = datedTestDirectory + '/TestResult.html';
var reportFile = fso.GetFile(reportPath);
var reportText = readFileIntoString(reportFile);
reportText = reportText.replace( /_startEmuShot_/g, '<img src="');
reportText = reportText.replace( /_endEmuShot_/g, '" height="200">');
writeStringIntoFile(reportFile, reportText);

WScript.echo('Report image references corrected.');
WScript.echo('');
WScript.echo("======================");
WScript.echo('Complete!');
WScript.echo('');


if (WScript.Arguments.Count() == 0 || WScript.Arguments[0] != '/s') {
    WScript.echo('Opening test result file...');
	shell.run(reportPath);
}

/*
This code is NBG!

var imageDirectory = baseDirectory + "/ExampleApp.Spec/bin/debug/";
var folder = fso.GetFolder(imageDirectory);
var countMoved = 0;
for (var item in folder.Files){
	if (item.Name.endsWith('.png') && item.Name.startsWith('_EmuShot_')) {
		item.Move(datedTestDirectory + '/' + item.Name);
		countMoved++;
	}
}
WScript.echo(countMoved + ' image(s) moved.');
*/
