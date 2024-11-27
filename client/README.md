# Client

This is client application for my [ToDo App](https://github.com/kuxulin/silver-octo-tribble)
This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 18.2.3.

## Installation

### Certificates

Project requires self signed certificate for appropriate work, create cert.key and cert.crt files and place them into root folder.
If you don`t know, how to do that, follow instructions in this section.

In order to use this project locally, you need to have [openssl](https://openssl-library.org/source/)

1. Open OpenSSL Command Prompt
2. Run this command
   `openssl req -x509 -newkey rsa:4096 -sha256 -days 365 -nodes -keyout cert.key -out cert.crt -subj /CN=<print name here> -addext "subjectAltName=DNS:localhost,IP:10.0.0.1"`
   It will create two files in openssl console folder. Paste them into your root folder
3. To avoid warning from browser, install .crt file to Trusted Root Certification Authorities folder

Run `npm i` to install all required dependencies
Run `ng serve` for a dev server. Navigate to `https://localhost:4200/`. The application will automatically reload if you change any of the source files.
