/* groovylint-disable DuplicateStringLiteral, LineLength, NestedBlockDepth, NoDef */
/* groovylint-disable-next-line CompileStatic */
pipeline {
    agent any

    environment {
        SCANNER_HOME = tool name: 'sonar_scanner_dotnet'
        // Github Enironmant Varibales
        GITHUB_CREDENTIALS = 'GitHub'
        GITHUB_URL = 'https://github.com/guchhaitprasun/app_prasunguchhait.git'
        GITHUB_BRANCH = 'master'
    }

    stages {
        // Git checkout
        stage('Checkout') {
            steps {
                echo 'Pulling latest code from GitHub'
                git credentialsId: env.GITHUB_CREDENTIALS, url: env.GITHUB_URL, branch: env.GITHUB_BRANCH
                echo 'Git Pull Complete'
            }
        }

        //Nuget Restore
        stage('Restore Packages') {
            steps {
                echo 'Restoring Nuget Packages'
                bat 'dotnet restore'
                echo 'Nuget Pacakges Restored'
            }
        }

        //Sonar qube analysis start
        stage('Start sonarqube analysis') {
            steps {
                echo 'Sonar Analysis Begin'
                withSonarQubeEnv('Test_Sonar') {
                    bat "${SCANNER_HOME}/SonarScanner.MSBuild.exe begin /k:DevOps_WebAPI /n:DevOps_WebAPI /v:1.0"
                }
            }
        }

        //Clean and Build solution
        stage('Clean & Build') {
            steps {
                echo 'Cleaning Previous Build'
                bat 'dotnet clean'
                echo 'Cleaning Finished'

                echo 'Building Projects'
                bat 'dotnet build --configuration Release'
                echo 'Build Finished'
            }
        }

        stage('Unit Testing') {
            steps {
                echo 'Begin Unit Testing'
                bat 'dotnet test DevOps_WebAPI.Test\\DevOps_WebAPI.Test.csproj -l:trx;LogFileName=DevOps_WebAPI_Test.xml'
                echo 'Unit Testing Finished'
            }
        }

        //Stop sonar qube analysis
        stage('Stop sonarqube analysis') {
            steps {
                echo 'Sonar Analysis Finished'
                withSonarQubeEnv('Test_Sonar') {
                    bat "${SCANNER_HOME}/SonarScanner.MSBuild.exe end"
                }
            }
        }
    }
}
