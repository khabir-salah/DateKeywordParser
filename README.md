# DateKeywordParser

**DateKeywordParser** is a lightweight and flexible NuGet package designed to parse and resolve date-related keywords in file names. It simplifies working with dynamically generated file names for tasks such as FTP file downloads or log file processing.

## Features

- **Keyword Parsing**: Supports `NOW`, `YESTERDAY`, `TODAY`, and more.
- **Relative Offsets**: Handles keywords like `NOW-1d`, `NOW+2h`.
- **Custom Formatting**: Resolves keywords using patterns like `Format(NOW, "yyyy-MM-dd")`.
- **Full Formatting**: Resolves Keyword using pattern like data-{YESTERDAY}.txt, log-{NOW-1d}.log
- **Localization Support**: Formats dates based on specific cultures or locales.
- **Lightweight**: efficient and easy to integrate.

## Installation

Install the package via NuGet Package Manager:
```bash
dotnet add package DateKeywordParser
