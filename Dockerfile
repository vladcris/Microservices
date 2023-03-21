# image for the .NET CLI .NET runtime ASP.NET Core
FROM mcr.microsoft.com/dotnet/sdk:7.0 as build  
# restore in /ShoppingCart our project and dependencies from our project relative location
WORKDIR /src
COPY ["ShoppingCart/ShoppingCart.csproj", "ShoppingCart/"]
RUN dotnet restore "ShoppingCart/ShoppingCart.csproj"

# copy the resulting in /src/ShoppingCart and build the app in realease mode
COPY . .
WORKDIR "/src/ShoppingCart"
RUN dotnet build "ShoppingCart.csproj" -c Release -o /app/build


# publish the app to the specified location
FROM build AS publish
RUN dotnet publish "ShoppingCart.csproj" -c Release -o /app/publish
 
# creates the final container image 
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShoppingCart.dll"]