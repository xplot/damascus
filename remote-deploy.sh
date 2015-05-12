#!/bin/bash

sudo su

DEPLOYMENT_FOLDER='/deployment'
DAMASCUS_FOLDER='/deployment/damascus'
GIT_BRANCH='new'

rm -rf $DAMASCUS_FOLDER

#Git Restore
eval "$(ssh-agent -s)"
ssh-add ~/.ssh/id_rsa
git clone git@github.com:xplot/damascus.git $DAMASCUS_FOLDER
cd $DAMASCUS_FOLDER
git checkout $GIT_BRANCH

##Config Values
cp $DEPLOYMENT_FOLDER/config_variables $DAMASCUS_FOLDER/config_variables
python travis-after-deploy.py

###Restoring Dependencies

#Updating NuGet.Config
cp NuGet.Config ~/.config/NuGet/

cd $DAMASCUS_FOLDER/
./build.sh

./stop-all.sh

./start-all.sh

#Permissions in Deployment folder
chmod 777 $DAMASCUS_FOLDER