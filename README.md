# Backend Project Setup Guide

## Инструкция по установке:

1. Подтягиваем изменения из промежуточной ветки dev:

    ```git pull origin dev```

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

Тестовый пользователь данные:
```aiignore
{
    "email": "test@test.com",
    "password": "Test123!"
}

student@test.com / Test123!
teacher@test.com / Test123!
```

### Докер подъехал!!!

Теперь этот проект можно запустить одной командой!

В корне проекта <ваш путь>\>
    1 - ```docker compose down -v``` - останавливает и удаляет контейнеры -v удаляет типо данные БД.
    2 - ```docker compose up --build``` - собираем
    3 - ```docker compose up``` - запускаем 
    4 - ```docker ps``` - можем проверить запустились ли наши контейнеры

Здесь лицезреем:
    ```http://localhost:8080```

Так для тех кто уже пытался запустить докер последовательность команд следующая:
    1, 2

Для тех кто впервые можно просто:
    2

В случае проблем сносим локальную БД... 
Шутка, Пишите мне.




