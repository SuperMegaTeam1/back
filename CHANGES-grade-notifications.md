# Изменения бэка: уведомления об оценках

Две задачи в одной ветке:
1. **Фикс:** уведомления об оценках не доходили до студента.
2. **Доработка:** в тексте уведомления — предмет и дата пары.

Контракт данных не менялся: сущность `Notification`, DTO ответа, JOIN, payload SignalR, миграции БД — **без изменений**. Фронтенд не трогался.

---

## 1. Фикс доставки — неправильный `ReceiverId`

**Причина:** в `Notification.ReceiverId` писался `Student.Id` (PK записи студента), а система адресует уведомления по `ParentUserId` (= `NameIdentifier` в JWT). Из-за этого SignalR не находил соединение, а `GET /notification` (фильтр по `ReceiverId == ParentUserId`) не возвращал запись.

### `Backend.Application/Services/JournalService.cs`
| Строка | Было | Стало |
|---|---|---|
| 65 | `ReceiverId = item.StudentId,` | `ReceiverId = student.ParentUserId,` |

`student` уже загружался выше (`_studentRepo.GetByIdAsync(item.StudentId)`).

### `Backend.Application/Services/GradeService.cs`
| Строка | Изменение |
|---|---|
| 18 | Добавлено поле `private readonly IStudentRepository _studentRepo;` |
| 20-26 | В конструктор добавлен параметр `IStudentRepository studentRepo` |
| 32 | Добавлено `_studentRepo = studentRepo;` |
| 46-47 | Добавлена загрузка студента: `var student = await _studentRepo.GetByIdAsync(studentGrade.StudentId) ?? throw new Exception("Студент не найден");` |
| 89 | `ReceiverId = studentId,` → `ReceiverId = student.ParentUserId,` |
| (удалено) | Убрана локальная переменная `var studentId = studentGrade?.StudentId ?? gradeId;` — больше не нужна |

---

## 2. Доработка текста — предмет и дата пары

Целевой формат:
- **Title:** `Оценка по {предмет}` (напр. «Оценка по Математике»)
- **Body:** `Вам поставили {балл} за пару {дата пары:dd.MM.yyyy}` (напр. «Вам поставили 7 за пару 02.06.2026»)
- Дата = `Lesson.StartsAt`.

### `Backend.Infrastructure/Repositories/GradeRepository.cs`
| Строки | Изменение |
|---|---|
| 39-45 | В `GetByIdAsync` добавлен `.Include(x => x.Lesson).ThenInclude(l => l.Subject)`, чтобы из `StudentGrade` были доступны предмет и дата пары. Метод используется только в `GradeService.UpdateGrade`. |

```csharp
return await _db.StudentGrades
    .Include(x => x.Lesson)
        .ThenInclude(l => l.Subject)
    .FirstOrDefaultAsync(x => x.Id == gradeId);
```

### `Backend.Application/Services/GradeService.cs`
| Строки | Изменение |
|---|---|
| 84 | Блок создания уведомления обёрнут в `if (grade.HasValue)` — при удалении оценки (`grade == null`) уведомление не отправляется (раньше уходило пустое «Вам поставили оценку »), плюс защита от NRE, т.к. `studentGrade` обнуляется в ветке удаления |
| 90 | `Title = $"Оценка по {studentGrade.Lesson.Subject.Name}"` (было `"Выставлена оценка"`) |
| 91 | `Body = $"Вам поставили {grade} за пару {studentGrade.Lesson.StartsAt:dd.MM.yyyy}"` (было `"Вам поставили оценку {grade}"`) |

### `Backend.Application/Services/JournalService.cs`
| Строки | Изменение |
|---|---|
| 20 | Добавлено поле `private readonly ISubjectRepository _subjectRepo;` |
| 22-29 | В конструктор добавлен параметр `ISubjectRepository subjectRepo` |
| 37 | Добавлено `_subjectRepo = subjectRepo;` |
| 61 | Добавлено получение имени предмета: `var subjectName = await _subjectRepo.GetNameByIdAsync(lesson.SubjectId);` (внутри блока `if (item.Grade != null)`) |
| 66 | `Title = $"Оценка по {subjectName}"` (было `"Выставлена оценка"`) |
| 67 | `Body = $"Вам поставили {item.Grade} за пару {lesson.StartsAt:dd.MM.yyyy}"` (было `"Вам поставили оценку {item.Grade}"`) |

---

## 3. Тесты

### `Backend.Tests/Unit/Services/GradeServiceTests.cs`
- Добавлен мок `Mock<IStudentRepository> _studentRepo` (поле, инициализация, дефолтный `Setup(GetByIdAsync)` возвращает `Student` с `ParentUserId`); передан 5-м аргументом в `CreateService()`.
- В тестах с `grade: 5` (`UpdateGrade_ShouldReturnUpdatedGrade`, `UpdateGrade_ShouldSendNotification`) у `StudentGrade` заполнена навигация `Lesson { StartsAt, Subject { Name } }` — иначе NRE при формировании текста.

### `Backend.Tests/Unit/Services/JournalServiceTests.cs`
- Добавлен мок `Mock<ISubjectRepository> _subjectRepo` (поле, инициализация); передан 7-м аргументом в конструктор `JournalService`.
- В `UpdateJournal_ShouldReturnUpdatedJournal` добавлены настройки `_lessonRepo.GetByIdAsync(lessonId)` (возвращает `Lesson` с `SubjectId` и `StartsAt`) и `_subjectRepo.GetNameByIdAsync(subjectId)` (возвращает «Математика»).

---

## DI
Новые зависимости (`IStudentRepository` в `GradeService`, `ISubjectRepository` в `JournalService`) уже зарегистрированы в контейнере — менять регистрацию не требуется.

## Проверка
- `dotnet build backend/Back.sln` — 0 ошибок.
- `dotnet test backend/Back.sln` — 44/44 пройдены.

## Не относится к этим изменениям
Ошибка при локальном запуске `42P07: отношение "AspNetRoles" уже существует` — рассинхрон локальной dev-БД (таблицы есть, миграция не отмечена в `__EFMigrationsHistory`), к коду не относится; миграции не менялись.
