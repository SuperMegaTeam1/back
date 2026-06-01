# Backend Project Setup Guide

## Инструкция по установке:


1. Подтягиваем изменения из ветки main:

    ```git pull origin main```

На проекте стоит, защита от прямого пуша в маин.

2.1. Настраиваем *appsettings.json* для работы с БД  согласно примеру в *appsettings.example.json*: 
Для этого должна быть создана база данных (```CREATE DATABASE moi-ivmiit-db```).
Вносим данные своего порта, username, password

2.2 Установить зависимости:

    ```dotnet restore```

2.4. Подтянуть БД из миграций:

    ```dotnet ef database update --project Backend.Infrastructure --startup-project Backend.API```

2.5. Запустить проект:

    ```dotnet run --project Backend.API```

3. Вариант запуска через докер
    ```docker compose --build```

При добавлении новых моделей не забывать:

    dotnet ef migrations add MigrationName
    dotnet ef database update

Тестовый пользователь для проверки учителя данные:
```aiignore
{
    "email": "teacher1@test.com",
    "password": "Password123!"
}

Тестовый пользователь для проверки студента данные:
```aiignore
{
    "email": "student1@test.com",
    "password": "Password123!"
}

student1@test.com / Password123!
teacher1@test.com / Password123!
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




