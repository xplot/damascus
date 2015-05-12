#Start remote deploy
echo "Starting remote deploy"

echo "Creating Login information"
mkdir -p ~/.ssh
chmod 600 ~/.ssh/damascus.pk
eval `ssh-agent -s`
ssh-add ~/.ssh/damascus.pk


echo "Copying sensitive variables to Server"
scp -i ~/.ssh/damascus.pk config_variables $deploy_user@$deploy_box:/deployment

ssh -i ~/.ssh/damascus.pk $deploy_user@$deploy_box 'bash -s' < remote-deploy.sh

echo "Finishing deploy"