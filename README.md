# RigCountProcessor

![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/z1z0v1c/RigCountDownloader/dotnet.yml?branch=master)
![GitHub last commit](https://img.shields.io/github/last-commit/z1z0v1c/RigCountDownloader)
![GitHub issues](https://img.shields.io/github/issues/z1z0v1c/RigCountDownloader)
![GitHub](https://img.shields.io/github/license/z1z0v1c/RigCountDownloader)

A C# console application that downloads an Excel file from a specified website and converts it into a CSV file on your local drive.

---

## 📦 Features

- Automated download of Excel files from a predefined URL.
- Conversion of Excel data to CSV format.
- Command-line interface for ease of use.
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

The application will download the Excel file from the specified URL and convert it into a CSV file saved on your local drive.

---

## 🧪 Running Tests

To run the unit tests:

```bash
dotnet test
```

This will execute all tests in the `RigCountProcessor.Tests` project.

---

## 📁 Project Structure

```text
RigCountDownloader/
├── RigCountProcessor/           # Main application source code
├── RigCountProcessor.Tests/     # Unit tests
├── .github/workflows/           # GitHub Actions workflows
├── RigCountProcessor.sln        # Solution file
├── Task.pdf                     # Project task description
└── README.md                    # Project documentation
```

---

## 📄 License

This project is licensed under the MIT License.  
See the [LICENSE](https://github.com/z1z0v1c/RigCountDownloader/blob/master/LICENSE) file for details.

---

## 🙌 Acknowledgments

- [CommandLineParser](https://github.com/commandlineparser/commandline) for parsing command line arguments
