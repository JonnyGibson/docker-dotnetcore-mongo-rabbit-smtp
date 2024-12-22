package main

import (
	"encoding/json"
	"fmt"
	"log"
	"os"
	"time"

	"github.com/streadway/amqp"
	"gopkg.in/gomail.v2"
)

type Recommendation struct {
	Author    string `json:"Author"`
	Country   string `json:"Country"`
	ImageLink string `json:"ImageLink"`
	Language  string `json:"Language"`
	Link      string `json:"Link"`
	Pages     int    `json:"Pages"`
	Title     string `json:"Title"`
	Year      int    `json:"Year"`
}

type RecommendationMessage struct {
	RecipientEmail string         `json:"RecipientEmail"`
	Recommendation Recommendation `json:"Recommendation"`
}

func connectWithRetry(host, user, pass string) (*amqp.Connection, error) {
	var conn *amqp.Connection
	var err error
	maxRetries := 10
	retryDelay := 5 * time.Second

	for i := 0; i < maxRetries; i++ {
		url := fmt.Sprintf("amqp://%s:%s@%s:5672/", user, pass, host)
		log.Printf("Attempting to connect to RabbitMQ (attempt %d/%d)...", i+1, maxRetries)
		conn, err = amqp.Dial(url)
		if err == nil {
			log.Printf("Successfully connected to RabbitMQ on attempt %d", i+1)
			return conn, nil
		}
		log.Printf("Failed to connect: %s. Retrying in %s...", err, retryDelay)
		time.Sleep(retryDelay)
	}
	return nil, fmt.Errorf("failed to connect after %d attempts: %v", maxRetries, err)
}

func main() {
	runnerName := "Go Listener"
	if len(os.Args) > 1 {
		runnerName = os.Args[1]
	}

	// Get RabbitMQ connection details from environment variables
	host := getEnv("RABBITMQ_HOST", "JGMQ")
	user := getEnv("RABBITMQ_USER", "store")
	pass := getEnv("RABBITMQ_PASSWORD", "password")

	// Connect to RabbitMQ with retry
	conn, err := connectWithRetry(host, user, pass)
	if err != nil {
		log.Fatalf("Failed to connect to RabbitMQ: %s", err)
	}
	defer conn.Close()

	ch, err := conn.Channel()
	if err != nil {
		log.Fatalf("Failed to open a channel: %s", err)
	}
	defer ch.Close()

	q, err := ch.QueueDeclare(
		"books", // name
		false,   // durable
		false,   // delete when unused
		false,   // exclusive
		false,   // no-wait
		nil,     // arguments
	)
	if err != nil {
		log.Fatalf("Failed to declare a queue: %s", err)
	}

	msgs, err := ch.Consume(
		q.Name, // queue
		"",     // consumer
		true,   // auto-ack
		false,  // exclusive
		false,  // no-local
		false,  // no-wait
		nil,    // args
	)
	if err != nil {
		log.Fatalf("Failed to register a consumer: %s", err)
	}

	log.Printf(" [*] Waiting for messages. To exit press CTRL+C")

	forever := make(chan bool)

	go func() {
		for d := range msgs {
			var msg RecommendationMessage
			err := json.Unmarshal(d.Body, &msg)
			if err != nil {
				log.Printf("Error parsing message: %s", err)
				continue
			}

			log.Printf(" [x] Received recommendation '%s' for %s", msg.Recommendation.Title, msg.RecipientEmail)
			sendEmail(msg, runnerName)
		}
	}()

	<-forever
}

func sendEmail(msg RecommendationMessage, runnerName string) {
	mailHost := getEnv("MAIL_HOST", "mail")
	mailPort := 1025 // MailHog default port

	m := gomail.NewMessage()
	m.SetHeader("From", "recs@engine.com")
	m.SetHeader("To", msg.RecipientEmail)
	m.SetHeader("Subject", fmt.Sprintf("Book Recommendation - %s (via %s)", msg.Recommendation.Title, runnerName))

	body := fmt.Sprintf(`
		<h2>Here is a book recommendation, please enjoy</h2>
		<h3>Brought to you by our recommendations team!</h3>
		<dl>
			<dt>Title</dt><dd>%s</dd>
			<dt>Author</dt><dd>%s</dd>
			<dt>Language</dt><dd>%s</dd>
			<dt>Pages</dt><dd>%d</dd>
			<dt>Wiki</dt><dd><a href="%s">%s</a></dd>
		</dl>
	`, msg.Recommendation.Title, msg.Recommendation.Author, msg.Recommendation.Language, 
	   msg.Recommendation.Pages, msg.Recommendation.Link, msg.Recommendation.Link)

	m.SetBody("text/html", body)

	d := gomail.NewDialer(mailHost, mailPort, "", "")
	if err := d.DialAndSend(m); err != nil {
		log.Printf("Error sending email: %s", err)
	} else {
		log.Printf(" [✓] Sent email for '%s' to %s", msg.Recommendation.Title, msg.RecipientEmail)
	}
}

func getEnv(key, fallback string) string {
	if value, exists := os.LookupEnv(key); exists {
		return value
	}
	return fallback
} 