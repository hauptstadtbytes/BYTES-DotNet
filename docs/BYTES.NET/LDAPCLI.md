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

## Unterschied Benutzernamenkonvention
OpenLDAP ist beim aktzeptieren der Benutzernamen strenger als AD. Um sich bei OpenLDAP zu authentifizieren benötigt man den Distiguished Name in Form von `cn=admin,dc=test,dc=local`. Bei AD kann man sich hingegen mit `<domain>@<username>`oder `<username>\<domain>` authentifizieren.

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

Weitere Information zum Setup von OpenLDAP per Docker sind auf der offiziellen [GitHub-Seite](https://github.com/osixia/container-openldap) zu finden.