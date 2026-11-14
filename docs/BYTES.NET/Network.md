# Network Namespace
Die Funktionen im Network Namespace ermöglichen das Verbinden mit einer Fileshare, sowie [User Impersonation](https://learn.microsoft.com/de-de/power-apps/developer/data-platform/impersonate-another-user). 
Der Namespace besteht aus:
 - **RemoteConnection.cs**: Eine Klasse, die Informationen über die Verbindung zu einer Fileshare speichert
 - **RemoteFolderInfo.cs**: Eine Klasse, die Informationen über die Fileshare und die darin enthaltenen Daten selber speichert.
 - **RemoteRessource.cs**: Eine Klasse, die Informationen über die Ressourcen der Fileshare speichert. 

Die Anmeldeinformationen der Benutzer werden in unserer **UserInfo**-Klasse gespeichert, zu finden im IO-Namespace.

Im Projekt BYTES.NET.Tests sind unter IO/Network/TestRemoteFolderInfo.cs Anwendungsbeispiele für den Network-Namespace zu sehen, sowie wie man die Funktionen benutzt, um sich mit einer Fileshare zu verbinden, und einen User zu imitieren.

# HTTPClient
Im Projekt **HTTPClientCLI** findet man eine Implementierung, wie man die neue .net HTTPClient-Klasse verwenden kann, um mit Websites per HTTP-Anfragen zu kommunizieren.
