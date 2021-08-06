
<img src="DocumentationResources/jenkins-logo.png" alt="Jenkis logo" title="eCommerceApp!" align="right" height="60"/>

# Devops & Microservices Advance
NAGP Home Assignment to Create CI/CD Pipeline using Jenkins 

## Getting Started
This application is build on [DotNet 3.1](https://dotnet.microsoft.com/download/dotnet/3.1) using [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) with support of version controlling using [Git](https://git-scm.com/) on [GitHub](https://github.com/guchhaitprasun)

Make sure following tools installed in your machine. 
- [Jenkins](https://www.jenkins.io/download/) **V.2.289.1** for CI/CD
- [docker](https://docs.docker.com/engine/install/) **V.20.10.7** for container support
- [SonarQube](https://docs.sonarqube.org/latest/setup/install-server/) for code analysis

This pipeline also take use of 
- [Git](https://www.jenkins.io/download/) for Code Versioning 
- [Google Cloud SDK](https://cloud.google.com/sdk) for [Kubernetes](https://kubernetes.io/) cluster
- [kubectl](https://kubernetes.io/docs/reference/kubectl/overview/) command line tool

## Pipeline
Using this repositiory I created a multi-branch pipeline, with master and develop branches. 

![](DocumentationResources/pipeline.png)

## Exposed Ports
List of all the running appliaction & their ports
| S.No | Application | Branch | Host & Ports |
|--|--|--|--|
|1|Jenkins | NA | localhost:8080
|2|SonarQube | NA | localhost:9000
|3|DotNet Application | master | localhost:7200 <br/> `kubernetes-exposed-url`:30157| 
|3|DotNet Application | develop | localhost:7300 <br/> `kubernetes-exposed-url`:30158| 