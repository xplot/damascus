FROM microsoft/aspnet

COPY ./src /damascus
WORKDIR /damascus/Damascus.Web	
RUN ["dnu", "restore"]

EXPOSE 5001

CMD dnx . kestrel