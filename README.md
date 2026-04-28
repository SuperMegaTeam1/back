# Backend Project Setup Guide

## Инструкция по установке:

1. Подтягиваем изменения из main:

    ```git pull origin main```

2. Настраиваем *appsettings.json* для работы с БД  согласно примеру в *appsettings.example.json*: 
Для этого должна быть создана база данных (```CREATE DATABASE moi-ivmiit-db```).
Вносим данные своего порта, username, password

3. Установить зависимости:

    ```dotnet restore```

4. Подтянуть БД из миграций:

    ```dotnet ef database update --project Backend.Infrastructure --startup-project Backend.API```

5. Запустить проект:

    ```dotnet run --project Backend.API```

При добавлении новых моделей не забывать:

    dotnet ef migrations add MigrationName
    dotnet ef database update



