# RigCountProcessor

![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/z1z0v1c/RigCountProcessor/dotnet.yml?branch=master)
![GitHub last commit](https://img.shields.io/github/last-commit/z1z0v1c/RigCountProcessor)
![GitHub issues](https://img.shields.io/github/issues/z1z0v1c/RigCountProcessor)
![GitHub](https://img.shields.io/github/license/z1z0v1c/RigCountProcessor)

A .NET Core console application that retrieves a file from the web, processes the data using a given algorithm, and
writes it to a specified output format.

Currently supports processing Xlsx data using a simple Rig Count algorithm and writing to a Csv file. More options will
be available soon.

---

## 📦 Features

- Automated file download from a predefined URL.
- Conversion and processing of Xlsx data to Csv format.
- Command-line interface for ease of use.
- Customizable behavior through command-line arguments and a config file
- Modular code structure for maintainability.
- Unit tests to ensure reliability.

---

## 🚀 Getting Started

### Prerequisites

- .NET 6.0 SDK or later
- Internet connection to download the Excel file

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/z1z0v1c/RigCountDownloader.git
   ```
2. Navigate to the project directory:

   ```bash
   cd RigCountDownloader
   ```

3. Restore dependencies:

   ```bash
   dotnet restore
   ```

---

## 🛠️ Usage

1. Build the application:

   ```bash
   dotnet build
   ```

2. Run the application:

   ```bash
   dotnet run --project RigCountProcessor
   ```

The application will download the Excel file from the specified URL and convert it into a CSV file saved on the local
drive.

---

### Command Line Options

```
--start-year, -s    Starting year of data processing. 
--year-count, -y    Data processing year count.
```

---

## 🧪 Running Tests

To run the unit tests:

```bash
dotnet test
```

This will execute all tests in the `RigCountProcessor.Tests` project.

---

## 📁 Project Structure (simplified)

```text
RigCountProcessor/
├── .github/                     # GitHub configuration
│   └── workflows/               # CI/CD pipeline
│              
├── RigCountProcessor/           # Main project 
│   ├── Application/             # Data processing pipeline
│   ├── Domain/                  # Domain models and interfaces 
│   │   ├── Interfaces/          
│   │   └── Models/              
│   ├── Services/                # Core services for data processing
│   │
│   └── Program.cs               # Application entry point
│   
├── RigCountProcessor.Tests/     # Unit testing project
│   ├── Mocks/                   # Mock classes
│   ├── Services/                # Tests for each service class 
│   └── TestData/                # Data used by unit tests
│
├── appsettings.json             # Configuration file
├── LICENSE                      # License file
└── README.md                    # Project documentation
```

---

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](https://github.com/z1z0v1c/RigCountDownloader/blob/master/LICENSE) file for details.

---

## 🙌 Acknowledgments

- [xUnit](https://github.com/xunit/xunit) for unit testing
- [NSubstitute](https://github.com/nsubstitute/NSubstitute) for mock substitutes
- [MockHttp](https://github.com/richardszalay/mockhttp) for HttpClient mocking
- [Serilog](https://github.com/serilog/serilog) for structured logging
- [EPPlus](https://github.com/EPPlusSoftware/EPPlus) for Xlsx data processing
- [CommandLineParser](https://github.com/commandlineparser/commandline) for parsing command line arguments
