# ScheduleAPI

API для работы с расписанием событий
- Регистрация - /account/register 
    - Method=POST 
    - Body={"login":string,"password":string,"confirmpassword":string} 
    - Return=Status 200

- Получение токена - /account/gettoken 
    - Method=POST 
    - Body={"login":string,"password":string} 
    - Return={"token":string, "username":string}

- Получение списка событий - /api/events 
    - Method=GET 
    - Headers={"Authentication": "Bearer "+token} 
    - Return=[{"id":int, "eventdate":string("yyyy-mm-ddThh:mm:ss"), "eventdescription":string}]

- Получение события - /api/events/{id} 
    - Method=GET 
    - Headers={"Authentication": "Bearer "+token} 
    - Return={"id":int, "eventdate":string("yyyy-mm-ddThh:mm:ss"), "eventdescription":string}

- Получение списка событий на определенный день - /api/events/bydate?date=string("yyyy-mm-dd") 
    - Method=GET 
    - Headers={"Authentication": "Bearer "+token} 
    - Return=[{"id":int, "eventdate":string("yyyy-mm-ddThh:mm:ss"), "eventdescription":string}]

- Добавление события - /api/events 
    - Method=POST 
    - Headers={"Authentication": "Bearer "+token} 
    - Body={"eventdate":string("yyyy-mm-ddThh:mm:ss"), "eventdescription":string} 
    - Return={"eventdate":string("yyyy-mm-ddThh:mm:ss"), "eventdescription":string}

- Редактирование события - /api/events/ 
    - Method=PUT 
    - Headers={"Authentication": "Bearer "+token} 
    - Body={"id":int "eventdate":string("yyyy-mm-ddThh:mm:ss"), "eventdescription":string} 
    - Return={"eventdate":string("yyyy-mm-ddThh:mm:ss"), "eventdescription":string}

- Удаление события - /api/events/ 
    - Method=DELETE 
    - Headers={"Authentication": "Bearer "+token} 
    - Return={"eventdate":string("yyyy-mm-ddThh:mm:ss"), "eventdescription":string}
