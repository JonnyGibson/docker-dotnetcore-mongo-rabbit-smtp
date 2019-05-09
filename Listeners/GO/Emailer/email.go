package main

import "gopkg.in/gomail.v2"

func sendEmail() {
    m := gomail.NewMessage()
    m.SetHeader("From", "from@example.com")
    m.SetHeader("To", "to@example.com")
    m.SetHeader("Subject", "Hello!")
    m.SetBody("text/plain", "What's up?")

    d := gomail.NewPlainDialer("localhost", 1025,"","")
    if err := d.DialAndSend(m); err != nil {
        panic(err)
    }
}