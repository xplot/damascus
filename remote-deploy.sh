#!/bin/bash

eval "$(ssh-agent -s)"
ssh-add ~/.ssh/id_rsa

sudo git clone git@github.com:xplot/damascus.git /deployment
cd /deployment
sudo git checkout new

#Docker Installation
sudo docker build -t damascus.web .

sudo docker run -t -d -p 80:5001 damascus.web
sudo docker run -t -d -p 80:5001 damascus.web
