package main

import (
    "fmt"
    "io/fs"
    "os"
    "path/filepath"
    "sync"
    "time"
)

func main() {
    if len(os.Args) != 3 {
        fmt.Println("Command Line usage. dif [source] [destination] - App uses ModTime to compare files")
        return
    }

    fmt.Println("dif - command line file difference viewer © 2017-2023 Luke Liukonen")
    fmt.Println("-------------------------------------------------------------------")

    sourceDir := os.Args[1]
    destDir := os.Args[2]

    sourceFiles := getFiles(sourceDir)
    destFiles := getFiles(destDir)

    sourceDict := make(map[string]time.Time)
    destDict := make(map[string]time.Time)

    var wg sync.WaitGroup
    wg.Add(2)

    go func() {
        defer wg.Done()
        for _, file := range sourceFiles {
            sourceDict[filepath.ToSlash(file[len(sourceDir)+1:])] = getFileModTime(file)
        }
    }()

    go func() {
        defer wg.Done()
        for _, file := range destFiles {
            destDict[filepath.ToSlash(file[len(destDir)+1:])] = getFileModTime(file)
        }
    }()

    wg.Wait()

    sourceOnlyFiles := getDiff(sourceDict, destDict)
    destOnlyFiles := getDiff(destDict, sourceDict)
    commonFiles := getIntersection(sourceDict, destDict)

    for _, item := range commonFiles {
        if destDict[item] != sourceDict[item] { // files exist, but don't match
            fmt.Printf("%s %s is %s than %s\n", item, sourceDir, getComparison(sourceDict[item], destDict[item]), destDir)
        }
    }

    for _, item := range sourceOnlyFiles {
        fmt.Printf("%s exists in %s, not in %s\n", item, sourceDir, destDir)
    }

    for _, item := range destOnlyFiles {
        fmt.Printf("%s exists in %s, not in %s\n", item, destDir, sourceDir)
    }
}

func getFiles(path string) []string {
    var files []string
    filepath.Walk(path, func(path string, info fs.FileInfo, err error) error {
        if !info.IsDir() {
            files = append(files, path)
        }
        return nil
    })
    return files
}

func getFileModTime(path string) time.Time {
    fileInfo, err := os.Stat(path)
    if err != nil {
        panic(err)
    }
    return fileInfo.ModTime()
}

func getDiff(dict1 map[string]time.Time, dict2 map[string]time.Time) []string {
    var diff []string
    for key := range dict1 {
        if _, ok := dict2[key]; !ok {
            diff = append(diff, key)
        }
    }
    return diff
}

func getIntersection(dict1 map[string]time.Time, dict2 map[string]time.Time) []string {
    var intersection []string
    for key := range dict1 {
        if _, ok := dict2[key]; ok {
            intersection = append(intersection, key)
        }
    }
    return intersection
}

func getComparison(time1 time.Time, time2 time.Time) string {
    if time1.After(time2) {
        return "newer"
    } else {
        return "older"
    }
}