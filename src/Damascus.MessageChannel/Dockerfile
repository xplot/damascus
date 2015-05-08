FROM microsoft/aspnet

COPY ./src /damascus
WORKDIR /damascus/Damascus.MessageChannel	
RUN ["dnu", "run"]

CMD dnx . run