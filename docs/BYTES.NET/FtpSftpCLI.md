# FTP and SFTP support

To support both protocols, we use the packages
- [FluentFTP](https://github.com/robinrodricks/FluentFTP) for FTP
- [SSH.NET](https://github.com/sshnet/SSH.NET) for SFTP

The following documentation shows how to set up two docker container for FTP/SFTP services, and how to use the packages to work with (S)FTP servers.

## FTP
1. Create a docker-compose.yml file with the following content:

```
services:

  ftp:

    image: stilliard/pure-ftpd

    container_name: ftp-test

    ports:

        - "2121:21"

        - "30000-30009:30000-30009"

    environment:

      PUBLICHOST: localhost

      FTP_USER_NAME: testuser

      FTP_USER_PASS: testpass

      FTP_USER_HOME: /home/testuser

    volumes:

      - ./ftp-data:/home/testuser
```

2. Create a folder calles `ftp-data` at the same level as the docker-compose.yml. Now add some random files to the folder. The content or names do not matter. We only want something inside the folder to display later on.

3. Start the container with `docker compose up`.
You can now start the CLI. To connect to the FTP server, use these credentials:
- host: localhost
- username: testuser
- password: testpass

## SFTP with keyfile
1. Create a key. To do so, open a terminal and run the following code:
   `ssh-keygen -t ed25519`
2. Create a docker container for the SFTP server. For that, create a docker-compose.yml and add the content:

```
services:
  sftp:
    image: atmoz/sftp
    container_name: sftp-server
    ports:
      - "2222:22"
    command: testuser:testpass:1001
    volumes:
      - ./sftp-data:/home/testuser
      - ./keys/:/home/testuser/.ssh/keys/
```

Copy the public key into the .ssh folder. 

3. Start the container. You can connect to the server with these credentials:
- host: localhost
- username: testuser
- keyfile path: (path to file)
- passphrase: (passphrase)


The server is configured in a way that allows both authentication via key certificate or username and password. To authenticate via username and password, use these credentials:
- host: localhost
- username: testuser
- password: testpass