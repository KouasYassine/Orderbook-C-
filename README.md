# 🏦 Trading Engine – Limit Order Book Implementation

## 📌 Overview

This project implements a simplified **Trading Engine Server** in **C# (.NET)** featuring a fully functional **Limit Order Book**.

The engine simulates the core behavior of a financial exchange by:

- Accepting buy and sell limit orders
- Matching orders using **price-time priority**
- Maintaining bid and ask books
- Generating match results

This project demonstrates understanding of trading system mechanics, data structures, clean architecture, and backend engineering principles.

---

## 🧠 Architecture

The solution is structured into multiple projects:

### 🔹 TradingEngine (Host)
- Console application
- Configures dependency injection
- Bootstraps the matching engine using `IHost`
- Handles configuration via `appsettings.json`

### 🔹 OrderBookCS (Core Matching Logic)

Contains:

- `Order`
- `OrderBook`
- `Limit`
- `OrderBookEntry`
- `MatchResult`

Interfaces:

- `IOrderEntryOrderBook`
- `IMatchingOrderBook`
- `IReadOnlyOrderBook`

Implements:
- Price-time priority
- Bid/Ask separation
- Linked-list structure per price level
- Efficient insertion and removal of orders

### 🔹 LoggingCS

Custom logging system featuring:

- Asynchronous logging queue
- Configurable logger types
- File-based logging
- JSON configuration binding
- Hosted background service integration

---

## ⚙️ Matching Rules

The engine follows standard exchange logic:

- Buy orders match with the **lowest available ask**
- Sell orders match with the **highest available bid**
- Orders match if:
  - Buy price ≥ best ask
  - Sell price ≤ best bid
- FIFO priority within the same price level

---

## 📈 Example

| Order ID | Side | Price | Quantity |
|----------|------|-------|----------|
| 1        | Buy  | 100   | 10       |
| 2        | Sell | 100   | 5        |

**Result:**

- Trade executed at 100
- Buy order remains with quantity 5
- Sell order fully filled

---

## 🚀 How to Run

1. Clone the repository
2. Open the solution in Visual Studio
3. Build the solution
4. Run the `TradingEngine` project

---

## 🧪 Testing Example

You can test the engine by adding sample orders in `Program.cs`:

```csharp
orderBook.AddOrder(new Order(1, Side.Buy, 100, 10));
orderBook.AddOrder(new Order(2, Side.Sell, 100, 5));
