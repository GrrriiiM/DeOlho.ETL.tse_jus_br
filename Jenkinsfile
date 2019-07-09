def exec(cmd) {
    if (Boolean.valueOf(env.UNIX)) {
        sh cmd
    }
    else {
        bat cmd
    }
}

node {
    stage('Git pull') {
        checkout scm
    }
    stage('Test') {
        exec('dotnet test src /p:CollectCoverage=true /p:Exclude="[xunit*]*" /p:CoverletOutputFormat="cobertura" /p:CoverletOutput=./coverage/"')
    }
    stage('Code Coverage Report') {
        cobertura autoUpdateHealth: false, autoUpdateStability: false, coberturaReportFile: 'src/5-UnitTests/coverage/coverage.cobertura.xml', conditionalCoverageTargets: '70, 0, 0', lineCoverageTargets: '80, 0, 0', maxNumberOfBuilds: 0, methodCoverageTargets: '80, 0, 0', onlyStable: false, sourceEncoding: 'ASCII', zoomCoverageChart: false
    }
    stage('Docker-compose up') {
        exec('docker-compose up -d')
    }
}