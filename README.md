# RigCountProcessor

![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/z1z0v1c/RigCountProcessor/dotnet.yml?branch=master)
![GitHub last commit](https://img.shields.io/github/last-commit/z1z0v1c/RigCountProcessor)
![GitHub issues](https://img.shields.io/github/issues/z1z0v1c/RigCountProcessor)
![GitHub](https://img.shields.io/github/license/z1z0v1c/RigCountProcessor)

A .NET Core console application that retrieves a file from the web, processes the data using a given algorithm, and
writes it to a specified output format.

Currently supports processing Xlsx data using a simple Rig Count algorithm and writing to a Csv file. More options will
be available soon.

## Features

- Automated file download from a predefined URL.
- Conversion and processing of Xlsx data to Csv format.
- Command-line interface for ease of use.
- Customizable behavior through command-line arguments and a config file.

## Installation

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
4. Build the application:

   ```bash
   dotnet build
   ```

## Usage

1. Run the application:

   ```bash
   dotnet run --project RigCountProcessor
   ```

The application will download the Excel file from the specified URL and convert it into a CSV file saved on the local
drive.

2. Command Line Options:

```
--start-year, -s    Starting year of data processing. 
--year-count, -y    Data processing year count.
```

3. Running Tests:

```bash
dotnet test
```

This will execute all tests in the `RigCountProcessor.Tests` project.

## License

This project is licensed under the MIT License. See the [LICENSE](https://github.com/z1z0v1c/RigCountDownloader/blob/master/LICENSE) file for details.
