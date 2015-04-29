echo "starting Damascus services"

net start "OpenIT.Damascus.IvrChannel"
net start "OpenIT.Damascus.SmsChannel"
net start "OpenIT.Damascus.EmailChannel"
net start "OpenIT.Damascus.Report"

IF /I "%1" NEQ "nopause" pause
