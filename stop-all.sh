#!/bin/bash

if [ -f src/Damascus.MessageChannel/run.pid ]; then
    kill -9 $(cat src/Damascus.MessageChannel/run.pid)
    rm -rf src/Damascus.MessageChannel/run.pid
fi

if [ -f src/Damascus.Web/run.pid ]; then
    kill -9 $(cat src/Damascus.Web/run.pid)
    rm -rf src/Damascus.Web/run.pid
fi