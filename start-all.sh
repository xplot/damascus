#!/bin/bash

#export CURRENT_DIR=pwd

echo "Starting Damascus.Web"
cd src/Damascus.Web/
rm -rf run.pid
nohup dnx . kestrel --server.urls="http://localhost:80" > web.log 2>/dev/null | echo $! >> run.pid &
echo "Finished starting Damascus.Web"

cd ../Damascus.MessageChannel/
echo "Starting Damascus.MessageChannel"
rm -rf run.pid
nohup dnx . run > damascus.log 2>/dev/null | echo $! >> run.pid &
echo "Finished starting Damascus.MessageChannel"

echo "Finished starting Services"

#cd $CURRENT_DIR