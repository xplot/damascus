#!/bin/bash

if test `uname` = Darwin; then
    cachedir=~/Library/Caches/KBuild
else
    if [ -z $XDG_DATA_HOME ]; then
        cachedir=$HOME/.local/share
    else
        cachedir=$XDG_DATA_HOME;
    fi
fi
mkdir -p $cachedir

url=https://www.nuget.org/nuget.exe

if test ! -f $cachedir/nuget.exe; then
    wget -O $cachedir/nuget.exe $url 2>/dev/null || curl -o $cachedir/nuget.exe --location $url /dev/null
fi

if test ! -e .nuget; then
    mkdir .nuget
    cp $cachedir/nuget.exe .nuget/nuget.exe
fi

if test ! -d packages/KoreBuild; then
    #To specify the Source in the following line shouldnt be necesary but it's making my build to fail
    mono .nuget/nuget.exe install KoreBuild -ExcludeVersion -o packages -nocache -pre -Source https://www.myget.org/F/aspnetvnext/api/v2
    mono .nuget/nuget.exe install Sake -version 0.2 -o packages -ExcludeVersion
fi

if ! type dnvm > /dev/null 2>&1; then
    source packages/KoreBuild/build/dnvm.sh
fi

if ! type dnx > /dev/null 2>&1; then
    dnvm upgrade
fi

mono packages/Sake/tools/Sake.exe -I packages/KoreBuild/build -f makefile.shade "$@"

#Start remote deploy
echo "Starting remote deploy"

echo "Creating Login information"
mkdir -p ~/.ssh
base64 --decode ./id_rsa_deploy_base64 > ~/.ssh/damascus.pk
chmod 600 ~/.ssh/damascus.pk
eval `ssh-agent -s`
ssh-add ~/.ssh/damascus.pk


echo "Copying sensitive variables to Server"
scp -i ~/.ssh/damascus.pk config_variables username$deploy_user@$deploy_box:/deployment

ssh -i ~/.ssh/damascus.pk $deploy_user@$deploy_box 'bash -s' < remote-deploy.sh

echo "Finishing deploy"