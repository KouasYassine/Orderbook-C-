🏦 Trading Engine – Order Book Implementation
📌 Overview

This project is a simplified Trading Engine Server written in C# (.NET) featuring a fully functional Limit Order Book implementation.

It simulates the core behavior of an exchange matching engine:

Accepting buy and sell limit orders

Matching orders based on price-time priority

Maintaining bid/ask book structure

Producing match results

The goal of this project is to demonstrate understanding of:

Data structures

Matching engine logic

Clean architecture design

Multithreading (logging / background services)

Dependency Injection using Microsoft.Extensions.Hosting

🧠 Architecture

The solution is structured into multiple projects:

1️⃣ TradingEngine (Host Project)

Console application

Configures services

Bootstraps the engine using IHost

Handles dependency injection

2️⃣ OrderBookCS (Core Matching Logic)

Contains:

Order

OrderBook

Limit

OrderBookEntry

MatchResult

IOrderEntryOrderBook

IMatchingOrderBook

IReadOnlyOrderBook

Implements:

Price-time priority

Bid/Ask separation

Order matching

Linked-list structure inside price levels

3️⃣ LoggingCS

Custom logging system with:

Configurable logger types

Async log queue

File-based logging

JSON configuration support

⚙️ Features

✅ Limit order submission

✅ Order matching engine

✅ Price-time priority

✅ Separate bid and ask books

✅ Order cancellation support (if implemented)

✅ Match result generation

✅ Configurable logging system

✅ Hosted background service architecture

 Order Book Design

The matching engine follows standard exchange rules:

Matching Rules

Buy orders match against lowest available ask

Sell orders match against highest available bid

Orders match if:

Buy price ≥ best ask

Sell price ≤ best bid

FIFO within the same price level

Data Structures

Dictionary / SortedDictionary for price levels

Linked list per price level (time priority)

Efficient insertion and removal
