pipeline {
    agent none
    stages {
        stage('checkout') {
            agent { label 'master' }
            steps {
                git branch: 'main', url: 'https://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@github.com/MorryGun/Jeopardy/'
            }
        }
        stage('restore') {
            agent { label 'master' }
            steps {
                bat 'dotnet restore Jeopardy.Backend.sln'
            }
        }
        stage('build') {
            agent { label 'master' }
            steps {
                bat 'dotnet build Jeopardy.Backend.sln'
            }
        }
        stage('unit test') {
            agent { label 'master' }
            steps {
                bat 'dotnet test "Jeopardy_Backend_UnitTests\\Jeopardy_Backend_UnitTests.csproj"'
            }
        }
        stage('integration test') {
            agent { label 'master' }
            steps {
                bat 'dotnet test "Jeopardy_Backend_IntegrationTests\\Jeopardy_Backend_IntegrationTests.csproj"'
            }
        }
        stage('publish') {
            agent { label 'master' }
            steps {
                bat 'dotnet publish "Jeopardy_Backend\\Jeopardy_Backend.csproj"'
            }
        }
        stage('deploy'){
            agent { label 'master' }
            steps{
                dir ('\\Jeopardy_Backend\\bin\\Debug\\netcoreapp5.0\\publish')
                {
                    powershell 'Start-Job -ScriptBlock {dotnet JeopardyBackend.dll}'
                } 
            }
        }
        stage('frontend'){
            agent { label 'master' }
            steps{
               dir('Jeopardy_Frontend') {
                    bat 'pm2 kill'
                    git branch: 'main', url: 'https://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@github.com/MorryGun/JeopardyFrontend/'
                    bat 'npm install'
                    bat 'pm2 start startscript.js --name Jeopardy -- start'
                }
            }
        }
        stage('uitests'){
            agent { label 'master' }
            steps{
               dir('UITests') {
                    git branch: 'add_simple_ui_tests', url: 'https://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@github.com/MorryGun/UITests/'
                    waitUntil(initialRecurrencePeriod: 120000) {
                        script{
                            try {         
                                 bat 'curl http://localhost:4200'
                                return true
                            } catch (Exception e) {
                            return false
                            }
                        }
                    }
                    bat 'dotnet restore JeopardyUITests.sln'
                    bat 'dotnet build JeopardyUITests.sln'
                    bat 'dotnet test "JeopardyUITests\\JeopardyUITests.csproj"'
                }
            }
        }
        stage('stash') {
            agent { label 'master' }
            steps {
                stash includes: 'Jeopardy_Backend\\bin\\Debug\\netcoreapp5.0\\publish\\*', name: 'binary'
            }
        }
        stage('switch_to_dev') {
            agent { label 'dev' }
            steps {
                unstash 'binary'
            }
        }
        stage('deploy_dev'){
            agent { label 'dev' }
            steps{
                dir ('\\Jeopardy_Backend\\bin\\Debug\\netcoreapp5.0\\publish')
                {
                    powershell 'Start-Job -ScriptBlock {dotnet JeopardyBackend.dll}'
                } 
            }
        }
        stage('frontend_dev'){
            agent { label 'dev' }
            steps{
               dir('Jeopardy_Frontend') {
                    bat 'pm2 kill'
                    git branch: 'main', url: 'https://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@github.com/MorryGun/JeopardyFrontend/'
                    bat 'npm install'
                    bat 'pm2 start startscript.js --name Jeopardy -- start'
                }
            }
        }
    }
}
