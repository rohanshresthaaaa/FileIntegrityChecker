#!/bin/bash
set -e

cd /app
echo "Starting file integrity check..."
dotnet bin/Release/net10.0/FileIntegrityChecker.dll "$@"
echo "Check finished."