#!/bin/bash

cd /deployment
sudo git checkout new

#Docker Installation
sudo docker build -t damascus.web .

sudo docker run -t -d -p 80:5001 damascus.web
sudo docker run -t -d -p 80:5001 damascus.web
