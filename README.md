# Loan API — ფინალური პროექტი

ეს არის **.NET 9**-ზე აგებული Web API, რომელიც იყენებს **სუფთა არქიტექტურას (Clean Architecture)**, **JWT ავტორიზაციას** და **როლებზე დაფუძნებულ წვდომებს (Role-Based Authorization)** (`Accountant` და `User`).

აპლიკაცია უზრუნველყოფს მომხმარებლების მართვას, სესხის განაცხადების მიღებას, სტატუსების ცვლილებასა და ადმინისტრაციულ ფუნქციონალს.

---

## პროექტის სტრუქტურა

| პროექტი | აღწერა |
|---|---|
| **FinalProject** (Presentation / Web API) | შეიცავს კონტროლერებს (`AuthController`, `UsersController`, `LoansController`), შუალედურ პროგრამებს (`ExceptionHandlingMiddleware`) და ვალიდაციის ფილტრებს. |
| **FinalProject.Application** | მოიცავს ბიზნეს ლოგიკის ინტერფეისებს, DTO-ებს (`AuthDtos`, `LoanDtos`, `UserDtos`), FluentValidation წესებსა და სერვისების იმპლემენტაციას (`UserService`, `LoanService`). |
| **FinalProject.Infrastructure** | პასუხისმგებელია მონაცემთა ბაზასთან კავშირზე Entity Framework Core-ის საშუალებით (`AppDbContext`), მიგრაციებსა და რეპოზიტორიებზე (`UserRepository`, `LoanRepository`, `AccountantRepository`). |
| **FinalProject.Domain** | შეიცავს ძირითად ობიექტებს (`User`, `Accountant`, `Loan`) და ენუმერაციებს (`LoanStatus`). |
| **FinalProject.Tests** | იუნიტ ტესტები, დაწერილი xUnit-ისა და Moq-ის გამოყენებით, სერვისების ლოგიკის შესამოწმებლად. |

---

##  ბიზნეს წესები და უფლებები

### როლები

- **User** — სტანდარტული მომხმარებელი (სესხის მაძიებელი).
- **Accountant** — ოპერატორი/ბუღალტერი სრული ადმინისტრაციული უფლებებით.

### ძირითადი წესები

- **რეგისტრაცია და ავტორიზაცია** — პაროლები ბაზაში ინახება დაჰეშილი სახით (SHA-256).
- **სესხის დამატება** — ავტორიზებულ მომხმარებელს შეუძლია სესხის მოთხოვნა. საწყისი სტატუსია `InProcess` (დამუშავების პროცესში).
- **დაბლოკილი მომხმარებლები** — თუ მომხმარებელი დაბლოკილია (`IsBlocked = true`), მას ეზღუდება ახალი სესხის მოთხოვნის უფლება.
- **მომხმარებლის უფლებები** — მომხმარებელს შეუძლია ნახოს, განაახლოს და წაშალოს მხოლოდ საკუთარი სესხები და მხოლოდ მაშინ, როცა სტატუსია `InProcess`.
- **ოპერატორის უფლებები** — ოპერატორს შეუძლია ნებისმიერი მომხმარებლის სესხის ნახვა, რედაქტირება, წაშლა და სტატუსის ცვლილება (`InProcess`, `Approved`, `Rejected`), ასევე მომხმარებლის დაბლოკვა/განბლოკვა.

---

##  API ენდპოინტები

### ავტორიზაცია — `/api/auth`

| მეთოდი | ენდპოინტი | წვდომა | აღწერა |
|---|---|---|---|
| `POST` | `/api/auth/register` | ყველასთვის | ახალი მომხმარებლის რეგისტრაცია |
| `POST` | `/api/auth/login/user` | ყველასთვის | მომხმარებლის ავტორიზაცია და JWT ტოკენის მიღება |
| `POST` | `/api/auth/login/accountant` | ყველასთვის | ოპერატორის ავტორიზაცია და JWT ტოკენის მიღება |

### მომხმარებლები — `/api/users`

| მეთოდი | ენდპოინტი | წვდომა | აღწერა |
|---|---|---|---|
| `GET` | `/api/users/{id}` | ავტორიზებული | მომხმარებლის ინფორმაციის წამოღება ID-ის მიხედვით |
| `PATCH` | `/api/users/{id}/block` | Accountant | მომხმარებლის დაბლოკვა ან განბლოკვა |

### სესხები — `/api/loans`

| მეთოდი | ენდპოინტი | წვდომა | აღწერა |
|---|---|---|---|
| `POST` | `/api/loans` | User | ახალი სესხის განაცხადის შექმნა |
| `GET` | `/api/loans/my-loans` | User | ავტორიზებული მომხმარებლის სესხების სიის ნახვა |
| `PUT` | `/api/loans/{id}` | User | საკუთარი სესხის ცვლილება (მხოლოდ `InProcess` სტატუსზე) |
| `DELETE` | `/api/loans/{id}` | User | საკუთარი სესხის წაშლა (მხოლოდ `InProcess` სტატუსზე) |
| `GET` | `/api/loans/all` | Accountant | ბაზაში არსებული ყველა სესხის ნახვა |
| `PUT` | `/api/loans/accountant/{id}` | Accountant | ნებისმიერი სესხის მონაცემების ცვლილება |
| `DELETE` | `/api/loans/accountant/{id}` | Accountant | ნებისმიერი სესხის წაშლა |
| `PATCH` | `/api/loans/{id}/status` | Accountant | სესხის სტატუსის შეცვლა (`Approved` / `Rejected`) |

---

## 🛠 პროექტის გაშვების ინსტრუქცია

### წინაპირობები

- დაინსტალირებული **.NET 9.0 SDK**
- **SQL Server** ან **LocalDB**

### ნაბიჯები გაშვებისთვის

**1. პროექტის კლონირება / გახსნა**

```bash
cd FinalProject
```

**2. მონაცემთა ბაზის კონფიგურაცია**

შეამოწმეთ `FinalProject/appsettings.json` ფაილში არსებული `ConnectionStrings` და მოარგეთ თქვენს local SQL Server-ს:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=LoanDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

**3. მიგრაციების გაშვება (Database Update)**

გაუშვით ბაზის შექმნის ბრძანება ტერმინალიდან:

```bash
dotnet ef database update --project FinalProject.Infrastructure --startup-project FinalProject
```

**4. აპლიკაციის გაშვება**

```bash
dotnet run --project FinalProject
```

**5. Swagger UI (OpenAPI)**

გახსენით ბრაუზერში `https://localhost:7000/swagger` (ან ტერმინალში გამოჩენილი პორტი) ენდპოინტების დასატესტად.

---

##  იუნიტ ტესტების გაშვება

ტესტები ფარავს `UserService`-ისა და `LoanService`-ის ბიზნეს ლოგიკას.

ტესტების გაშვება ტერმინალიდან:

```bash
dotnet test
```

ან IDE-ს (Rider / Visual Studio) **Unit Tests** ფანჯრიდან.
