/* groovylint-disable DuplicateStringLiteral, LineLength, NestedBlockDepth, NoDef */
/* groovylint-disable-next-line CompileStatic */
pipeline {
    agent any

    environment {
        SCANNER_HOME = tool name: 'sonar_scanner_dotnet'
        USERNAME = 'prasunguchhait'

        // Github Enironmant Varibales
        GITHUB_CREDENTIALS = 'GitHub'
        GITHUB_URL = 'https://github.com/guchhaitprasun/app_prasunguchhait.git'

        // Docker Enviornment Variables
        DOCKER_CREDENTIALS = 'DockerHub'
        DOCKER_REGISTRY = 'prasunguchhait/app-prasunguchhait'
        DOCKER_CONTAINER_NAME = 'c-prasunguchhait'
        DOCKER_PORT_MASTER = '7200:80'
        DOCKER_PORT_DEVELOP = '7300:80'

        //Google Kubernetes Credentials
        GKE_PROJECT_ID = 'devopsoneapi'
        GKE_CLUSTER_NAME = 'one-api-cluster'
        GKE_CLUSTER_LOCATION = 'us-central1'
        GKE_CREDENTIALS_ID = 'GKECredentials_OneAPI'
        GKE_MANIFEST_PATTERN_MASTER = 'master-deployment.yaml'
        GKE_MANIFEST_PATTERN_DEVELOP = 'develop-deployment.yaml'
    }

    stages {
        // Git checkout
        stage('Checkout') {
            steps {
                echo "Pulling latest code from GitHub Branch: ${BRANCH_NAME}"
                git credentialsId: env.GITHUB_CREDENTIALS, url: env.GITHUB_URL, branch: env.BRANCH_NAME
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
            when {
                branch 'master'
            }

            steps {
                echo 'Sonar Analysis Begin'
                withSonarQubeEnv('Test_Sonar') {
                    bat "${SCANNER_HOME}/SonarScanner.MSBuild.exe begin /k:DevOps_WebAPI /d:sonar.cs.opencover.reportsPaths=DevOps_WebAPI.Test/coverage.opencover.xml /d:sonar.coverage.exclusions='**Test*.cs'"
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

        // Release Artifiacts
        stage ('Release Artifiact') {
            when {
                branch 'develop'
            }

            steps {
                echo 'Publishing Project with Release configuration'
                bat 'dotnet publish --configuration Release'
                echo 'Publish Finished'
            }
        }

        // Start Unit Testing
        stage('Unit Testing') {
            when {
                branch 'master'
            }

            steps {
                echo 'Begin Unit Testing'
                bat 'dotnet test DevOps_WebAPI.Test\\DevOps_WebAPI.Test.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=opencover'
                echo 'Unit Testing Finished'
            }
        }

        //Stop sonar qube analysis
        stage('Stop sonarqube analysis') {
            when {
                branch 'master'
            }

            steps {
                echo 'Sonar Analysis Finished'
                withSonarQubeEnv('Test_Sonar') {
                    bat "${SCANNER_HOME}/SonarScanner.MSBuild.exe end"
                }
            }
        }

        //Docker Image
        stage ('Docker Image') {
            steps {
                echo 'Building Docker Image'
                bat "docker build -t i-${USERNAME}-${BRANCH_NAME} --no-cache -f Dockerfile ."
                echo 'docker Image build complete'
            }
        }

        stage('container') {
            parallel {
                stage('Pre-Container Check') {
                    steps {
                        echo 'Checking if Container is previously deployed'
                        script {
                            String dockerCommand = "docker ps -a -q -f name=${DOCKER_CONTAINER_NAME}-${BRANCH_NAME}"
                            String commandExecution = "${bat(returnStdout: true, script: dockerCommand)}"
                            dockerPreviousDeploymentContainerId = "${commandExecution.trim().readLines().drop(1).join(' ')}"

                            if (dockerPreviousDeploymentContainerId != null && dockerPreviousDeploymentContainerId != '') {
                                echo "Previous Deploymnet Found. Container Id ${dockerPreviousDeploymentContainerId}"

                                echo "Stopping Container ${dockerPreviousDeploymentContainerId}"
                                bat "docker stop ${dockerPreviousDeploymentContainerId}"

                                echo "Removing Container ${dockerPreviousDeploymentContainerId}"
                                bat "docker rm ${dockerPreviousDeploymentContainerId}"
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
                        bat "docker tag i-${USERNAME}-${BRANCH_NAME} ${DOCKER_REGISTRY}:${BRANCH_NAME}-${BUILD_NUMBER}"
                        bat "docker tag i-${USERNAME}-${BRANCH_NAME} ${DOCKER_REGISTRY}:${BRANCH_NAME}-latest"

                        echo 'Pushing Image to Docker Hub'
                        withDockerRegistry([credentialsId: env.DOCKER_CREDENTIALS, url: '']) {
                            bat "docker push ${DOCKER_REGISTRY}:${BRANCH_NAME}-${BUILD_NUMBER}"
                            bat "docker push ${DOCKER_REGISTRY}:${BRANCH_NAME}-latest"
                        }
                    }
                }
            }
        }

        stage('Deploy Docker Image') {
            steps {
                switch (env.BRANCH_NAME) {
                    case 'master':
                        echo "Deploying master branch docker Image on ${dockerPort}"
                        bat "docker run --name ${DOCKER_CONTAINER_NAME}-${BRANCH_NAME} -d -p ${DOCKER_PORT_MASTER} ${DOCKER_REGISTRY}:${BRANCH_NAME}-${BUILD_NUMBER}"
                    break
                    case 'develop':
                        echo "Deploying develop branch docker Image on ${dockerPort}"
                        bat "docker run --name ${DOCKER_CONTAINER_NAME}-${BRANCH_NAME} -d -p ${DOCKER_PORT_DEVELOP} ${DOCKER_REGISTRY}:${BRANCH_NAME}-${BUILD_NUMBER}"
                    break
                    default :
                        echo 'No Branch Match Found'
                    break
                }
            }
        }

        stage('Kubernetes Deployment') {
            steps {
                script {
                    withCredentials ([file(credentialsId: env.GKE_CREDENTIALS_ID, variable: 'GC_KEY')]) {
                        echo 'Authenticating From Google Cloud.....'
                        bat "gcloud auth activate-service-account --key-file=${GC_KEY}"
                        echo 'Authentication complete'

                        echo 'Connecting To Cluster'
                        bat "gcloud container clusters get-credentials one-api-cluster --region us-central1 --project ${GKE_PROJECT_ID}"
                        echo 'Cluster Connection complete'
                    }
                }

                switch (env.BRANCH_NAME) {
                    case 'master':
                        echo "Deploying container to  Google Kubernetes Engine for ${BRANCH_NAME} branch using ${GKE_MANIFEST_PATTERN_MASTER} manifest pattern"
                        bat "kubectl apply -f ${GKE_MANIFEST_PATTERN_MASTER}"
                    break
                    case 'develop':
                        echo "Deploying container to  Google Kubernetes Engine for ${BRANCH_NAME} branch using ${GKE_MANIFEST_PATTERN_DEVELOP} manifest pattern"
                        bat "kubectl apply -f ${GKE_MANIFEST_PATTERN_DEVELOP}"
                    break
                    default :
                        echo 'No Branch Match Found'
                    break
                }
            }
        }
    }
}
