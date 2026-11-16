# General
For working with Microsofts Active Directory (AD) as well as OpenLDAP we recommend using the open-source library [*Novell*](https://github.com/dsbenghe/Novell.Directory.Ldap.NETStandard). An example of how to use it can be found under /src-23/BYTES.NET (Core)/NovellExampleCLI.

# Usage

Logging in a user via Novell requires two commands: 

```
await using (var cn = new LdapConnection())
{
    await cn.ConnectAsync("<<hostname>>", 389);
    await cn.BindAsync("<<userdn>>", "<<userpassword>>");
}
```

This creates a new connection to AD/OpenLDAP.

When naming users, note that the username format is not the same everywhere. OpenLDAP works with usernames following the schema `cn=admin,dc=test,dc=local`, whereas AD accepts usernames following the schema `username@domain`.

To send requests, we recommend using the `SearchAsync` function.

## Differences in naming conventions

OpenLDAP is stricter than AD when it comes to accepting usernames. To authenticate against OpenLDAP, you need the Distinguished Name in the form `cn=admin,dc=test,dc=local`. With AD, on the other hand, you can authenticate using `<domain>@<username>` or `<username>\<domain>`.

# OpenLDAP, Docker and Novell

You can host OpenLDAP locally using Docker. To do so, create a `docker-compose.yml` file:

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

The port is set to the default.

Further information on setting up OpenLDAP in a Docker container can be found on the official [GitHub page](https://github.com/osixia/container-openldap).