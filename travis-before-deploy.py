import os
import fileinput
import shutil

def environment_dict():
    imeet_dict = {}
    for variable in os.environ.keys():
        variablel = variable.lower()
        if "damascus." in variablel:
            imeet_dict[variablel.replace("damascus.", "")] = os.environ[variablel]
    return imeet_dict


def create_deploy_file(deploy_file, environment_dic):
    pretty_name = lambda x: x.replace("damascus.", "")
    git_repository = 'git@github.com:xplot/damascus.git'
    deployment_folder = '/deployment'
    git_branch = 'new'
        
    with open(deploy_file, 'w') as f:
        f.write('#!/bin/bash\n')
        
        #Exporting Travis variables to use in remote server
        for damascus_var in environment_dic:
            f.write('export %s = %s \n' % (pretty_name(damascus_var), os.environ[damascus_var]))
        f.write('eval "$(ssh-agent -s)"\n')
        f.write('ssh-add ~/.ssh/id_rsa\n')
        f.write('sudo git clone %s %s\n' %(git_repository, deployment_folder))
        f.write('cd %s\n' % deployment_folder)
        f.write('sudo git checkout %s\n' % git_branch)
        f.write('python travis-after-deploy.py\n')
        f.write('sudo docker build -t damascus.web .\n')
        f.write('sudo docker run -t -d -p 80:5001 damascus.web\n')
        
imeet_dict = environment_dict()
create_deploy_file('remote-deploy.sh', imeet_dict)