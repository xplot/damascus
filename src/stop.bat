echo "stoping Damascus services"

net stop "OpenIT.Damascus.SmsChannel"
net stop "OpenIT.Damascus.IVRChannel"
net stop "OpenIT.Damascus.EmailChannel"
net stop "OpenIT.Damascus.Report"

pause