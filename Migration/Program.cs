using System;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MigrationTrackingApp
{
    class Program
    {
        static void Main()
        {
            App app = new App();
            app.Run();
        }
    }

    public class App
    {
        private readonly UserService userService;
        private readonly ApplicationService applicationService;

        public App()
        {
            userService = new UserService();
            applicationService = new ApplicationService();
        }

        public void Run()
        {
            Console.WriteLine("Приложение для миграционного учета");
            User currentUser = null;

            while (true)
            {
                if (currentUser == null)
                {
                    Console.WriteLine("1. Войти");
                    Console.WriteLine("2. Зарегистрироваться");
                    Console.Write("Выберите действие: ");
                    string choice = Console.ReadLine();

                    if (choice == "1")
                    {
                        Console.Write("Введите Email: ");
                        string email = Console.ReadLine();
                        Console.Write("Введите пароль: ");
                        string password = Console.ReadLine();

                        currentUser = userService.Authenticate(email, password);
                        if (currentUser == null)
                        {
                            Console.WriteLine("Неверные учетные данные. Попробуйте еще раз.");
                        }
                        else
                        {
                            Console.WriteLine($"Добро пожаловать, {currentUser.Role}!");
                        }
                    }
                    else if (choice == "2")
                    {
                        userService.RegisterUser();
                    }
                    else
                    {
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    }
                }
                else
                {
                    switch (currentUser.Role)
                    {
                        case "Admin":
                            AdminMenu(currentUser);
                            break;
                        case "Landlord":
                            LandlordMenu(currentUser);
                            break;
                        case "Specialist":
                            SpecialistMenu(currentUser);
                            break;
                        default:
                            Console.WriteLine("Неверная роль");
                            break;
                    }

                    Console.WriteLine("Вы хотите выйти из аккаунта? (да/нет)");
                    if (Console.ReadLine()?.ToLower() == "да")
                    {
                        currentUser = null;
                    }
                }
            }
        }

        private void AdminMenu(User admin)
        {
            string choice;
            do
            {
                Console.WriteLine("\nМеню администратора:");
                Console.WriteLine("1. Добавить пользователя");
                Console.WriteLine("2. Удалить пользователя");
                Console.WriteLine("3. Назначить роль пользователю");
                Console.WriteLine("0. Вернуться в главное меню");

                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        userService.AddUser();
                        break;
                    case "2":
                        userService.DisplayAllUsers();
                        userService.DeleteUser();
                        break;
                    case "3":
                        userService.DisplayAllUsers();
                        userService.AssignRole();
                        break;
                    case "0":
                        Console.WriteLine("Возврат в главное меню");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }
            } while (choice != "0");
        }

        private void LandlordMenu(User landlord)
        {
            string choice;
            do
            {
                Console.WriteLine("\nМеню собственника:");
                Console.WriteLine("1. Проверить статус заявления");
                Console.WriteLine("2. Подать новое заявление");
                Console.WriteLine("0. Вернуться в главное меню");

                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        applicationService.CheckStatusWithExportOption(landlord);
                        break;
                    case "2":
                        applicationService.SubmitApplication(landlord);
                        break;
                    case "0":
                        Console.WriteLine("Возврат в главное меню...");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }
            } while (choice != "0");
        }

        private void SpecialistMenu(User specialist)
        {
            string choice;
            do
            {
                Console.WriteLine("\nМеню сотрудника МВД:");
                Console.WriteLine("1. Просмотреть заявления");
                Console.WriteLine("2. Изменить статус заявления");
                Console.WriteLine("0. Вернуться в главное меню");

                choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        applicationService.ViewApplicationsWithDetails();
                        break;
                    case "2":
                        applicationService.ChangeStatus();
                        break;
                    case "0":
                        Console.WriteLine("Возврат в главное меню.");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }
            } while (choice != "0");
        }
    }

    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class UserService
    {
        private readonly List<User> users = new List<User>();

        public UserService()
        {
            users.Add(new User { Email = "admin@mail.ru", Password = "123", Role = "Admin" });
            users.Add(new User { Email = "landlord@mail.ru", Password = "123", Role = "Landlord" });
            users.Add(new User { Email = "specialist@mail.ru", Password = "123", Role = "Specialist" });
        }

        public User Authenticate(string email, string password)
        {
            return users.Find(user => user.Email == email && user.Password == password);
        }

        public void RegisterUser()
        {
            Console.Write("Введите Email для регистрации: ");
            string email = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();

            if (users.Exists(user => user.Email == email))
            {
                Console.WriteLine("Пользователь с таким Email уже существует.");
                return;
            }

            users.Add(new User { Email = email, Password = password, Role = "Landlord" });
            Console.WriteLine("Регистрация успешно завершена.");
        }

        public void AddUser()
        {
            Console.Write("Введите Email нового пользователя: ");
            string email = Console.ReadLine();
            Console.Write("Введите пароль нового пользователя: ");
            string password = Console.ReadLine();
            Console.Write("Введите роль нового пользователя(Admin/Landlord/Specialist): ");
            string role = Console.ReadLine();

            users.Add(new User { Email = email, Password = password, Role = role });
            Console.WriteLine("Пользователь успешно добавлен.");
        }

        public void DeleteUser()
        {
            Console.Write("Введите Email пользователя для удаления: ");
            string email = Console.ReadLine();
            User user = users.Find(u => u.Email == email);

            if (user != null)
            {
                users.Remove(user);
                Console.WriteLine("Пользователь успешно удален.");
            }
            else
            {
                Console.WriteLine("Пользователь не найден.");
            }
        }

        public void AssignRole()
        {
            Console.Write("Введите Email пользователя для назначения роли: ");
            string email = Console.ReadLine();
            User user = users.Find(u => u.Email == email);

            if (user != null)
            {
                Console.Write("Введите новую роль(Admin/Landlord/Specialist): ");
                string role = Console.ReadLine();
                user.Role = role;
                Console.WriteLine("Роль успешно назначена.");
            }
            else
            {
                Console.WriteLine("Пользователь не найден.");
            }
        }

        public void DisplayAllUsers()
        {
            Console.WriteLine("\nТекущие пользователи:");
            foreach (var user in users)
            {
                Console.WriteLine($"Email: {user.Email}, Роль: {user.Role}");
            }
        }
    }

    public class Application
    {
        private static int NextId = 1;
        public int Id { get; private set; }
        public string ApplicantEmail { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPassport { get; set; }
        public string Address { get; set; }
        public string MigrantName { get; set; }
        public string MigrantPassport { get; set; }
        public string VisaNumber { get; set; }
        public string MigrationCard { get; set; }
        public string Status { get; set; } = "На рассмотрении";

        public Application()
        {
            Id = NextId++;
        }
    }

    public class ApplicationService
    {
        private readonly List<Application> applications = new List<Application>();

        public void SubmitApplication(User landlord)
        {
            Application application = new Application { ApplicantEmail = landlord.Email };

            Console.Write("Введите ФИО собственника: ");
            application.OwnerName = Console.ReadLine();
            Console.Write("Введите серию и номер паспорта собственника: ");
            application.OwnerPassport = Console.ReadLine();
            Console.Write("Введите адрес учета: ");
            application.Address = Console.ReadLine();
            Console.Write("Введите ФИО мигранта: ");
            application.MigrantName = Console.ReadLine();
            Console.Write("Введите серию и номер паспорта мигранта: ");
            application.MigrantPassport = Console.ReadLine();
            Console.Write("Введите серию и номер визы: ");
            application.VisaNumber = Console.ReadLine();
            Console.Write("Введите серию и номер миграционной карты: ");
            application.MigrationCard = Console.ReadLine();

            applications.Add(application);
            Console.WriteLine("Заявление успешно подано.");
        }

        public void CheckStatusWithExportOption(User landlord)
        {
            var userApplications = applications.FindAll(a => a.ApplicantEmail == landlord.Email);

            if (userApplications.Count > 0)
            {
                foreach (var app in userApplications)
                {
                    Console.WriteLine($"Заявление ID {app.Id}: На {app.Address} - Статус: {app.Status}");
                }

                Console.WriteLine("1. Создать документ MS Word");
                Console.WriteLine("0. Назад");

                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    Console.Write("Введите ID заявления для экспорта: ");
                    if (int.TryParse(Console.ReadLine(), out int appId))
                    {
                        var app = userApplications.Find(a => a.Id == appId);
                        if (app != null)
                        {
                            CreateWordDocument(app);
                        }
                        else
                        {
                            Console.WriteLine("Заявление не найдено.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Некорректный ID заявления.");
                    }
                }
                else if (choice != "0")
                {
                    Console.WriteLine("Неверный выбор");
                }
            }
            else
            {
                Console.WriteLine("Заявлений не найдено.");
            }
        }

        private void CreateWordDocument(Application application)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, $"Заявление_{application.Id}.docx");

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                void AddParagraph(string text)
                {
                    Paragraph paragraph = body.AppendChild(new Paragraph());
                    Run run = paragraph.AppendChild(new Run());
                    run.AppendChild(new Text(text));
                }

                AddParagraph($"ID заявления: {application.Id}");
                AddParagraph($"ФИО собственника: {application.OwnerName}");
                AddParagraph($"Паспорт собственника: {application.OwnerPassport}");
                AddParagraph($"Адрес учета: {application.Address}");
                AddParagraph($"ФИО мигранта: {application.MigrantName}");
                AddParagraph($"Паспорт мигранта: {application.MigrantPassport}");
                AddParagraph($"Серия и номер визы: {application.VisaNumber}");
                AddParagraph($"Серия и номер миграционной карты: {application.MigrationCard}");
                AddParagraph($"Статус: {application.Status}");

                mainPart.Document.Save();
            }

            Console.WriteLine($"Документ успешно создан на рабочем столе: {filePath}");
        }

        public void ViewApplicationsWithDetails()
        {
            if (applications.Count > 0)
            {
                foreach (var app in applications)
                {
                    Console.WriteLine($"Заявление ID {app.Id}: На {app.Address} - Статус: {app.Status}");
                }

                Console.WriteLine("1. Просмотреть детали");
                Console.WriteLine("0. Назад");

                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    Console.Write("Введите ID заявления для просмотра деталей: ");
                    if (int.TryParse(Console.ReadLine(), out int appId))
                    {
                        var app = applications.Find(a => a.Id == appId);
                        if (app != null)
                        {
                            Console.WriteLine($"ID заявления: {app.Id}");
                            Console.WriteLine($"ФИО собственника: {app.OwnerName}");
                            Console.WriteLine($"Паспорт собственника: {app.OwnerPassport}");
                            Console.WriteLine($"Адрес учета: {app.Address}");
                            Console.WriteLine($"ФИО мигранта: {app.MigrantName}");
                            Console.WriteLine($"Паспорт мигранта: {app.MigrantPassport}");
                            Console.WriteLine($"Серия и номер визы: {app.VisaNumber}");
                            Console.WriteLine($"Серия и номер миграционной карты: {app.MigrationCard}");
                            Console.WriteLine($"Статус: {app.Status}");
                        }
                        else
                        {
                            Console.WriteLine("Заявление не найдено.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Некорректный ID заявления.");
                    }
                }
                else if (choice != "0")
                {
                    Console.WriteLine("Неверный выбор");
                }
            }
            else
            {
                Console.WriteLine("Заявлений не найдено.");
            }
        }

        public void ChangeStatus()
        {
            if (applications.Count > 0)
            {
                foreach (var app in applications)
                {
                    Console.WriteLine($"Заявление ID {app.Id}: На {app.Address} - Статус: {app.Status}");
                }

                Console.Write("Введите ID заявления для изменения статуса: ");
                if (int.TryParse(Console.ReadLine(), out int appId))
                {
                    var app = applications.Find(a => a.Id == appId);
                    if (app != null)
                    {
                        Console.WriteLine("Введите новый статус (Одобрено/Отклонено/На рассмотрении): ");
                        string newStatus = Console.ReadLine();
                        app.Status = newStatus;
                        Console.WriteLine("Статус успешно обновлен.");
                    }
                    else
                    {
                        Console.WriteLine("Заявление не найдено.");
                    }
                }
                else
                {
                    Console.WriteLine("Некорректный ID заявления.");
                }
            }
            else
            {
                Console.WriteLine("Заявлений не найдено.");
            }
        }
    }
}
