![image](https://github.com/user-attachments/assets/1014ecd1-1781-4e90-ab9d-cd2ec21a8a4d)
# 📈 Stock Market Analysis Tool (C# Windows Forms)

A feature-rich Windows Forms application developed in **C#** for stock market visualization, pattern recognition, and predictive analytics using **candlestick charts** and **Fibonacci wave analysis**.

> This project was developed in **three progressive stages**:
> - 📦 Project 1: Data loading, candlestick/volume chart, DataGridView.
> - 🔍 Project 2: Multi-stock analysis, candlestick pattern detection (Peak/Valley, Doji, Hammer, etc.).
> - 🌊 Project 3: Fibonacci wave analysis, Beauty function, prediction modeling.

---

## 🧠 Features

### ✅ Core Features (Project 1)
- Load **CSV-based stock data** (Daily, Weekly, Monthly) from Yahoo Finance.
- Visualize **OHLC (Open, High, Low, Close)** prices in **candlestick format**.
- Display **volume data** alongside price in column chart.
- Bind data using **Windows Forms data binding**.
- Use a **DataGridView** for raw data inspection (Project 1 only).

### 📊 Advanced Charting (Project 2)
- Normalize chart axes to remove gaps and fully utilize display area.
- Support **multiple stock comparisons**, each in its own window.
- Annotate **Peak (Green)** and **Valley (Red)** candlesticks.
- Highlight special candlestick patterns using a custom `SmartCandlestick` class:
  - Doji, Marubozu, Hammer, Dragonfly, Gravestone, Bullish, Bearish, Neutral
- Automatically remove weekend/holiday gaps for daily data.

### 🔮 Predictive Analytics (Project 3)
- Enable interactive wave selection on charts (via mouse or scrollbars).
- Compute **Fibonacci levels** (0%, 23.6%, ..., 100%) between selected points.
- Evaluate **"Beauty" score** of a wave based on OHLC confirmations with Fibonacci lines.
- Display **Beauty(price)** as a function plot—used to estimate future highs/lows.
- Real-time updates to charts without reloading stock data.

---

## 🧱 Technologies Used

| Tech | Description |
|------|-------------|
| C# | Language |
| .NET Framework | Windows Forms |
| WinForms Charting | Candlestick & volume plots |
| Data Binding | For stock data integration |
| Yahoo Finance | Source of CSV data |
| Custom Classes | `Candlestick`, `SmartCandlestick`, Fibonacci, and Beauty logic |

---

## 🗂 Folder Structure

```
/StockMarketAnalyzer
│
├── /Stock Data             # Input CSVs: xxx-Day.csv, xxx-Week.csv, xxx-Month.csv
├── /Forms                  # Main form, chart windows
├── /Classes                # Candlestick, SmartCandlestick, Fibonacci logic
├── Program.cs              # Entry point
└── README.md
```

---

## 🚀 Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/deshninad/Stock-Analysis-App.git
   ```

2. Download stock CSVs from [Yahoo Finance](https://finance.yahoo.com/) and place them in the `Stock Data` folder.

3. Open the solution in **Visual Studio** and run the app.

---

## 👨‍💻 Author

**Ninad Deshpande**  
💼 CS Student @ University of South Florida  
🔗 [LinkedIn](https://www.linkedin.com/in/deshninad) • 🧠 [GitHub](https://github.com/deshninad)
