usecase1: авторизация в систему. "Войти в систему"
![image](https://github.com/user-attachments/assets/4c67a28a-2ad2-4cec-9f2c-0417f17e5e40)

Описание: Пользователь авторизуется с помощью Email и пароль и входит в систему. Затем получает доступ к функционалу исходя из его роли. 
Собственник может подать заявление для постановки мигранта на учет, получить отрывную часть уведомления о прибытии. 
Мигрант может узнать результат по заявлению. 
Сотрудник МВД может просмотреть заявление, принять решение по заявлению, отправить уведомление о решении. 
Админ может добавить\удалить пользователя и выдать права доступа.


usecase2: удаление пользователя админом. "Удалить пользователя"
![image](https://github.com/user-attachments/assets/764b326d-4a71-4302-b447-e43daea5b119)

Описание: Админ выбирает "просмотреть всех пользователей". Из списка выбирает необходимого пользователя на удаление, удаляет его и получает сообщение об успешности операции.

usecase3: создание пользователя админом. "Создать пользователя"
![image](https://github.com/user-attachments/assets/e88f36f2-0cf2-489d-accc-a0a4fc55fc79)

Описание: Админ выбирает "Создать пользователя", в соответствующие поля вводит ФИО, email и пароль, выбирает из списка необходимую роль, подтверждает действие. 
Созданный пользователь добавляется в систему и выводится сообщение об успехе.

usecase4: Админ выдаёт роль. "Выдать роль"
![image](https://github.com/user-attachments/assets/fe4c4ba6-4904-47b7-b34b-9646d9daae02)

Описание: Админ выбирает "Выдать роль", в поле вводит Email необходимого пользователя, из списка выбирает роль, подтверждает действие и получает сообщение об успешности операции.

usecase5: Сотрудник МВД просматривает заявление. "Просмотреть заявление".
![image](https://github.com/user-attachments/assets/d2b7836d-b48d-4a9c-be17-ec0ef406680b)

Описание: Сотрудник МВД выбирает "Просмотреть заявление". Если заявления есть, выаодится последнее заявление, иначе выводится сообщение об отсутствие заявлений