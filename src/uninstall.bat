@echo off

pushd Damascus.EmailChannel

cmd /c uninstall.bat

popd

pushd Damascus.SmsChannel

cmd /c uninstall.bat

popd

pushd Damascus.IVRChannel

cmd /c uninstall.bat

popd

pushd Damascus.Report

cmd /c uninstall.bat

popd

IF "%1" NEQ "nopause" pause