# grab the sdk image, create an name for it  "build"

FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /source
EXPOSE 80

# copy csproj and restore as distinct layers
#COPY *.props .
COPY *.sln .
COPY Directory.Build.props .

# Base projects

COPY Base.BLL/*.csproj ./Base.BLL/
COPY Base.DAL/*.csproj ./Base.DAL/
COPY Base.DAL.EF/*.csproj ./Base.DAL.EF/
COPY Base.Domain/*.csproj ./Base.Domain/
COPY Base.Extensions/*.csproj ./Base.Extensions/
COPY Base.Resources/*.csproj ./Base.Resources/
COPY Helpers.WebApp/*.csproj ./Helpers.WebApp/

COPY Base.Contracts.Base/*.csproj ./Base.Contracts.Base/
COPY Base.Contracts.BLL/*.csproj ./Base.Contracts.BLL/
COPY Base.Contracts.DAL/*.csproj ./Base.Contracts.DAL/
COPY Base.Contracts.Domain/*.csproj ./Base.Contracts.Domain/


# App projects
COPY App.BLL/*.csproj ./App.BLL/
COPY App.BLL.DTO/*.csproj ./App.BLL.DTO/
COPY App.Contracts.BLL/*.csproj ./App.Contracts.BLL/
COPY App.Contracts.DAL/*.csproj ./App.Contracts.DAL/
COPY App.DAL.DTO/*.csproj ./App.DAL.DTO/
COPY App.DAL.EF/*.csproj ./App.DAL.EF/
COPY App.Domain/*.csproj ./App.Domain/
COPY App.Public.DTO.v1/*.csproj ./App.Public.DTO.v1/
COPY App.Resources/*.csproj ./App.Resources/
COPY Test.WebApp/*.csproj ./Test.WebApp/

COPY WebApp/*.csproj ./WebApp/

# create the first layer with just nuget packages installed 
RUN dotnet restore

# copy everything else and build app



# Base projects

COPY Base.BLL/. ./Base.BLL/
COPY Base.DAL/. ./Base.DAL/
COPY Base.DAL.EF/. ./Base.DAL.EF/
COPY Base.Domain/. ./Base.Domain/
COPY Base.Extensions/. ./Base.Extensions/
COPY Base.Resources/. ./Base.Resources/
COPY Helpers.WebApp/. ./Helpers.WebApp/

COPY Base.Contracts.Base/. ./Base.Contracts.Base/
COPY Base.Contracts.BLL/. ./Base.Contracts.BLL/
COPY Base.Contracts.DAL/. ./Base.Contracts.DAL/
COPY Base.Contracts.Domain/. ./Base.Contracts.Domain/


# App projects
COPY App.BLL/. ./App.BLL/
COPY App.BLL.DTO/. ./App.BLL.DTO/
COPY App.Contracts.BLL/. ./App.Contracts.BLL/
COPY App.Contracts.DAL/. ./App.Contracts.DAL/
COPY App.DAL.DTO/. ./App.DAL.DTO/
COPY App.DAL.EF/. ./App.DAL.EF/
COPY App.Domain/. ./App.Domain/
COPY App.Public.DTO.v1/. ./App.Public.DTO.v1/
COPY App.Resources/. ./App.Resources/
COPY Test.WebApp/. ./Test.WebApp/

COPY WebApp/. ./WebApp/

WORKDIR /source/WebApp

# compile the app with Release option and put files into dir "out"
RUN dotnet publish -c Release -o out

# create a new image, from aspnet runtime (no compilers)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime

# RUN apt-get -y update && apt-get -y upgrade
WORKDIR /app

# copy files from previous image (the "build")
COPY --from=build /source/WebApp/out ./
ENV  ConnectionStrings:NpgsqlConnection Server=apartmentwebapp.postgres.database.azure.com;Database=postgres;Port=5432;User Id=kpoldr@apartmentwebapp;Password=VoiLeibadeKuningas123;
# run this command when container starts up (not runned during image build phase)
ENTRYPOINT ["dotnet", "WebApp.dll"]