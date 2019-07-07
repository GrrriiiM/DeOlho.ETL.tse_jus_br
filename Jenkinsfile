pipeline {
    agent any
    stages {
        stage('Test') {
            steps {
                bat 'dotnet test src/4-API /p:CollectCoverage=true /p:Exclude="[xunit*]*" /p:CoverletOutputFormat="cobertura" /p:CoverletOutput=./coverage/"'
            }
        }
        stage('Code Coverage Report') {
            steps {
                cobertura autoUpdateHealth: false, autoUpdateStability: false, coberturaReportFile: 'UnitTest/coverage/coverage.cobertura.xml', conditionalCoverageTargets: '70, 0, 0', lineCoverageTargets: '80, 0, 0', maxNumberOfBuilds: 0, methodCoverageTargets: '80, 0, 0', onlyStable: false, sourceEncoding: 'ASCII', zoomCoverageChart: false
            }
        }
        stage('Deploy') {
            steps {
                echo 'Deploying....'
            }
        }
    }
}