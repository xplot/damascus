#!/bin/bash

DEPLOYMENT_FOLDER='/deployment'
DAMASCUS_FOLDER='/deployment/damascus'
GIT_BRANCH='new'

sudo rm -rf $DAMASCUS_FOLDER

eval "$(ssh-agent -s)"
sudo ssh-add ~/.ssh/id_rsa
sudo git clone git@github.com:xplot/damascus.git $DAMASCUS_FOLDER

cd $DAMASCUS_FOLDER
sudo chmod 777 $DAMASCUS_FOLDER
sudo git checkout $GIT_BRANCH

cp $DEPLOYMENT_FOLDER/config_variables $DAMASCUS_FOLDER/config_variables
sudo python travis-after-deploy.py