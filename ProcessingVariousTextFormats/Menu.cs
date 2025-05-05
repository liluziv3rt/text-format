using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcessingVariousTextFormats
{
    class Program
    {
        // Список для хранения контактов в памяти
        private static List<Contact> contacts = new List<Contact>();

        // Экземпляр класса для работы с различными форматами файлов
        private static universalMethods processor = new universalMethods();

        static void Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                // Основное меню приложения
                Console.WriteLine("\nМеню управления контактами:");
                Console.WriteLine("1. Загрузить контакты из файла");
                Console.WriteLine("2. Сохранить контакты в файл");
                Console.WriteLine("3. Показать все контакты");
                Console.WriteLine("4. Сортировать контакты");
                Console.WriteLine("5. Найти контакт");
                Console.WriteLine("6. Добавить новый контакт");
                Console.WriteLine("7. Удалить контакт");
                Console.WriteLine("8. Изменить контакт");
                Console.WriteLine("9. Выход");
                Console.Write("Выберите действие: ");

                // Обработка выбора пользователя
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    try
                    {
                        switch (choice)
                        {
                            case 1:
                                LoadContactsFromFile(); // Загрузка из файла
                                break;
                            case 2:
                                SaveContactsToFile();   // Сохранение в файл
                                break;
                            case 3:
                                DisplayContacts(contacts);  // Отображение
                                break;
                            case 4:
                                SortContacts();         // Сортировка
                                break;
                            case 5:
                                SearchContacts();       // Поиск
                                break;
                            case 6:
                                AddContact();           // Добавление
                                break;
                            case 7:
                                DeleteContact();        // Удаление
                                break;
                            case 8:
                                EditContact();          // Редактирование
                                break;
                            case 9:
                                exit = true;            // Выход
                                break;
                            default:
                                Console.WriteLine("Неверный выбор. Попробуйте снова.");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Пожалуйста, введите число от 1 до 9.");
                }
            }
        }

        private static void LoadContactsFromFile()      // Загрузка контактов из файла (JSON/XML/CSV/YAML)
        {
            Console.Write("Введите путь к файлу (JSON/XML/CSV/YAML): ");
            string filePath = Console.ReadLine();

            try
            {
                contacts = processor.ReadFromFile<List<Contact>>(filePath);     // Чтение и десериализация контактов
                Console.WriteLine($"Успешно загружено {contacts.Count} контактов.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке файла: {ex.Message}");
            }
        }

        private static void SaveContactsToFile()        // Сохранение контактов в файл (JSON/XML/CSV/YAML)
        {
            if (contacts.Count == 0)
            {
                Console.WriteLine("Нет контактов для сохранения.");
                return;
            }

            Console.Write("Введите путь к файлу для сохранения (JSON/XML/CSV/YAML): ");
            string filePath = Console.ReadLine();

            try
            {
                // Сериализация и сохранение контактов
                processor.WriteToFile(contacts, filePath);
                Console.WriteLine("Контакты успешно сохранены.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
            }
        }

        private static void DisplayContacts(List<Contact> contactsToDisplay)        // Отображение списка контактов в табличном формате
        {
            if (contactsToDisplay.Count == 0)
            {
                Console.WriteLine("Нет контактов для отображения.");
                return;
            }

            Console.WriteLine("\nСписок контактов:");
            Console.WriteLine("-----------------------------------------------------------------");
            Console.WriteLine("| №  | Имя       | Фамилия   | Email               | Телефон     |");
            Console.WriteLine("-----------------------------------------------------------------");

            for (int i = 0; i < contactsToDisplay.Count; i++)
            {
                var contact = contactsToDisplay[i];
                Console.WriteLine($"| {i + 1,-3}| {contact.Name,-10}| {contact.Surname,-10}| {contact.Email,-19}| {contact.Phone,-12}|");
            }
            Console.WriteLine("-----------------------------------------------------------------");
        }

        private static void SortContacts()      // Сортировка контактов по выбранному полю
        {
            if (contacts.Count == 0)
            {
                Console.WriteLine("Нет контактов для сортировки.");
                return;
            }

            Console.WriteLine("Сортировать по:");
            Console.WriteLine("1. Имени");
            Console.WriteLine("2. Фамилии");
            Console.WriteLine("3. Email");
            Console.WriteLine("4. Телефону");
            Console.Write("Выберите поле для сортировки: ");

            if (int.TryParse(Console.ReadLine(), out int sortChoice))
            {
                // Сортировка в зависимости от выбора пользователя
                List<Contact> sortedContacts = sortChoice switch
                {
                    1 => contacts.OrderBy(c => c.Name).ToList(),
                    2 => contacts.OrderBy(c => c.Surname).ToList(),
                    3 => contacts.OrderBy(c => c.Email).ToList(),
                    4 => contacts.OrderBy(c => c.Phone).ToList(),
                    _ => contacts
                };

                contacts = sortedContacts;
                Console.WriteLine("Контакты отсортированы.");
                DisplayContacts(contacts);
            }
            else
            {
                Console.WriteLine("Неверный выбор сортировки.");
            }
        }

        private static void SearchContacts()    // Поиск контактов по подстроке (по всем полям)
        {
            if (contacts.Count == 0)
            {
                Console.WriteLine("Нет контактов для поиска.");
                return;
            }

            Console.Write("Введите строку для поиска: ");
            string searchTerm = Console.ReadLine().ToLower();

            // Поиск по всем полям контакта
            var foundContacts = contacts.Where(c =>
                c.Name.ToLower().Contains(searchTerm) ||
                c.Surname.ToLower().Contains(searchTerm) ||
                c.Email.ToLower().Contains(searchTerm) ||
                c.Phone.ToLower().Contains(searchTerm) ||
                c.Adress.ToLower().Contains(searchTerm)).ToList();

            if (foundContacts.Count > 0)
            {
                Console.WriteLine($"Найдено {foundContacts.Count} контактов:");
                DisplayContacts(foundContacts);
            }
            else
            {
                Console.WriteLine("Контакты не найдены.");
            }
        }

        private static void AddContact()    // Добавление нового контакта
        {
            Console.WriteLine("\nДобавление нового контакта:");

            var newContact = new Contact();

            // Ввод данных нового контакта
            Console.Write("Имя: ");
            newContact.Name = Console.ReadLine();

            Console.Write("Фамилия: ");
            newContact.Surname = Console.ReadLine();

            Console.Write("Email: ");
            newContact.Email = Console.ReadLine();

            Console.Write("Телефон: ");
            newContact.Phone = Console.ReadLine();

            Console.Write("Адрес: ");
            newContact.Adress = Console.ReadLine();

            contacts.Add(newContact);   // Добавление в список
            Console.WriteLine("Контакт успешно добавлен.");
        }

        private static void DeleteContact()     // Удаление контакта по номеру
        {
            if (contacts.Count == 0)
            {
                Console.WriteLine("Нет контактов для удаления.");
                return;
            }

            DisplayContacts(contacts);
            Console.Write("Введите номер контакта для удаления: ");

            // Проверка корректности ввода номера
            if (int.TryParse(Console.ReadLine(), out int contactNumber) && contactNumber > 0 && contactNumber <= contacts.Count)
            {
                var contactToRemove = contacts[contactNumber - 1];
                Console.WriteLine($"Вы уверены, что хотите удалить контакт {contactToRemove.Name} {contactToRemove.Surname}? (y/n)");
                if (Console.ReadLine().ToLower() == "y")
                {
                    contacts.RemoveAt(contactNumber - 1);
                    Console.WriteLine("Контакт удален.");
                }
            }
            else
            {
                Console.WriteLine("Неверный номер контакта.");
            }
        }

        private static void EditContact()   // Редактирование существующего контакта
        {
            if (contacts.Count == 0)
            {
                Console.WriteLine("Нет контактов для редактирования.");
                return;
            }

            DisplayContacts(contacts);
            Console.Write("Введите номер контакта для редактирования: ");

            // Проверка корректности ввода номера
            if (int.TryParse(Console.ReadLine(), out int contactNumber) && contactNumber > 0 && contactNumber <= contacts.Count)    
            {
                var contactToEdit = contacts[contactNumber - 1];

                Console.WriteLine("\nРедактирование контакта (оставьте поле пустым, чтобы не изменять):");

                // Редактирование каждого поля с возможностью оставить без изменений
                Console.Write($"Имя ({contactToEdit.Name}): ");
                var nameInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(nameInput)) contactToEdit.Name = nameInput;

                Console.Write($"Фамилия ({contactToEdit.Surname}): ");
                var surnameInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(surnameInput)) contactToEdit.Surname = surnameInput;

                Console.Write($"Email ({contactToEdit.Email}): ");
                var emailInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(emailInput)) contactToEdit.Email = emailInput;

                Console.Write($"Телефон ({contactToEdit.Phone}): ");
                var phoneInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(phoneInput)) contactToEdit.Phone = phoneInput;

                Console.Write($"Адрес ({contactToEdit.Adress}): ");
                var addressInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(addressInput)) contactToEdit.Adress = addressInput;

                Console.WriteLine("Контакт успешно обновлен.");
            }
            else
            {
                Console.WriteLine("Неверный номер контакта.");
            }
        }
    }
}