/* groovylint-disable DuplicateStringLiteral, LineLength, NestedBlockDepth, NoDef */
/* groovylint-disable-next-line CompileStatic */
pipeline {
    agent any

    environment {
        SCANNER_HOME = tool name: 'sonar_scanner_dotnet'
        USERNAME = 'prasunguchhait'
        DOCKER_PREVIOUSDEPLOYMNET_CONTAINER_ID = null

        // Github Enironmant Varibales
        GITHUB_CREDENTIALS = 'GitHub'
        GITHUB_URL = 'https://github.com/guchhaitprasun/app_prasunguchhait.git'
        GITHUB_BRANCH = 'develop'

        // Docker Enviornment Variables
        DOCKER_CREDENTIALS = 'DockerHub'
        DOCKER_REGISTRY = 'prasunguchhait/app-prasunguchhait-develop'
        CONTAINER_NAME = 'c-prasunguchhait-develop'
        DOCKER_PORT = '7300:80'
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

         stage ('Release Artifiact') {
            steps {
                echo 'Publishing Project with Release configuration'
                bat 'dotnet publish --configuration Release'
                echo 'Publish Finished'
            }
        }

         stage ('Docker Image') {
            steps {
                echo 'Building Docker Image'
                bat "docker build -t i-${USERNAME}-${GITHUB_BRANCH} --no-cache -f Dockerfile ."
                echo 'docker Image build complete'
            }
        }

        stage('Container') {
            parallel {
                stage('Pre-Container Check') {
                    steps {
                        echo 'Checking if Container is previously deployed'
                        script {
                            String dockerCommand = "docker ps -a -q -f name=${CONTAINER_NAME}"
                            String commandExecution = "${bat(returnStdout: true, script: dockerCommand)}"
                            DOCKER_PREVIOUSDEPLOYMNET_CONTAINER_ID = "${commandExecution.trim().readLines().drop(1).join(' ')}"

                            if (DOCKER_PREVIOUSDEPLOYMNET_CONTAINER_ID != '') {
                                echo "Previous Deploymnet Found. Container Id ${DOCKER_PREVIOUSDEPLOYMNET_CONTAINER_ID}"

                                echo "Stopping Container ${DOCKER_PREVIOUSDEPLOYMNET_CONTAINER_ID}"
                                bat "docker stop ${DOCKER_PREVIOUSDEPLOYMNET_CONTAINER_ID}"

                                echo "Removing Container ${DOCKER_PREVIOUSDEPLOYMNET_CONTAINER_ID}"
                                bat "docker rm ${DOCKER_PREVIOUSDEPLOYMNET_CONTAINER_ID}"
                            } else {
                                echo 'Container Not Deployed Previously'
                            }
                        }
                        echo 'Pre-Container Check Complete'
                    }
                }

                stage('Publish to Docker Hub') {
                    steps {
                        echo 'Tagging Docker Image'
                        bat "docker tag i-${USERNAME}-${GITHUB_BRANCH} ${DOCKER_REGISTRY}:${BUILD_NUMBER}"
                        bat "docker tag i-${USERNAME}-${GITHUB_BRANCH} ${DOCKER_REGISTRY}:latest"

                        echo 'Pushing Image to Docker Hub'
                        withDockerRegistry([credentialsId: env.DOCKER_CREDENTIALS, url: '']) {
                            bat "docker push ${DOCKER_REGISTRY}:${BUILD_NUMBER}"
                            bat "docker push ${DOCKER_REGISTRY}:latest"
                        }
                    }
                }
            }
        }

        stage('Deploy Docker Image') {
            steps {
                echo 'Deploying docker Image'
                bat "docker run --name ${CONTAINER_NAME} -d -p ${DOCKER_PORT} ${DOCKER_REGISTRY}:${BUILD_NUMBER}"
            }
        }
    }
}
