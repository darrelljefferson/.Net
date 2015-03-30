==========================================================
C#.NET 2.0 quickstart instructions:
==========================================================
Open Visual studio 2005 & create new project.

Add Web reference http://YOURSERVER:82/?wsdl
Name it lmapiSoap

------C#---------
// Initialize listmanager soap object
lm = new lmapiSoap.lmapi();

// setup basic authorization 
String username = "admin";
String password = "lyris";
lm.Credentials = new System.Net.NetworkCredential(username, password);

// Display current API version
MessageBox.Show("Current API Version: "+ lm.ApiVersion()); 

// verify API functions work...
MessageBox.Show(lm.CurrentUserEmailAddress());



==========================================================
Running Lyris Listmanager Client Tests Using C#.NET.
==========================================================

1) Unzip project file

2) Open "Lyris Listmanager SOAP API Tests.csproj"

3) Update your web reference to your server (if your reference is different from http://localhost:82/?wsdl)
http://YOURSERVER:82/?wsdl

3a) name the web reference "lmapiSoap"

4) You can now run the test suite by choosing Debug > Start.
