# Allgemeines

Für die Arbeit mit Microsoft Active Directory (AD), sowie OpenLDAP, empfehlen wir die Benutzung der Open-Source Bibliothek [*Novell*](https://github.com/dsbenghe/Novell.Directory.Ldap.NETStandard). Die Benutzung ist unter /src-23/BYTES.NET (Core)/NovellExampleCLI zu finden.

# Benutzung

Zum Einloggen eines Users per Novell braucht man zwei Befehle:

```
await using (var cn = new LdapConnection())
{
	await cn.ConnectAsync("<<hostname>>", 389);
	await cn.BindAsync("<<userdn>>", "<<userpassword>>");
}
```

Damit erstellt man eine neue Verbindung zu AD/ OpenLDAP. 

Bei der Benennung der Usernamen ist jedoch zu beachten, dass der Name nicht überall gleich sein kann. So arbeitet OpenLDAP mit Usernamen des Schemas `cn=admin,dc=test,dc=local`, wogegen AD Usernamen mit dem Schema `username@domain` aktzeptiert.

Danach kann man Anfragen an den Service stellen (unter Benutzung der `SearchAsync`-Funktion).

# OpenLDAP, Docker und Novell
Man kann OpenLDAP mit Docker lokal hosten. Dazu erstellt man eine `docker-compose.yml`-Datei:

```
services:
  openldap:
    image: osixia/openldap:1.5.0
    container_name: openldap
    environment:
      LDAP_ORGANISATION: "Test Company"
      LDAP_DOMAIN: "test.local"
      LDAP_ADMIN_PASSWORD: "admin"
    ports:
      - "389:389"
```
Der Port ist dabei als Standard-Port gewählt.
Nach starten des Containers kann man den OpenLDAP Server mit Daten füllen. Die Daten dazu schreibt man in eine Datei `users.ldif`:
```
dn: ou=users,dc=test,dc=local
objectClass: organizationalUnit
ou: users

dn: uid=max,ou=users,dc=test,dc=local
objectClass: inetOrgPerson
objectClass: organizationalPerson
objectClass: person
objectClass: top
cn: Max Mustermann
sn: Mustermann
uid: max
mail: max@test.local
userPassword: test123
```
Hier wird beispielsweise eine Organisation, sowie ein User angelegt. Wenn man nun in dem Ordner, wo die Datei gespeichert ist, ein Terminal öffnet, kann man per Befehl 
`docker cp "Path\to\users.ldif" openldap:/tmp/users.ldif` die Datei in den Speicher des Servers kopieren. 
Mit ausführen von `docker exec -it openldap ldapadd -x -D "cn=admin,dc=test,dc=local" -w admin -f /tmp/users.ldif` fügt der Server die Daten hinzu.
Danach kann man per `SearchAsync` die Daten abfragen.

**DAS FUNKTIONIERT NOCH NICHT GANZ. AKTUELL FUNKTIONIERT DAS HINZUFÜGEN VON DATEN NOCH NICHT.**