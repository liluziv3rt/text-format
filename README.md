Документация: Приложение для управления контактами

Решение представляет собой систему управления контактами с:

    Поддержкой сериализации в нескольких форматах (JSON, XML, YAML, CSV)

    Консольным интерфейсом пользователя

    Модульными тестами основной функциональности

Класс Contact

    Назначение
    Хранит информацию о контакте.

Класс universalMethods

    Назначение
    Обеспечивает сериализацию и десериализацию в различных форматах.

Основные методы:
    ReadFromFile<T>(string path)

Описание

    Читает и десериализует файл по его расширению
Параметры

    path: Путь к файлу
Возвращает
    
    Десериализованный объект типа T

Исключения
    
    Exception для неподдерживаемых форматов
       Формат-специфичные исключения для поврежденных файлов

WriteToFile<T>(T data, string path)
Описание
    
    Сериализует данные и записывает в файл

Параметры

       data: Объект для сериализации
       path: Путь для сохранения

Исключения

    Exception для неподдерживаемых форматов
    Ошибки сериализации

Класс Menu (Меню)

Функциональность

    Интерактивное консольное меню
    CRUD-операции с контактами
    Импорт/экспорт
    Сортировка и поиск

Опции меню

    Загрузить контакты: Импорт из JSON/XML/CSV/YAML
    Сохранить контакты: Экспорт в JSON/XML/CSV/YAML
    Показать контакты: Табличное отображение
    Сортировать: По имени, фамилии, email или телефону
    Поиск: Полнотекстовый поиск по всем полям
    Добавить: Создание нового контакта
    Удалить: Удаление по индексу
    Изменить: Редактирование контакта
    Выход: Завершение работы

Обработка ошибок

    Проверка ввода в меню
    Грамотная обработка файловых операций
    Подтверждение опасных действий

Класс UnitTest1

    Тесты сериализации
    JSON_WriteAndRead_Success(): Проверка работы с JSON
    XML_WriteAndRead_Success(): Проверка работы с XML
    YAML_WriteAndRead_Success(): Проверка работы с YAML

Тесты исключений

    Read_InvalidFormat_ThrowsException(): Проверка неподдерживаемых форматов (чтение)
    Write_InvalidFormat_ThrowsException(): Проверка неподдерживаемых форматов (запись)

Методика тестирования

    Создание тестового контакта
    Запись во временный файл
    Чтение из файла
    Проверка целостности данных
    Удаление временного файла
