@echo off

pushd Damascus.EmailChannel

cmd /c install.bat

popd

pushd Damascus.SmsChannel

cmd /c install.bat

popd

pushd Damascus.IvrChannel

cmd /c install.bat

popd

pushd Damascus.Report

cmd /c install.bat

popd

IF "%1" NEQ "nopause" pause