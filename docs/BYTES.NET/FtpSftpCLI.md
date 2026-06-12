# FTP/ SFTP Docker Container

## FTP
1. Erstellen der docker-compose.yml mit Inhalt:

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

2. Hinzufügen von Dateien in den `ftp-data`-Ordner (und den ftp-data Ordner selber)
3. Nun kann man die CLI ausführen. Die Anmeldedaten sind dabei:
- host: localhost
- username: testuser
- password: testpass

## SFTP mit Zertifikat
1. Keypaar erstellen
   ssh-keygen -t ed25519
2. Docker Container erstellen
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

Der public key wird in das .ssh-Verzeichnis des Containers kopiert. 
Nun kann man sich mit 
- host: localhost
- username: testuser
- keyfile path: (path to file)
- passphrase: (passphrase)
anmelden.

Aktuell ist der Container so konfiguriert, dass man sich mit oder ohne Zertifikat anmelden kann.
Bei dem selben Server kann man sich ebenfalls gleichzeitig mit 
- host: localhost
- username: testuser
- password: testpass
anmelden.