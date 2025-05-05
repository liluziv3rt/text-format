using ProcessingVariousTextFormats;

namespace unit_tests
{
    [TestClass]
    public class UnitTest1
    {
        private universalMethods processor = new universalMethods();
        private Contact testContact = new Contact
        {
            Name = "Иван",
            Surname = "Иванов",
            Email = "ivan@mail.ru",
            Phone = "79991234567",
            Adress = "ул. Пушкина, д.10"
        };

        [TestMethod]
        public void JSON_WriteAndRead_Success()
        {
            string file = "test.json";

            // Запись
            processor.WriteToFile(testContact, file);

            // Чтение
            var result = processor.ReadFromFile<Contact>(file);

            Assert.AreEqual(testContact.Name, result.Name);
            Assert.AreEqual(testContact.Phone, result.Phone);

            File.Delete(file);
        }

        [TestMethod]
        public void XML_WriteAndRead_Success()
        {
            string file = "test.xml";

            processor.WriteToFile(testContact, file);
            var result = processor.ReadFromFile<Contact>(file);

            Assert.AreEqual(testContact.Email, result.Email);
            Assert.AreEqual(testContact.Adress, result.Adress);

            File.Delete(file);
        }

        [TestMethod]
        public void YAML_WriteAndRead_Success()
        {
            string file = "test.yaml";

            processor.WriteToFile(testContact, file);
            var result = processor.ReadFromFile<Contact>(file);

            Assert.AreEqual(testContact.Name, result.Name);
            Assert.AreEqual(testContact.Email, result.Email);

            File.Delete(file);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Read_InvalidFormat_ThrowsException()
        {
            string file = "test.txt";
            File.WriteAllText(file, "просто текст");

            processor.ReadFromFile<Contact>(file);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Write_InvalidFormat_ThrowsException()
        {
            string file = "test.dat";

            processor.WriteToFile(testContact, file);
        }
    }
}
