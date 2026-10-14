# File Integrity Checker

A C# command-line tool that detects modified, new, and deleted
files by comparing SHA-256 hashes against a saved JSON baseline.

## Technologies
C#, .NET 10, Linux, Bash, Docker, JSON

## Run locally
dotnet run -- samples baseline.json

## Run on Linux with Docker
docker build -t file-integrity-checker .
docker run --rm file-integrity-checker

## Choose paths inside the container
docker run --rm file-integrity-checker ./check.sh /app/samples /app/baseline.json

## Behavior
- Creates a baseline if the specified baseline file is missing.
- Reports UNCHANGED, MODIFIED, NEW, and DELETED files.
- Returns exit code 1 for handled file-access or baseline errors.
- Checks files directly inside the chosen folder.

## Limitations
The baseline must be trusted and protected from modification.
Detected changes do not automatically indicate malicious activity.
Docker uses copies of sample files taken during the image build.