#!/bin/sh

# https://unix.stackexchange.com/questions/129391/passing-named-arguments-to-shell-scripts
for ARGUMENT in "$@"
do

    KEY=$(echo $ARGUMENT | cut -f1 -d=)
    VALUE=$(echo $ARGUMENT | cut -f2 -d=)   

    case "$KEY" in
            BUILD_ID)              BUILD_ID=${VALUE} ;;
            BUILD_NUMBER)          BUILD_NUMBER=${VALUE} ;;  
            *)   
    esac    


done

if [ -z "$BUILD_ID" ]
then
      echo "BUILD_ID is empty setting default value..."
      BUILD_ID=1
fi

if [ -z "$BUILD_NUMBER" ]
then
      echo "BUILD_NUMBER is empty setting default value..."
      BUILD_NUMBER=$(date +"%Y%m%d.%H%M%S")
fi

echo "BUILD_ID = '$BUILD_ID'"
echo "BUILD_NUMBER = '$BUILD_NUMBER'"

_cwd="$PWD"
CONFIGURATION=Release

dotnet restore

dotnet build --configuration $CONFIGURATION --no-restore /p:BuildId=$BUILD_ID /p:BuildNumber=$BUILD_NUMBER

# dotnet test --configuration $CONFIGURATION --no-build --verbosity normal
#dotnet test --configuration $CONFIGURATION --no-build --verbosity normal /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=TestResults/lcov.info

# dotnet test /p:CollectCoverage=true /p:CoverletOutput=TestResults/ /p:CoverletOutputFormat=lcov
# dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./lcov.info
# dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=TestResults/lcov.info

# Generate coverage report
# Publish coverage report
dotnet "artifacts/bin/Kaylumah.Ssg.Client.SiteGenerator/$CONFIGURATION/netcoreapp3.1/Kaylumah.Ssg.Client.SiteGenerator.dll" SiteConfiguration:AssetDirectory=assets

cd dist
npm i
npm run build:prod
rm styles.css
rm -rf node_modules
rm package.json package-lock.json
rm tailwind.config.js postcss.config.js