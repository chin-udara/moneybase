# Moneybase - Take home assignment

Candidate name: Udara de Silva
Attempted on: 20th July 2025
Completed on: 23rd July 2025

## How to run?
 - execute ```docker compose up --build```
 - swagger UI can be accessed by visiting ```http://172.20.0.10/swagger/index.html``` on the host
 - on the initial run, EF will seed data and add a single team with few agents to the database
 - to send a chat request, click on the execute button under ```/api/MoneyBase`` in swagger UI

## Assumptions
 - Multiple teams can be working at the same time
 - A team will have multiple agents

## Improvements that were not implemented due to time constraints
 - Once a successful chat request is created, the API can send a JWT secure cookie back to the client so they don't have to manually attach the chat-request identifier on subsequent requests
 - Only includes the basic endpoints to send a chat request and to send the polling requests
 - Lacks integration tests
 - Does not include the front-end

## Tech stack
 - DotNet
 - MariaDB
 - RabbitMq
 - Docker
 - Unit Testing: NUnit
