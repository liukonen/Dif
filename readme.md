# Dif - File Comparison Utility

Dif is a command-line application designed to compare files between two directories and identify the differences based on file existence and LastModified timestamps. This tool was initially created to analyze the changes that could occur when running a robocopy command between two directories. It serves as a helpful utility for understanding the variances between two sets of files.

## Features

- Compares files in two directories and identifies the differences.
- Utilizes the LastModified timestamp of the files for comparison.
- Supports parallel tasks for efficient processing.
- Enables file enumeration to obtain a comprehensive list of files.

## Usage
```bash
dif [source] [destination]
```

- `source`: The source directory to compare.
- `destination`: The destination directory to compare.

## Compile

To compile Dif, follow these steps:

1. Install Go programming language (if not already installed).
2. Clone the Dif repository or download the source code.
3. Build the executable by running the following command:

```bash
go build dif.go
```

## How It Works

1. Dif takes two directory paths as command-line arguments: `source` and `destination`.
2. It retrieves the list of files in both directories using the `getFiles` function.
3. The LastModified timestamps of the files in each directory are stored in separate dictionaries: `sourceDict` and `destDict`.
4. Parallel tasks are used to populate the dictionaries efficiently.
5. The function `getDiff` identifies files present in one dictionary but not in the other, yielding `sourceOnlyFiles` and `destOnlyFiles`.
6. The function `getIntersection` finds files that exist in both directories, resulting in `commonFiles`.
7. Dif compares the LastModified timestamps of the common files and displays the differences.
8. Finally, Dif presents the files unique to each directory.

## License

Dif is open source and released under the [MIT License](https://opensource.org/licenses/MIT).

## Acknowledgments

This application was created by Luke Liukonen in 2017-2023 as a small single-purpose utility. It incorporates learnings related to parallel tasks and file enumeration.

For more information, contact Luke Liukonen 
