W niniejszym zadaniu skorzystamy z biblioteki EntityFramework Core. Poniżej przedstawiony jest diagram na którym będziemy pracować.  
![image](https://user-images.githubusercontent.com/35382676/167697816-76520842-a184-4130-a176-578b23bb224c.png)
1. Stwórz aplikację typu REST Api.
2. Przygotuj końcówkę zwracającą dane z pomocą EntityFramework Core zgodnie z poniższymi informacjami:
   - Końcówkę odpowiadającą na żądania ```HTTP GET``` wysyłane na adres ```/api/trips```
   - Końcówka powinna zwrócić listę podróży w kolejności posortowanej malejącą po dacie rozpoczęcia wycieczki.
   - Poniżej przedstawiony przykładowy format zwróconych danych.  
![image](https://user-images.githubusercontent.com/35382676/167698007-0ca92d62-a37c-4deb-81eb-15c6213b091a.png)
3. Przygotuj końcówkę pozwalającą na usunięcie danych klienta.
   - Końcówka przyjmująca dane wysłane na adres ```HTTP DELETE``` na adres ```/api/clients/{idClient}```
   - Końcówka powinna najpierw sprawdzić czy klient nie posiada żadnych przypisanych wycieczek. Jeśli klient posiada co najmniej jedną przypisaną wycieczkę – zwracamy błąd i usunięcie nie dochodzi do skutku.
4. Przygotuj końcówkę pozwalającą na przypisanie klienta do wycieczki.
   - Końcówka powinna przyjmować żądania ```HTTP POST``` wysłane na adres ```/api/trips/{idTrip}/clients```
   - Parametry przesłane w ciele żądania powinna wyglądać następująco (format danych może być inny):  
![image](https://user-images.githubusercontent.com/35382676/167698112-083649ef-9479-4d25-9af9-78d5c7e5d1b1.png)
   - Po przyjęciu danych sprawdzamy:
     - Czy klient o danym numerze PESEL istnieje. Jeśli nie, dodajemy go do bazy danych.
     - Czy klient nie jest już zapisaną na wspomnianą wycieczkę – w takim wypadku zwracamy błąd
     - Czy wycieczka istnieje – jeśli nie – zwracamy błąd
   - „PaymentDate” może posiadać wartość null, dla tych klientów, którzy jeszcze nie zapłacili za podróż. Ponadto kolumna „RegisteredAt” w tabeli Client_Trip powinna być równa aktualnemu czasowi przetworzenia żądania.
5. Pamiętaj o poprawnych nazwach zmiennych/metod/klas
6. Wykorzystaj dodatkowe modele dla danych zwracanych i przyjmowanych przez końcówki – DTO (ang. Data Transfer Object)
7. Pamiętaj o SOLID, DI
