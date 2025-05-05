using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Xml.Serialization;
using Newtonsoft.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using CsvHelper;
using System.Globalization;

namespace ProcessingVariousTextFormats
{
    public class Contact
    {
        public string Name { get; set; }    // Имя контакта
        public string Surname { get; set; } // Фамилия контакта
        public string Email { get; set; }   // Электронная почта
        public string Phone { get; set; }   // Номер телефона
        public string Adress { get; set; }  // Адрес проживания
    }
    public class universalMethods   // Универсальный класс для работы с различными форматами файлов
    {
        public T ReadFromFile<T>(string path)   // Чтение данных из файла с автоматическим определением формата
        {
            string extension = Path.GetExtension(path).ToLower();  // Получение расширения файла и приведение всех его букв к нижнему регистру
            string content = File.ReadAllText(path);

            // Выбор соответствующего десериализатора
            if (extension == ".json")
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            else if (extension == ".xml")
            {
                return DeserializeXml<T>(content);
            }
            else if (extension == ".csv")
            {
                return DeserializeCsv<T>(content);
            }
            else if (extension == ".yaml" || extension == ".yml")
            {
                return DeserializeYaml<T>(content);
            }
            else throw new Exception("Формат не поддерживается");
        }

        public void WriteToFile<T>(T data, string path) // Запись данных в файл с автоматическим определением формата
        {
            string extension = Path.GetExtension(path).ToLower();  // Получение расширения файла и приведение всех его букв к нижнему регистру
            string content;

            // Выбор соответствующего сериализатора
            if (extension == ".json")
            {
                content = JsonConvert.SerializeObject(data);
            }
            else if (extension == ".xml")
            {
                content = SerializeXml<T>(data);
            }
            else if (extension == ".csv")
            {
                content = SerializeCsv<T>(data);
            }
            else if (extension == ".yaml" || extension == ".yml")
            {
                content = SerializeYaml<T>(data);
            }
            else throw new Exception("Формат не поддерживается");
            File.WriteAllText(path, content);    // Запись результата в файл

        }

        private static T DeserializeXml<T>(string content)  // Десериализация XML контента
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using var reader = new StringReader(content);
                return (T)serializer.Deserialize(reader);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception($"Некорректный XML формат в файле ", ex);
            }
        }

        private static string SerializeXml<T>(T data)   // Сериализация данных в XML формат
        {
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using var writer = new StringWriter();
                serializer.Serialize(writer, data);
                return writer.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сериализации XML", ex);
            }
        }
        private static T DeserializeYaml<T>(string content) // Десериализация YAML контента
        {
            try
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)   // Использование camelCase
                    .Build();
                return deserializer.Deserialize<T>(content);
            }
            catch (YamlDotNet.Core.YamlException ex)
            {
                throw new Exception($"Некорректный YAML формат в файле ", ex);
            }
        }

        private static string SerializeYaml<T>(T data)  // Сериализация данных в YAML формат
        {
            try
            {
                var serializer = new SerializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)   // Использование camelCase
                    .Build();
                return serializer.Serialize(data);
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сериализации YAML", ex);
            }
        }

        private static T DeserializeCsv<T>(string content)  // Десериализация CSV контента
        {
            try
            {
                using var reader = new StringReader(content);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);    // Чтение с инвариантной культурой

                return csv.GetRecord<T>();  // Получение одной записи
            }
            catch (CsvHelperException ex)
            {
                throw new Exception($"Некорректный CSV формат в файле ", ex);
            }
        }

        private static string SerializeCsv<T>(T data)   // Сериализация данных в CSV формат
        {
            try
            {
                using var writer = new StringWriter();
                using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);    // Запись с инвариантной культурой
                csv.WriteRecord(data);  // Запись одной записи
                return writer.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сериализации CSV", ex);
            }
        }
    }
}
