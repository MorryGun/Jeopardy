pipeline {
    agent any
    environment {
        myVersion = '0.9'
    }
    stages {
        stage('checkout') {
            steps {
                git branch: 'main', url: 'https://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@github.com/MorryGun/Jeopardy/'
            }
        }
        stage('restore') {
            steps {
                bat 'dotnet restore Jeopardy.Backend.sln'
            }
        }
        stage('build') {
            steps {
                bat 'dotnet build Jeopardy.Backend.sln'
            }
        }
        stage('unit test') {
            steps {
                bat 'dotnet test "Jeopardy_Backend_UnitTests\\Jeopardy_Backend_UnitTests.csproj"'
            }
        }
        stage('integration test') {
            steps {
                bat 'dotnet test "Jeopardy_Backend_IntegrationTests\\Jeopardy_Backend_IntegrationTests.csproj"'
            }
        }
        stage('publish') {
            steps {
                bat 'dotnet publish "Jeopardy_Backend\\Jeopardy_Backend.csproj"'
            }
        }
        stage('archive') {
            steps {
                archiveArtifacts artifacts: 'Jeopardy_Backend\\bin\\Debug\\netcoreapp5.0\\Jeopardy_Backend.dll', followSymlinks: false
            }
        }
    }
}