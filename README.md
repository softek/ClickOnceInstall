# ClickOnceAppRunner

This tool downloads a ClickOnce application's files, then runs it with your command-line parameters.
This may come in handy when you want to run the software from a production and a test environment from the same computer.  With ClickOnce, an application can only be installed from one server or path.  This circumvents this undesireable standard behavior by downloaded the application manually.

**Note:** Your application must not depend on being a true network-deployed application.

### Usage

`ClickOnceAppRunner.exe http://server/my.application AppOption=abc ServerUrl=http://server/`

Example:

`ClickOnceAppRunner.exe http://illum-qa-sierra/clients/clinical.patient.application IlluminateServerUrl=http://illum-qa-sierra/`
