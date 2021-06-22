#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-nanoserver-1903 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-nanoserver-1903 AS build
WORKDIR /src
COPY ["KKIHub.ContentSync.Web.csproj", ""]
COPY ["gulpfile.js", ""]
COPY ["package.json", ""]


RUN dotnet restore "./KKIHub.ContentSync.Web.csproj"
RUN set -ex; \
	if ! command -v gpg > /dev/null; then \
		apt-get update; \
		apt-get install -y --no-install-recommends \
			gnupg2 \
			dirmngr \
		; \
		rm -rf /var/lib/apt/lists/*; \
	fi && curl -sL https://deb.nodesource.com/setup_12.x | bash - && apt-get update && apt-get install -y build-essential nodejs
COPY . .

WORKDIR "/src/."
RUN dotnet build "KKIHub.ContentSync.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN npm install
RUN dotnet publish "KKIHub.ContentSync.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KKIHub.ContentSync.Web.dll"]