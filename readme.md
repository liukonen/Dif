# Dif
`Go` `File Comparison` `Concurrency` `CLI` `File System`

A lightweight CLI tool to compare two directories based on file existence and modification timestamps.

## Context & Story
This utility was originally created to address the need for a simple way to preview changes between two directories when using `robocopy`. At the time, the `/l` (list) option in `robocopy` was unknown, so this tool was developed to list differences based on file existence and modification timestamps. Initially written in C#, it was later converted to Go for simplicity and portability.

## Architecture & Decisions
- **Go**: Chosen for its simplicity and performance in building CLI tools.
- **Concurrency**: Utilized `sync.WaitGroup` and goroutines for parallel file processing.
- **File Comparison**: Based on `LastModified` timestamps for efficiency.
- **Portability**: Designed as a cross-platform CLI tool.

## Key Features
- Compares two directories for:
  - Files present in one directory but not the other.
  - Files with differing modification timestamps.
- Outputs differences in a clear, human-readable format.
- Lightweight and fast, leveraging Go's concurrency model.

## Quick Start
1. **Build the executable**:
   - On Windows: Run `makeExecutables.cmd`.
   - On other platforms: Use `go build` to compile the source.
2. **Run the tool**:
   ```bash
   dif [source_directory] [destination_directory]
   ```
   Example:
   ```bash
   dif ./dir1 ./dir2
   ```
   The tool will output the differences between the two directories.
