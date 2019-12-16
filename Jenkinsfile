pipeline {
    agent any

    environment {
        buildUser = getBuildUser()
        changedFilesString = getChangedFilesList().join('<br>')
    }

    stages {

        stage('deploy') {
            steps {
                script {
                   println(currentBuild.changeSets);
                }
            }
        }

    }
    post {

        failure {
            echo "${buildUser}";
						

        }

    }
}


String getChangedFilesList() {

    changedFiles = []
    for (changeLogSet in currentBuild.changeSets) {
        for (entry in changeLogSet.getItems()) { // for each commit in the detected changes
            for (file in entry.getAffectedFiles()) {
                changedFiles.add(file.getPath()) // add changed file to list
            }
        }
    }
    return changedFiles;
}

String getBuildUser() {
    def USER = wrap([$class: 'BuildUser']) {
        return env.BUILD_USER
    }
    return "${USER}";
}