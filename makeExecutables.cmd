@echo off
setlocal

@echo win_x64
set goos=windows
set goarch=amd64
go build -o output\%goos%\%goarch%\dif.exe

@echo Lin_386
set goos=linux
set goarch=386
go build -o output\%goos%\%goarch%\dif

@echo Lin_x64
set goos=linux
set goarch=amd64
go build -o output\%goos%\%goarch%\dif

@echo Lin_arm
set goos=linux
set goarch=arm
set goarm=7
go build -o output\%goos%\%goarch%\dif

@echo Lin_arm64
set goos=linux
set goarch=arm64
set goarm=7
go build -o output\%goos%\%goarch%\dif

@echo MacOS Intel
set goos=darwin
set goarch=amd64
go build -o output\MacOS\intel\dif

@echo MacOS M1/2
set goos=darwin
set goarch=arm64
go build -o output\MacOS\M1\dif

endlocal

@echo all done