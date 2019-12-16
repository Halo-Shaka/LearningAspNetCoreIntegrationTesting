pipeline {
    agent any

    environment {
        Env_BuildUser = getBuildUser()
        Env_ChangedFilesString = getChangedFilesList().join('<br>')
    }

    stages {

        stage('deploy') {
            steps {
                script {
                    error "111";
                }
            }
        }

    }
    post {

        failure {
            echo "${Env_BuildUser}";
			
			if (Env_ChangedFilesString)
			{
				            echo "${Env_ChangedFilesString}";
			}
			

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