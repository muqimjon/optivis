using System.Globalization;

namespace OptiVis.UI.Shared.i18n;

public enum AppLanguage
{
    UzbekLatin,
    UzbekCyrillic,
    Russian,
    English
}

public static class Translations
{
    private static AppLanguage _currentLanguage = AppLanguage.UzbekLatin;

    public static AppLanguage CurrentLanguage
    {
        get => _currentLanguage;
        set
        {
            if (_currentLanguage == value)
            {
                return;
            }
            _currentLanguage = value;
            OnLanguageChanged?.Invoke();
        }
    }

    public static void SetLanguageWithoutNotify(AppLanguage language)
    {
        _currentLanguage = language;
    }

    public static event Action? OnLanguageChanged;

    private static readonly Dictionary<string, Dictionary<AppLanguage, string>> _translations = new()
    {
        ["Dashboard"] = new()
        {
            [AppLanguage.UzbekLatin] = "Bosh sahifa",
            [AppLanguage.UzbekCyrillic] = "Бош саҳифа",
            [AppLanguage.Russian] = "Главная",
            [AppLanguage.English] = "Dashboard"
        },
        ["Operators"] = new()
        {
            [AppLanguage.UzbekLatin] = "Operatorlar",
            [AppLanguage.UzbekCyrillic] = "Операторлар",
            [AppLanguage.Russian] = "Операторы",
            [AppLanguage.English] = "Operators"
        },
        ["Search"] = new()
        {
            [AppLanguage.UzbekLatin] = "Qidiruv",
            [AppLanguage.UzbekCyrillic] = "Қидирув",
            [AppLanguage.Russian] = "Поиск",
            [AppLanguage.English] = "Search"
        },
        ["Logs"] = new()
        {
            [AppLanguage.UzbekLatin] = "Loglar",
            [AppLanguage.UzbekCyrillic] = "Логлар",
            [AppLanguage.Russian] = "Логи",
            [AppLanguage.English] = "Logs"
        },
        ["Settings"] = new()
        {
            [AppLanguage.UzbekLatin] = "Sozlamalar",
            [AppLanguage.UzbekCyrillic] = "Созламалар",
            [AppLanguage.Russian] = "Настройки",
            [AppLanguage.English] = "Settings"
        },
        ["TotalCalls"] = new()
        {
            [AppLanguage.UzbekLatin] = "Jami qo'ng'iroqlar",
            [AppLanguage.UzbekCyrillic] = "Жами қўнғироқлар",
            [AppLanguage.Russian] = "Всего звонков",
            [AppLanguage.English] = "Total Calls"
        },
        ["Success"] = new()
        {
            [AppLanguage.UzbekLatin] = "Muvaffaqiyatli",
            [AppLanguage.UzbekCyrillic] = "Муваффақиятли",
            [AppLanguage.Russian] = "Успешных",
            [AppLanguage.English] = "Success"
        },
        ["Cancelled"] = new()
        {
            [AppLanguage.UzbekLatin] = "Bekor qilingan",
            [AppLanguage.UzbekCyrillic] = "Бекор қилинган",
            [AppLanguage.Russian] = "Отменённых",
            [AppLanguage.English] = "Cancelled"
        },
        ["AvgWait"] = new()
        {
            [AppLanguage.UzbekLatin] = "O'rtacha kutish",
            [AppLanguage.UzbekCyrillic] = "Ўртача кутиш",
            [AppLanguage.Russian] = "Среднее ожидание",
            [AppLanguage.English] = "Avg Wait"
        },
        ["AvgDuration"] = new()
        {
            [AppLanguage.UzbekLatin] = "O'rtacha davomiylik",
            [AppLanguage.UzbekCyrillic] = "Ўртача давомийлик",
            [AppLanguage.Russian] = "Средняя длительность",
            [AppLanguage.English] = "Avg Duration"
        },
        ["CallVolume"] = new()
        {
            [AppLanguage.UzbekLatin] = "Soatlik qo'ng'iroqlar",
            [AppLanguage.UzbekCyrillic] = "Соатлик қўнғироқлар",
            [AppLanguage.Russian] = "Звонки по часам",
            [AppLanguage.English] = "Call Volume per Hour"
        },
        ["StatusDistribution"] = new()
        {
            [AppLanguage.UzbekLatin] = "Holat taqsimoti",
            [AppLanguage.UzbekCyrillic] = "Ҳолат тақсимоти",
            [AppLanguage.Russian] = "Распределение статусов",
            [AppLanguage.English] = "Status Distribution"
        },
        ["ActiveOperators"] = new()
        {
            [AppLanguage.UzbekLatin] = "Faol operatorlar",
            [AppLanguage.UzbekCyrillic] = "Фаол операторлар",
            [AppLanguage.Russian] = "Активные операторы",
            [AppLanguage.English] = "Active Operators"
        },
        ["Inbound"] = new()
        {
            [AppLanguage.UzbekLatin] = "Kiruvchi",
            [AppLanguage.UzbekCyrillic] = "Кирувчи",
            [AppLanguage.Russian] = "Входящие",
            [AppLanguage.English] = "Inbound"
        },
        ["Outbound"] = new()
        {
            [AppLanguage.UzbekLatin] = "Chiquvchi",
            [AppLanguage.UzbekCyrillic] = "Чиқувчи",
            [AppLanguage.Russian] = "Исходящие",
            [AppLanguage.English] = "Outbound"
        },
        ["Today"] = new()
        {
            [AppLanguage.UzbekLatin] = "Bugun",
            [AppLanguage.UzbekCyrillic] = "Бугун",
            [AppLanguage.Russian] = "Сегодня",
            [AppLanguage.English] = "Today"
        },
        ["Operator"] = new()
        {
            [AppLanguage.UzbekLatin] = "Operator",
            [AppLanguage.UzbekCyrillic] = "Оператор",
            [AppLanguage.Russian] = "Оператор",
            [AppLanguage.English] = "Operator"
        },
        ["Status"] = new()
        {
            [AppLanguage.UzbekLatin] = "Holat",
            [AppLanguage.UzbekCyrillic] = "Ҳолат",
            [AppLanguage.Russian] = "Статус",
            [AppLanguage.English] = "Status"
        },
        ["SuccessRate"] = new()
        {
            [AppLanguage.UzbekLatin] = "Muvaffaqiyat %",
            [AppLanguage.UzbekCyrillic] = "Муваффақият %",
            [AppLanguage.Russian] = "Успешность %",
            [AppLanguage.English] = "Success Rate"
        },
        ["TalkTime"] = new()
        {
            [AppLanguage.UzbekLatin] = "Suhbat vaqti",
            [AppLanguage.UzbekCyrillic] = "Суҳбат вақти",
            [AppLanguage.Russian] = "Время разговора",
            [AppLanguage.English] = "Talk Time"
        },
        ["Calls"] = new()
        {
            [AppLanguage.UzbekLatin] = "Qo'ng'iroqlar",
            [AppLanguage.UzbekCyrillic] = "Қўнғироқлар",
            [AppLanguage.Russian] = "Звонки",
            [AppLanguage.English] = "Calls"
        },
        ["EnterPhoneNumber"] = new()
        {
            [AppLanguage.UzbekLatin] = "Telefon raqamini kiriting...",
            [AppLanguage.UzbekCyrillic] = "Телефон рақамини киритинг...",
            [AppLanguage.Russian] = "Введите номер телефона...",
            [AppLanguage.English] = "Enter phone number..."
        },
        ["DateTime"] = new()
        {
            [AppLanguage.UzbekLatin] = "Sana/Vaqt",
            [AppLanguage.UzbekCyrillic] = "Сана/Вақт",
            [AppLanguage.Russian] = "Дата/Время",
            [AppLanguage.English] = "Date/Time"
        },
        ["Connected"] = new()
        {
            [AppLanguage.UzbekLatin] = "Bog'langan",
            [AppLanguage.UzbekCyrillic] = "Боғланган",
            [AppLanguage.Russian] = "Подключено",
            [AppLanguage.English] = "Connected"
        },
        ["About"] = new()
        {
            [AppLanguage.UzbekLatin] = "Dastur haqida",
            [AppLanguage.UzbekCyrillic] = "Дастур ҳақида",
            [AppLanguage.Russian] = "О программе",
            [AppLanguage.English] = "About"
        },
        ["AppDescription"] = new()
        {
            [AppLanguage.UzbekLatin] = "Call center tahlil qilish tizimi",
            [AppLanguage.UzbekCyrillic] = "Call center таҳлил қилиш тизими",
            [AppLanguage.Russian] = "Система аналитики колл-центра",
            [AppLanguage.English] = "Call center analytics dashboard"
        },
        ["Duration"] = new()
        {
            [AppLanguage.UzbekLatin] = "Davomiyligi",
            [AppLanguage.UzbekCyrillic] = "Давомийлиги",
            [AppLanguage.Russian] = "Длительность",
            [AppLanguage.English] = "Duration"
        },
        ["Language"] = new()
        {
            [AppLanguage.UzbekLatin] = "Til",
            [AppLanguage.UzbekCyrillic] = "Тил",
            [AppLanguage.Russian] = "Язык",
            [AppLanguage.English] = "Language"
        },
        ["Theme"] = new()
        {
            [AppLanguage.UzbekLatin] = "Mavzu",
            [AppLanguage.UzbekCyrillic] = "Мавзу",
            [AppLanguage.Russian] = "Тема",
            [AppLanguage.English] = "Theme"
        },
        ["Light"] = new()
        {
            [AppLanguage.UzbekLatin] = "Yorug'",
            [AppLanguage.UzbekCyrillic] = "Ёруғ",
            [AppLanguage.Russian] = "Светлая",
            [AppLanguage.English] = "Light"
        },
        ["Dark"] = new()
        {
            [AppLanguage.UzbekLatin] = "Qorong'u",
            [AppLanguage.UzbekCyrillic] = "Қоронғу",
            [AppLanguage.Russian] = "Тёмная",
            [AppLanguage.English] = "Dark"
        },
        ["Answered"] = new()
        {
            [AppLanguage.UzbekLatin] = "Javob berilgan",
            [AppLanguage.UzbekCyrillic] = "Жавоб берилган",
            [AppLanguage.Russian] = "Отвечено",
            [AppLanguage.English] = "Answered"
        },
        ["NoAnswer"] = new()
        {
            [AppLanguage.UzbekLatin] = "Javob berilmagan",
            [AppLanguage.UzbekCyrillic] = "Жавоб берилмаган",
            [AppLanguage.Russian] = "Без ответа",
            [AppLanguage.English] = "No Answer"
        },
        ["Busy"] = new()
        {
            [AppLanguage.UzbekLatin] = "Band",
            [AppLanguage.UzbekCyrillic] = "Банд",
            [AppLanguage.Russian] = "Занято",
            [AppLanguage.English] = "Busy"
        },
        ["Online"] = new()
        {
            [AppLanguage.UzbekLatin] = "Onlayn",
            [AppLanguage.UzbekCyrillic] = "Онлайн",
            [AppLanguage.Russian] = "Онлайн",
            [AppLanguage.English] = "Online"
        },
        ["Offline"] = new()
        {
            [AppLanguage.UzbekLatin] = "Oflayn",
            [AppLanguage.UzbekCyrillic] = "Офлайн",
            [AppLanguage.Russian] = "Оффлайн",
            [AppLanguage.English] = "Offline"
        },
        ["All"] = new()
        {
            [AppLanguage.UzbekLatin] = "Hammasi",
            [AppLanguage.UzbekCyrillic] = "Ҳаммаси",
            [AppLanguage.Russian] = "Все",
            [AppLanguage.English] = "All"
        },
        ["Total"] = new()
        {
            [AppLanguage.UzbekLatin] = "Jami",
            [AppLanguage.UzbekCyrillic] = "Жами",
            [AppLanguage.Russian] = "Всего",
            [AppLanguage.English] = "Total"
        },
        ["Completed"] = new()
        {
            [AppLanguage.UzbekLatin] = "Yakunlangan",
            [AppLanguage.UzbekCyrillic] = "Якунланган",
            [AppLanguage.Russian] = "Завершён",
            [AppLanguage.English] = "Completed"
        },
        ["Available"] = new()
        {
            [AppLanguage.UzbekLatin] = "Tayyor",
            [AppLanguage.UzbekCyrillic] = "Тайёр",
            [AppLanguage.Russian] = "Доступен",
            [AppLanguage.English] = "Available"
        },
        ["Other"] = new()
        {
            [AppLanguage.UzbekLatin] = "Boshqa",
            [AppLanguage.UzbekCyrillic] = "Бошқа",
            [AppLanguage.Russian] = "Другое",
            [AppLanguage.English] = "Other"
        },
        ["PrecisionDashboard"] = new()
        {
            [AppLanguage.UzbekLatin] = "Aniq monitoring",
            [AppLanguage.UzbekCyrillic] = "Аниқ мониторинг",
            [AppLanguage.Russian] = "Точный мониторинг",
            [AppLanguage.English] = "Precision Dashboard"
        },
        ["RealTimeMetrics"] = new()
        {
            [AppLanguage.UzbekLatin] = "Real vaqt ko'rsatkichlari va operator holati",
            [AppLanguage.UzbekCyrillic] = "Реал вақт кўрсаткичлари ва оператор ҳолати",
            [AppLanguage.Russian] = "Метрики реального времени и статус операторов",
            [AppLanguage.English] = "Real-time performance metrics & operator status"
        },
        ["ExportCSV"] = new()
        {
            [AppLanguage.UzbekLatin] = "CSV yuklash",
            [AppLanguage.UzbekCyrillic] = "CSV юклаш",
            [AppLanguage.Russian] = "Экспорт CSV",
            [AppLanguage.English] = "Export CSV"
        },
        ["ViewAllTeams"] = new()
        {
            [AppLanguage.UzbekLatin] = "Barcha jamoalarni ko'rish",
            [AppLanguage.UzbekCyrillic] = "Барча жамоаларни кўриш",
            [AppLanguage.Russian] = "Все команды",
            [AppLanguage.English] = "View All Teams"
        },
        ["EfficiencyComparison"] = new()
        {
            [AppLanguage.UzbekLatin] = "Samaradorlik taqqoslash",
            [AppLanguage.UzbekCyrillic] = "Самарадорлик таққослаш",
            [AppLanguage.Russian] = "Сравнение эффективности",
            [AppLanguage.English] = "Efficiency Comparison"
        },
        ["WeeklyProgress"] = new()
        {
            [AppLanguage.UzbekLatin] = "Haftalik jamoalar natijalari",
            [AppLanguage.UzbekCyrillic] = "Ҳафталик жамоалар натижалари",
            [AppLanguage.Russian] = "Еженедельный прогресс команд",
            [AppLanguage.English] = "Weekly target progress by team"
        },
        ["Connected"] = new()
        {
            [AppLanguage.UzbekLatin] = "Ulangan",
            [AppLanguage.UzbekCyrillic] = "Уланган",
            [AppLanguage.Russian] = "Подключен",
            [AppLanguage.English] = "Connected"
        },
        ["Disconnected"] = new()
        {
            [AppLanguage.UzbekLatin] = "Uzilgan",
            [AppLanguage.UzbekCyrillic] = "Узилган",
            [AppLanguage.Russian] = "Отключен",
            [AppLanguage.English] = "Disconnected"
        },
        ["ManagementTerminal"] = new()
        {
            [AppLanguage.UzbekLatin] = "BOSHQARUV TERMINALI",
            [AppLanguage.UzbekCyrillic] = "БОШҚАРУВ ТЕРМИНАЛИ",
            [AppLanguage.Russian] = "ТЕРМИНАЛ УПРАВЛЕНИЯ",
            [AppLanguage.English] = "MANAGEMENT TERMINAL"
        },
        ["TopPerformers"] = new()
        {
            [AppLanguage.UzbekLatin] = "Eng yaxshilar",
            [AppLanguage.UzbekCyrillic] = "Энг яхшилар",
            [AppLanguage.Russian] = "Лучшие",
            [AppLanguage.English] = "Top Performers"
        },
        ["LiveFleetStats"] = new()
        {
            [AppLanguage.UzbekLatin] = "Jonli statistika",
            [AppLanguage.UzbekCyrillic] = "Жонли статистика",
            [AppLanguage.Russian] = "Живая статистика",
            [AppLanguage.English] = "Live Fleet Stats"
        },
        ["Active"] = new()
        {
            [AppLanguage.UzbekLatin] = "FAOL",
            [AppLanguage.UzbekCyrillic] = "ФАОЛ",
            [AppLanguage.Russian] = "АКТИВНЫХ",
            [AppLanguage.English] = "ACTIVE"
        },
        ["AvgTalk"] = new()
        {
            [AppLanguage.UzbekLatin] = "O'RT. SUHBAT",
            [AppLanguage.UzbekCyrillic] = "ЎРТ. СУҲБАТ",
            [AppLanguage.Russian] = "СР. РАЗГОВОР",
            [AppLanguage.English] = "AVG TALK"
        },
        ["OperatorRoster"] = new()
        {
            [AppLanguage.UzbekLatin] = "Operatorlar ro'yxati",
            [AppLanguage.UzbekCyrillic] = "Операторлар рўйхати",
            [AppLanguage.Russian] = "Список операторов",
            [AppLanguage.English] = "Operator Roster"
        },
        ["View"] = new()
        {
            [AppLanguage.UzbekLatin] = "Ko'rish",
            [AppLanguage.UzbekCyrillic] = "Кўриш",
            [AppLanguage.Russian] = "Смотреть",
            [AppLanguage.English] = "View"
        },
        ["RecentCalls"] = new()
        {
            [AppLanguage.UzbekLatin] = "So'nggi qo'ng'iroqlar",
            [AppLanguage.UzbekCyrillic] = "Сўнгги қўнғироқлар",
            [AppLanguage.Russian] = "Последние звонки",
            [AppLanguage.English] = "Recent Calls"
        },
        ["From"] = new()
        {
            [AppLanguage.UzbekLatin] = "Dan",
            [AppLanguage.UzbekCyrillic] = "Дан",
            [AppLanguage.Russian] = "От",
            [AppLanguage.English] = "From"
        },
        ["To"] = new()
        {
            [AppLanguage.UzbekLatin] = "Gacha",
            [AppLanguage.UzbekCyrillic] = "Гача",
            [AppLanguage.Russian] = "До",
            [AppLanguage.English] = "To"
        },
        ["Time"] = new()
        {
            [AppLanguage.UzbekLatin] = "Vaqt",
            [AppLanguage.UzbekCyrillic] = "Вақт",
            [AppLanguage.Russian] = "Время",
            [AppLanguage.English] = "Time"
        },
        ["SearchHistory"] = new()
        {
            [AppLanguage.UzbekLatin] = "TARIXI QIDIRISH",
            [AppLanguage.UzbekCyrillic] = "ТАРИХИ ҚИДИРИШ",
            [AppLanguage.Russian] = "ПОИСК В ИСТОРИИ",
            [AppLanguage.English] = "SEARCH HISTORY"
        },
        ["Filters"] = new()
        {
            [AppLanguage.UzbekLatin] = "Filtrlar",
            [AppLanguage.UzbekCyrillic] = "Филтрлар",
            [AppLanguage.Russian] = "Фильтры",
            [AppLanguage.English] = "Filters"
        },
        ["StartDate"] = new()
        {
            [AppLanguage.UzbekLatin] = "Boshlanish sanasi",
            [AppLanguage.UzbekCyrillic] = "Бошланиш санаси",
            [AppLanguage.Russian] = "Дата начала",
            [AppLanguage.English] = "Start Date"
        },
        ["EndDate"] = new()
        {
            [AppLanguage.UzbekLatin] = "Tugash sanasi",
            [AppLanguage.UzbekCyrillic] = "Тугаш санаси",
            [AppLanguage.Russian] = "Дата окончания",
            [AppLanguage.English] = "End Date"
        },
        ["Appearance"] = new()
        {
            [AppLanguage.UzbekLatin] = "Tashqi ko'rinish",
            [AppLanguage.UzbekCyrillic] = "Ташқи кўриниш",
            [AppLanguage.Russian] = "Внешний вид",
            [AppLanguage.English] = "Appearance"
        },
        ["CallTrends"] = new()
        {
            [AppLanguage.UzbekLatin] = "Qo'ng'iroqlar dinamikasi",
            [AppLanguage.UzbekCyrillic] = "Қўнғироқлар динамикаси",
            [AppLanguage.Russian] = "Динамика звонков",
            [AppLanguage.English] = "Call Trends"
        },
        ["CallLogs"] = new()
        {
            [AppLanguage.UzbekLatin] = "Qo'ng'iroqlar jurnali",
            [AppLanguage.UzbekCyrillic] = "Қўнғироқлар журнали",
            [AppLanguage.Russian] = "Журнал звонков",
            [AppLanguage.English] = "Call Logs"
        },
        ["Sequence"] = new()
        {
            [AppLanguage.UzbekLatin] = "T/R",
            [AppLanguage.UzbekCyrillic] = "Т/Р",
            [AppLanguage.Russian] = "№",
            [AppLanguage.English] = "SEQ"
        },
        ["Direction"] = new()
        {
            [AppLanguage.UzbekLatin] = "Yo'nalish",
            [AppLanguage.UzbekCyrillic] = "Йўналиш",
            [AppLanguage.Russian] = "Напр.",
            [AppLanguage.English] = "DIR"
        },
        ["Wait"] = new()
        {
            [AppLanguage.UzbekLatin] = "Kutish (s)",
            [AppLanguage.UzbekCyrillic] = "Кутиш (с)",
            [AppLanguage.Russian] = "Ожидание (с)",
            [AppLanguage.English] = "WAIT (s)"
        },
        ["Bill"] = new()
        {
            [AppLanguage.UzbekLatin] = "Hisob (s)",
            [AppLanguage.UzbekCyrillic] = "Ҳисоб (с)",
            [AppLanguage.Russian] = "Счет (с)",
            [AppLanguage.English] = "BILL (s)"
        },
        ["DurationSec"] = new()
        {
            [AppLanguage.UzbekLatin] = "Davomiylik (s)",
            [AppLanguage.UzbekCyrillic] = "Давомийлик (с)",
            [AppLanguage.Russian] = "Длительность (с)",
            [AppLanguage.English] = "DUR (s)"
        },
        ["Extension"] = new()
        {
            [AppLanguage.UzbekLatin] = "Ichki raqam",
            [AppLanguage.UzbekCyrillic] = "Ички рақам",
            [AppLanguage.Russian] = "Внутр. номер",
            [AppLanguage.English] = "EXT"
        },
        ["Actions"] = new()
        {
            [AppLanguage.UzbekLatin] = "Amallar",
            [AppLanguage.UzbekCyrillic] = "Амаллар",
            [AppLanguage.Russian] = "Действия",
            [AppLanguage.English] = "ACTIONS"
        },
        ["Week"] = new()
        {
            [AppLanguage.UzbekLatin] = "1 hafta",
            [AppLanguage.UzbekCyrillic] = "1 ҳафта",
            [AppLanguage.Russian] = "1 неделя",
            [AppLanguage.English] = "1 Week"
        },
        ["Month"] = new()
        {
            [AppLanguage.UzbekLatin] = "1 oy",
            [AppLanguage.UzbekCyrillic] = "1 ой",
            [AppLanguage.Russian] = "1 месяц",
            [AppLanguage.English] = "1 Month"
        },
        ["TotalTalkTime"] = new()
        {
            [AppLanguage.UzbekLatin] = "Jami suhbat",
            [AppLanguage.UzbekCyrillic] = "Жами суҳбат",
            [AppLanguage.Russian] = "Общее время",
            [AppLanguage.English] = "Total Talk"
        },
        ["TotalTimeSpent"] = new()
        {
            [AppLanguage.UzbekLatin] = "Jami sarflagan",
            [AppLanguage.UzbekCyrillic] = "Жами сарфланган",
            [AppLanguage.Russian] = "Затрачено",
            [AppLanguage.English] = "Total Spent"
        },
        ["Numbers"] = new()
        {
            [AppLanguage.UzbekLatin] = "Raqamlar",
            [AppLanguage.UzbekCyrillic] = "Рақамлар",
            [AppLanguage.Russian] = "Номера",
            [AppLanguage.English] = "Numbers"
        },
        ["PhoneNumber"] = new()
        {
            [AppLanguage.UzbekLatin] = "Telefon raqami",
            [AppLanguage.UzbekCyrillic] = "Телефон рақами",
            [AppLanguage.Russian] = "Номер телефона",
            [AppLanguage.English] = "Phone Number"
        },
        ["LastCall"] = new()
        {
            [AppLanguage.UzbekLatin] = "Oxirgi qo'ng'iroq",
            [AppLanguage.UzbekCyrillic] = "Охирги қўнғироқ",
            [AppLanguage.Russian] = "Последний звонок",
            [AppLanguage.English] = "Last Call"
        },
        ["CallDetails"] = new()
        {
            [AppLanguage.UzbekLatin] = "Qo'ng'iroq tafsilotlari",
            [AppLanguage.UzbekCyrillic] = "Қўнғироқ тафсилотлари",
            [AppLanguage.Russian] = "Детали звонков",
            [AppLanguage.English] = "Call Details"
        },
        ["OperatorProfile"] = new()
        {
            [AppLanguage.UzbekLatin] = "Operator profili",
            [AppLanguage.UzbekCyrillic] = "Оператор профили",
            [AppLanguage.Russian] = "Профиль оператора",
            [AppLanguage.English] = "Operator Profile"
        },
        ["BackToList"] = new()
        {
            [AppLanguage.UzbekLatin] = "Ro'yxatga qaytish",
            [AppLanguage.UzbekCyrillic] = "Рўйхатга қайтиш",
            [AppLanguage.Russian] = "К списку",
            [AppLanguage.English] = "Back"
        },
        ["DailyTalkTime"] = new()
        {
            [AppLanguage.UzbekLatin] = "Kunlik suhbat vaqti",
            [AppLanguage.UzbekCyrillic] = "Кунлик суҳбат вақти",
            [AppLanguage.Russian] = "Время разговора за день",
            [AppLanguage.English] = "Daily Talk Time"
        },
        ["AvgResponseTime"] = new()
        {
            [AppLanguage.UzbekLatin] = "O'rtacha javob vaqti",
            [AppLanguage.UzbekCyrillic] = "Ўртача жавоб вақти",
            [AppLanguage.Russian] = "Среднее время ответа",
            [AppLanguage.English] = "Avg Response Time"
        },
        ["RecentActivity"] = new()
        {
            [AppLanguage.UzbekLatin] = "So'nggi faoliyat",
            [AppLanguage.UzbekCyrillic] = "Сўнгги фаолият",
            [AppLanguage.Russian] = "Последняя активность",
            [AppLanguage.English] = "Recent Activity"
        },
        ["CallReference"] = new()
        {
            [AppLanguage.UzbekLatin] = "Qo'ng'iroq ID",
            [AppLanguage.UzbekCyrillic] = "Қўнғироқ ID",
            [AppLanguage.Russian] = "Ссылка",
            [AppLanguage.English] = "Call Ref."
        },
        ["Timestamp"] = new()
        {
            [AppLanguage.UzbekLatin] = "Vaqt",
            [AppLanguage.UzbekCyrillic] = "Вақт",
            [AppLanguage.Russian] = "Время",
            [AppLanguage.English] = "Time"
        },
        ["Caller"] = new()
        {
            [AppLanguage.UzbekLatin] = "Raqam",
            [AppLanguage.UzbekCyrillic] = "Рақам",
            [AppLanguage.Russian] = "Номер",
            [AppLanguage.English] = "Caller"
        },
        ["DownloadCsv"] = new()
        {
            [AppLanguage.UzbekLatin] = "CSV yuklab olish",
            [AppLanguage.UzbekCyrillic] = "CSV юклаб олиш",
            [AppLanguage.Russian] = "Скачать CSV",
            [AppLanguage.English] = "Download CSV"
        },
        ["Download"] = new()
        {
            [AppLanguage.UzbekLatin] = "Yuklash",
            [AppLanguage.UzbekCyrillic] = "Юклаш",
            [AppLanguage.Russian] = "Скачать",
            [AppLanguage.English] = "Download"
        },
        ["Yesterday"] = new()
        {
            [AppLanguage.UzbekLatin] = "Kecha",
            [AppLanguage.UzbekCyrillic] = "Кеча",
            [AppLanguage.Russian] = "Вчера",
            [AppLanguage.English] = "Yesterday"
        },
        ["RenameOperator"] = new()
        {
            [AppLanguage.UzbekLatin] = "Nomni o'zgartirish",
            [AppLanguage.UzbekCyrillic] = "Номни ўзгартириш",
            [AppLanguage.Russian] = "Переименовать",
            [AppLanguage.English] = "Rename"
        },
        ["Save"] = new()
        {
            [AppLanguage.UzbekLatin] = "Saqlash",
            [AppLanguage.UzbekCyrillic] = "Сақлаш",
            [AppLanguage.Russian] = "Сохранить",
            [AppLanguage.English] = "Save"
        },
        ["Cancel"] = new()
        {
            [AppLanguage.UzbekLatin] = "Bekor qilish",
            [AppLanguage.UzbekCyrillic] = "Бекор қилиш",
            [AppLanguage.Russian] = "Отмена",
            [AppLanguage.English] = "Cancel"
        },

        // ═══════════════════════════════════════════════════════════════════════
        // Sozlamalar sahifasi uchun qo'shimcha tarjimalar
        // ═══════════════════════════════════════════════════════════════════════
        
        ["OperatorManagement"] = new()
        {
            [AppLanguage.UzbekLatin] = "Operatorlar boshqaruvi",
            [AppLanguage.UzbekCyrillic] = "Операторлар бошқаруви",
            [AppLanguage.Russian] = "Управление операторами",
            [AppLanguage.English] = "Operator Management"
        },
        ["OperatorManagementDesc"] = new()
        {
            [AppLanguage.UzbekLatin] = "Operator nomlarini tahrirlang yoki mobil raqam qo'shing. Ma'lumotlar faqat bu kompyuterda saqlanadi.",
            [AppLanguage.UzbekCyrillic] = "Оператор номларини таҳрирланг ёки мобил рақам қўшинг. Маълумотлар фақат бу компьютерда сақланади.",
            [AppLanguage.Russian] = "Редактируйте имена операторов или добавьте мобильный номер. Данные хранятся только на этом компьютере.",
            [AppLanguage.English] = "Edit operator names or add mobile number. Data is stored only on this computer."
        },
        ["FullName"] = new()
        {
            [AppLanguage.UzbekLatin] = "Ism Familiya",
            [AppLanguage.UzbekCyrillic] = "Исм Фамилия",
            [AppLanguage.Russian] = "Имя Фамилия",
            [AppLanguage.English] = "Full Name"
        },
        ["MobileNumber"] = new()
        {
            [AppLanguage.UzbekLatin] = "Mobil raqam",
            [AppLanguage.UzbekCyrillic] = "Мобил рақам",
            [AppLanguage.Russian] = "Мобильный номер",
            [AppLanguage.English] = "Mobile Number"
        },
        ["AddOperator"] = new()
        {
            [AppLanguage.UzbekLatin] = "Operator qo'shish",
            [AppLanguage.UzbekCyrillic] = "Оператор қўшиш",
            [AppLanguage.Russian] = "Добавить оператора",
            [AppLanguage.English] = "Add Operator"
        },
        ["AddOperatorHint"] = new()
        {
            [AppLanguage.UzbekLatin] = "CDR da ko'rinmagan (hali qo'ng'iroq qilmagan) operator uchun qo'lda qo'shish mumkin.",
            [AppLanguage.UzbekCyrillic] = "CDR да кўринмаган (ҳали қўнғироқ қилмаган) оператор учун қўлда қўшиш мумкин.",
            [AppLanguage.Russian] = "Можно добавить вручную оператора, который не отображается в CDR (еще не звонил).",
            [AppLanguage.English] = "You can manually add an operator not visible in CDR (hasn't made calls yet)."
        },
        ["ExtensionExample"] = new()
        {
            [AppLanguage.UzbekLatin] = "Extension (masalan: 1005)",
            [AppLanguage.UzbekCyrillic] = "Extension (масалан: 1005)",
            [AppLanguage.Russian] = "Extension (например: 1005)",
            [AppLanguage.English] = "Extension (e.g.: 1005)"
        },
        ["Loading"] = new()
        {
            [AppLanguage.UzbekLatin] = "Yuklanmoqda...",
            [AppLanguage.UzbekCyrillic] = "Юкланмоқда...",
            [AppLanguage.Russian] = "Загрузка...",
            [AppLanguage.English] = "Loading..."
        },
        ["RemoveOperator"] = new()
        {
            [AppLanguage.UzbekLatin] = "O'chirish",
            [AppLanguage.UzbekCyrillic] = "Ўчириш",
            [AppLanguage.Russian] = "Удалить",
            [AppLanguage.English] = "Remove"
        },
        ["ConfirmRemoveOperator"] = new()
        {
            [AppLanguage.UzbekLatin] = "Operatorni o'chirishni tasdiqlaysizmi?",
            [AppLanguage.UzbekCyrillic] = "Операторни ўчиришни тасдиқлайсизми?",
            [AppLanguage.Russian] = "Подтвердите удаление оператора?",
            [AppLanguage.English] = "Confirm operator removal?"
        },
        ["Yes"] = new()
        {
            [AppLanguage.UzbekLatin] = "Ha",
            [AppLanguage.UzbekCyrillic] = "Ҳа",
            [AppLanguage.Russian] = "Да",
            [AppLanguage.English] = "Yes"
        },
        ["No"] = new()
        {
            [AppLanguage.UzbekLatin] = "Yo'q",
            [AppLanguage.UzbekCyrillic] = "Йўқ",
            [AppLanguage.Russian] = "Нет",
            [AppLanguage.English] = "No"
        },

        // Backend ulanish
        ["BackendConnection"] = new()
        {
            [AppLanguage.UzbekLatin] = "Backend ulanish",
            [AppLanguage.UzbekCyrillic] = "Backend уланиш",
            [AppLanguage.Russian] = "Подключение к серверу",
            [AppLanguage.English] = "Backend Connection"
        },
        ["BackendUrl"] = new()
        {
            [AppLanguage.UzbekLatin] = "Backend URL",
            [AppLanguage.UzbekCyrillic] = "Backend URL",
            [AppLanguage.Russian] = "URL сервера",
            [AppLanguage.English] = "Backend URL"
        },
        ["BackendUrlHint"] = new()
        {
            [AppLanguage.UzbekLatin] = "API server manzilini kiriting (masalan: https://example.com/)",
            [AppLanguage.UzbekCyrillic] = "API сервер манзилини киритинг (масалан: https://example.com/)",
            [AppLanguage.Russian] = "Введите адрес API сервера (например: https://example.com/)",
            [AppLanguage.English] = "Enter API server address (e.g.: https://example.com/)"
        },
        ["BackendUrlSaved"] = new()
        {
            [AppLanguage.UzbekLatin] = "Backend URL saqlandi.",
            [AppLanguage.UzbekCyrillic] = "Backend URL сақланди.",
            [AppLanguage.Russian] = "URL сервера сохранен.",
            [AppLanguage.English] = "Backend URL saved."
        },
        ["BackendUrlInvalid"] = new()
        {
            [AppLanguage.UzbekLatin] = "Backend URL noto'g'ri. To'liq manzil kiriting (http:// yoki https:// bilan).",
            [AppLanguage.UzbekCyrillic] = "Backend URL нотўғри. Тўлиқ манзил киритинг (http:// ёки https:// билан).",
            [AppLanguage.Russian] = "Неверный URL сервера. Введите полный адрес (с http:// или https://).",
            [AppLanguage.English] = "Invalid backend URL. Enter a full address (with http:// or https://)."
        },
        ["ConnectionStatus"] = new()
        {
            [AppLanguage.UzbekLatin] = "Ulanish holati",
            [AppLanguage.UzbekCyrillic] = "Уланиш ҳолати",
            [AppLanguage.Russian] = "Статус подключения",
            [AppLanguage.English] = "Connection Status"
        },
        ["PollingInterval"] = new()
        {
            [AppLanguage.UzbekLatin] = "Ma'lumot yangilanish intervali",
            [AppLanguage.UzbekCyrillic] = "Маълумот янгиланиш интервали",
            [AppLanguage.Russian] = "Интервал обновления данных",
            [AppLanguage.English] = "Data refresh interval"
        },
        ["PollingStatus"] = new()
        {
            [AppLanguage.UzbekLatin] = "Avtomatik yangilanish",
            [AppLanguage.UzbekCyrillic] = "Автоматик янгиланиш",
            [AppLanguage.Russian] = "Автоматическое обновление",
            [AppLanguage.English] = "Auto-refresh"
        },
        ["SignalRConnection"] = new()
        {
            [AppLanguage.UzbekLatin] = "Backend bilan ulanish",
            [AppLanguage.UzbekCyrillic] = "Backend билан уланиш",
            [AppLanguage.Russian] = "Подключение к backend",
            [AppLanguage.English] = "Backend connection"
        },
        ["Reconnect"] = new()
        {
            [AppLanguage.UzbekLatin] = "Qayta ulash",
            [AppLanguage.UzbekCyrillic] = "Қайта улаш",
            [AppLanguage.Russian] = "Переподключить",
            [AppLanguage.English] = "Reconnect"
        },
        ["Connecting"] = new()
        {
            [AppLanguage.UzbekLatin] = "Ulanmoqda...",
            [AppLanguage.UzbekCyrillic] = "Уланмоқда...",
            [AppLanguage.Russian] = "Подключение...",
            [AppLanguage.English] = "Connecting..."
        },

        // Ilova haqida
        ["AppName"] = new()
        {
            [AppLanguage.UzbekLatin] = "OptiVis — IP Telefon Analitikasi",
            [AppLanguage.UzbekCyrillic] = "OptiVis — IP Телефон Аналитикаси",
            [AppLanguage.Russian] = "OptiVis — IP Телефон Аналитика",
            [AppLanguage.English] = "OptiVis — IP Phone Analytics"
        },
        ["AppVersion"] = new()
        {
            [AppLanguage.UzbekLatin] = "Versiya",
            [AppLanguage.UzbekCyrillic] = "Версия",
            [AppLanguage.Russian] = "Версия",
            [AppLanguage.English] = "Version"
        },
        ["AppInfo"] = new()
        {
            [AppLanguage.UzbekLatin] = "Issabel PBX CDR ma'lumotlarini real-vaqtda tahlil qilish",
            [AppLanguage.UzbekCyrillic] = "Issabel PBX CDR маълумотларини реал-вақтда таҳлил қилиш",
            [AppLanguage.Russian] = "Анализ данных CDR Issabel PBX в реальном времени",
            [AppLanguage.English] = "Real-time analysis of Issabel PBX CDR data"
        },
        ["Copyright"] = new()
        {
            [AppLanguage.UzbekLatin] = "© 2026 Ovoza dasturlar",
            [AppLanguage.UzbekCyrillic] = "© 2026 Ovoza дастурлар",
            [AppLanguage.Russian] = "© 2026 Ovoza программы",
            [AppLanguage.English] = "© 2026 Ovoza Software"
        },
        ["Telegram"] = new()
        {
            [AppLanguage.UzbekLatin] = "Telegram",
            [AppLanguage.UzbekCyrillic] = "Телеграм",
            [AppLanguage.Russian] = "Телеграм",
            [AppLanguage.English] = "Telegram"
        },

        // Ko'rinish sozlamalari
        ["ThemeDescription"] = new()
        {
            [AppLanguage.UzbekLatin] = "Ilova ko'rinishini o'zgartirish",
            [AppLanguage.UzbekCyrillic] = "Илова кўринишини ўзгартириш",
            [AppLanguage.Russian] = "Изменить внешний вид приложения",
            [AppLanguage.English] = "Change application appearance"
        },
        ["LanguageDescription"] = new()
        {
            [AppLanguage.UzbekLatin] = "Interfeys tilini tanlang",
            [AppLanguage.UzbekCyrillic] = "Интерфейс тилини танланг",
            [AppLanguage.Russian] = "Выберите язык интерфейса",
            [AppLanguage.English] = "Select interface language"
        },

        // Dashboard sahifasi
        ["DashboardTitle"] = new()
        {
            [AppLanguage.UzbekLatin] = "Bosh sahifa",
            [AppLanguage.UzbekCyrillic] = "Бош саҳифа",
            [AppLanguage.Russian] = "Главная",
            [AppLanguage.English] = "Dashboard"
        },
        ["TodayStats"] = new()
        {
            [AppLanguage.UzbekLatin] = "Bugungi statistika",
            [AppLanguage.UzbekCyrillic] = "Бугунги статистика",
            [AppLanguage.Russian] = "Статистика за сегодня",
            [AppLanguage.English] = "Today's Statistics"
        },
        ["InboundCalls"] = new()
        {
            [AppLanguage.UzbekLatin] = "Kiruvchi qo'ng'iroqlar",
            [AppLanguage.UzbekCyrillic] = "Кирувчи қўнғироқлар",
            [AppLanguage.Russian] = "Входящие звонки",
            [AppLanguage.English] = "Inbound Calls"
        },
        ["OutboundCalls"] = new()
        {
            [AppLanguage.UzbekLatin] = "Chiquvchi qo'ng'iroqlar",
            [AppLanguage.UzbekCyrillic] = "Чиқувчи қўнғироқлар",
            [AppLanguage.Russian] = "Исходящие звонки",
            [AppLanguage.English] = "Outbound Calls"
        },
        ["MissedCalls"] = new()
        {
            [AppLanguage.UzbekLatin] = "O'tkazib yuborilgan",
            [AppLanguage.UzbekCyrillic] = "Ўтказиб юборилган",
            [AppLanguage.Russian] = "Пропущенные",
            [AppLanguage.English] = "Missed Calls"
        },

        // Operatorlar sahifasi
        ["OperatorsTitle"] = new()
        {
            [AppLanguage.UzbekLatin] = "Operatorlar ro'yxati",
            [AppLanguage.UzbekCyrillic] = "Операторлар рўйхати",
            [AppLanguage.Russian] = "Список операторов",
            [AppLanguage.English] = "Operators List"
        },
        ["SearchOperator"] = new()
        {
            [AppLanguage.UzbekLatin] = "Operator qidirish...",
            [AppLanguage.UzbekCyrillic] = "Оператор қидириш...",
            [AppLanguage.Russian] = "Поиск оператора...",
            [AppLanguage.English] = "Search operator..."
        },
        ["ViewDetails"] = new()
        {
            [AppLanguage.UzbekLatin] = "Batafsil ko'rish",
            [AppLanguage.UzbekCyrillic] = "Батафсил кўриш",
            [AppLanguage.Russian] = "Подробнее",
            [AppLanguage.English] = "View Details"
        },

        // Operator Detail sahifasi
        ["OperatorDetail"] = new()
        {
            [AppLanguage.UzbekLatin] = "Operator ma'lumotlari",
            [AppLanguage.UzbekCyrillic] = "Оператор маълумотлари",
            [AppLanguage.Russian] = "Информация об операторе",
            [AppLanguage.English] = "Operator Details"
        },
        ["CallHistory"] = new()
        {
            [AppLanguage.UzbekLatin] = "Qo'ng'iroqlar tarixi",
            [AppLanguage.UzbekCyrillic] = "Қўнғироқлар тарихи",
            [AppLanguage.Russian] = "История звонков",
            [AppLanguage.English] = "Call History"
        },
        ["Performance"] = new()
        {
            [AppLanguage.UzbekLatin] = "Samaradorlik",
            [AppLanguage.UzbekCyrillic] = "Самарадорлик",
            [AppLanguage.Russian] = "Производительность",
            [AppLanguage.English] = "Performance"
        },

        // Raqamlar sahifasi (Search)
        ["NumbersTitle"] = new()
        {
            [AppLanguage.UzbekLatin] = "Raqamlar qidiruvi",
            [AppLanguage.UzbekCyrillic] = "Рақамлар қидируви",
            [AppLanguage.Russian] = "Поиск номеров",
            [AppLanguage.English] = "Number Search"
        },
        ["SearchByNumber"] = new()
        {
            [AppLanguage.UzbekLatin] = "Raqam bo'yicha qidirish",
            [AppLanguage.UzbekCyrillic] = "Рақам бўйича қидириш",
            [AppLanguage.Russian] = "Поиск по номеру",
            [AppLanguage.English] = "Search by Number"
        },
        ["NoResults"] = new()
        {
            [AppLanguage.UzbekLatin] = "Natija topilmadi",
            [AppLanguage.UzbekCyrillic] = "Натижа топилмади",
            [AppLanguage.Russian] = "Результаты не найдены",
            [AppLanguage.English] = "No Results Found"
        },
        ["Clear"] = new()
        {
            [AppLanguage.UzbekLatin] = "Tozalash",
            [AppLanguage.UzbekCyrillic] = "Тозалаш",
            [AppLanguage.Russian] = "Очистить",
            [AppLanguage.English] = "Clear"
        },
        ["Apply"] = new()
        {
            [AppLanguage.UzbekLatin] = "Qo'llash",
            [AppLanguage.UzbekCyrillic] = "Қўллаш",
            [AppLanguage.Russian] = "Применить",
            [AppLanguage.English] = "Apply"
        },

        // Umumiy
        ["Refresh"] = new()
        {
            [AppLanguage.UzbekLatin] = "Yangilash",
            [AppLanguage.UzbekCyrillic] = "Янгилаш",
            [AppLanguage.Russian] = "Обновить",
            [AppLanguage.English] = "Refresh"
        },
        ["Close"] = new()
        {
            [AppLanguage.UzbekLatin] = "Yopish",
            [AppLanguage.UzbekCyrillic] = "Ёпиш",
            [AppLanguage.Russian] = "Закрыть",
            [AppLanguage.English] = "Close"
        },
        ["Delete"] = new()
        {
            [AppLanguage.UzbekLatin] = "O'chirish",
            [AppLanguage.UzbekCyrillic] = "Ўчириш",
            [AppLanguage.Russian] = "Удалить",
            [AppLanguage.English] = "Delete"
        },
        ["Edit"] = new()
        {
            [AppLanguage.UzbekLatin] = "Tahrirlash",
            [AppLanguage.UzbekCyrillic] = "Таҳрирлаш",
            [AppLanguage.Russian] = "Редактировать",
            [AppLanguage.English] = "Edit"
        },
        ["Confirm"] = new()
        {
            [AppLanguage.UzbekLatin] = "Tasdiqlash",
            [AppLanguage.UzbekCyrillic] = "Тасдиқлаш",
            [AppLanguage.Russian] = "Подтвердить",
            [AppLanguage.English] = "Confirm"
        },
        ["Warning"] = new()
        {
            [AppLanguage.UzbekLatin] = "Ogohlantirish",
            [AppLanguage.UzbekCyrillic] = "Огоҳлантириш",
            [AppLanguage.Russian] = "Предупреждение",
            [AppLanguage.English] = "Warning"
        },
        ["Error"] = new()
        {
            [AppLanguage.UzbekLatin] = "Xatolik",
            [AppLanguage.UzbekCyrillic] = "Хатолик",
            [AppLanguage.Russian] = "Ошибка",
            [AppLanguage.English] = "Error"
        },
        ["Info"] = new()
        {
            [AppLanguage.UzbekLatin] = "Ma'lumot",
            [AppLanguage.UzbekCyrillic] = "Маълумот",
            [AppLanguage.Russian] = "Информация",
            [AppLanguage.English] = "Information"
        },
        ["OvozaDescription"] = new()
        {
            [AppLanguage.UzbekLatin] = "Biznesni avtomatlashtirish va IT yechimlari bo'yicha xizmatlar.",
            [AppLanguage.UzbekCyrillic] = "Бизнесни автоматлаштириш ва IT ечимлари бўйича хизматлар.",
            [AppLanguage.Russian] = "Услуги по автоматизации бизнеса и IT-решениям.",
            [AppLanguage.English] = "Business automation and IT solutions services."
        },
        ["Numbers"] = new()
        {
            [AppLanguage.UzbekLatin] = "Raqamlar",
            [AppLanguage.UzbekCyrillic] = "Рақамлар",
            [AppLanguage.Russian] = "Номера",
            [AppLanguage.English] = "Numbers"
        },
        ["TotalCalls"] = new()
        {
            [AppLanguage.UzbekLatin] = "Jami qo'ng'iroqlar",
            [AppLanguage.UzbekCyrillic] = "Жами қўнғироқлар",
            [AppLanguage.Russian] = "Всего звонков",
            [AppLanguage.English] = "Total Calls"
        },
        ["Incoming"] = new()
        {
            [AppLanguage.UzbekLatin] = "Kiruvchi",
            [AppLanguage.UzbekCyrillic] = "Кирувчи",
            [AppLanguage.Russian] = "Входящие",
            [AppLanguage.English] = "Incoming"
        },
        ["Outgoing"] = new()
        {
            [AppLanguage.UzbekLatin] = "Chiquvchi",
            [AppLanguage.UzbekCyrillic] = "Чиқувчи",
            [AppLanguage.Russian] = "Исходящие",
            [AppLanguage.English] = "Outgoing"
        },
        ["Connected"] = new()
        {
            [AppLanguage.UzbekLatin] = "Bog'lanildi",
            [AppLanguage.UzbekCyrillic] = "Боғланилди",
            [AppLanguage.Russian] = "Подключено",
            [AppLanguage.English] = "Connected"
        },
        ["SuccessRate"] = new()
        {
            [AppLanguage.UzbekLatin] = "Muvaffaqiyat %",
            [AppLanguage.UzbekCyrillic] = "Муваффақият %",
            [AppLanguage.Russian] = "Успешность %",
            [AppLanguage.English] = "Success Rate"
        },
        ["TotalTalkTime"] = new()
        {
            [AppLanguage.UzbekLatin] = "Jami suhbat vaqti",
            [AppLanguage.UzbekCyrillic] = "Жами суҳбат вақти",
            [AppLanguage.Russian] = "Общее время разговора",
            [AppLanguage.English] = "Total Talk Time"
        },
        ["Duration"] = new()
        {
            [AppLanguage.UzbekLatin] = "Davomiylik",
            [AppLanguage.UzbekCyrillic] = "Давомийлик",
            [AppLanguage.Russian] = "Длительность",
            [AppLanguage.English] = "Duration"
        },
        ["Today"] = new()
        {
            [AppLanguage.UzbekLatin] = "Bugun",
            [AppLanguage.UzbekCyrillic] = "Бугун",
            [AppLanguage.Russian] = "Сегодня",
            [AppLanguage.English] = "Today"
        },
        ["Yesterday"] = new()
        {
            [AppLanguage.UzbekLatin] = "Kecha",
            [AppLanguage.UzbekCyrillic] = "Кеча",
            [AppLanguage.Russian] = "Вчера",
            [AppLanguage.English] = "Yesterday"
        },
        ["Week"] = new()
        {
            [AppLanguage.UzbekLatin] = "Hafta",
            [AppLanguage.UzbekCyrillic] = "Ҳафта",
            [AppLanguage.Russian] = "Неделя",
            [AppLanguage.English] = "Week"
        },
        ["Month"] = new()
        {
            [AppLanguage.UzbekLatin] = "Oy",
            [AppLanguage.UzbekCyrillic] = "Ой",
            [AppLanguage.Russian] = "Месяц",
            [AppLanguage.English] = "Month"
        },
        ["From"] = new()
        {
            [AppLanguage.UzbekLatin] = "Dan:",
            [AppLanguage.UzbekCyrillic] = "Дан:",
            [AppLanguage.Russian] = "От:",
            [AppLanguage.English] = "From:"
        },
        ["To"] = new()
        {
            [AppLanguage.UzbekLatin] = "Gacha:",
            [AppLanguage.UzbekCyrillic] = "Гача:",
            [AppLanguage.Russian] = "До:",
            [AppLanguage.English] = "To:"
        },
        ["ExcelExport"] = new()
        {
            [AppLanguage.UzbekLatin] = "Excel eksport",
            [AppLanguage.UzbekCyrillic] = "Excel экспорт",
            [AppLanguage.Russian] = "Экспорт в Excel",
            [AppLanguage.English] = "Excel Export"
        },
        ["Disconnected"] = new()
        {
            [AppLanguage.UzbekLatin] = "Uzilgan",
            [AppLanguage.UzbekCyrillic] = "Узилган",
            [AppLanguage.Russian] = "Отключено",
            [AppLanguage.English] = "Disconnected"
        },
        ["CallsInSelectedPeriod"] = new()
        {
            [AppLanguage.UzbekLatin] = "Tanlangan muddatdagi qo'ng'iroqlar:",
            [AppLanguage.UzbekCyrillic] = "Танланган муддатдаги қўнғироқлар:",
            [AppLanguage.Russian] = "Звонки за выбранный период:",
            [AppLanguage.English] = "Calls in selected period:"
        },
        ["GeneralStats"] = new()
        {
            [AppLanguage.UzbekLatin] = "Umumiy ma'lumotlar",
            [AppLanguage.UzbekCyrillic] = "Умумий маълумотлар",
            [AppLanguage.Russian] = "Общая информация",
            [AppLanguage.English] = "General Statistics"
        },
        ["NotConnected"] = new()
        {
            [AppLanguage.UzbekLatin] = "Bog'lanilmadi",
            [AppLanguage.UzbekCyrillic] = "Боғланилмади",
            [AppLanguage.Russian] = "Не подключено",
            [AppLanguage.English] = "Not Connected"
        },
        ["TotalDuration"] = new()
        {
            [AppLanguage.UzbekLatin] = "Jami davomiylik",
            [AppLanguage.UzbekCyrillic] = "Жами давомийлик",
            [AppLanguage.Russian] = "Общая длительность",
            [AppLanguage.English] = "Total Duration"
        },
        ["TotalOutgoing"] = new()
        {
            [AppLanguage.UzbekLatin] = "Jami chiquvchi",
            [AppLanguage.UzbekCyrillic] = "Жами чиқувчи",
            [AppLanguage.Russian] = "Всего исходящих",
            [AppLanguage.English] = "Total Outgoing"
        },
        ["GotAnswer"] = new()
        {
            [AppLanguage.UzbekLatin] = "Javob olindi",
            [AppLanguage.UzbekCyrillic] = "Жавоб олинди",
            [AppLanguage.Russian] = "Получен ответ",
            [AppLanguage.English] = "Got Answer"
        },
        ["NoAnswerGot"] = new()
        {
            [AppLanguage.UzbekLatin] = "Javob berilmadi",
            [AppLanguage.UzbekCyrillic] = "Жавоб берилмади",
            [AppLanguage.Russian] = "Нет ответа",
            [AppLanguage.English] = "No Answer"
        },
        ["AnswerRate"] = new()
        {
            [AppLanguage.UzbekLatin] = "Javob foizi",
            [AppLanguage.UzbekCyrillic] = "Жавоб фоизи",
            [AppLanguage.Russian] = "Процент ответов",
            [AppLanguage.English] = "Answer Rate"
        },
        ["AvgTalkTime"] = new()
        {
            [AppLanguage.UzbekLatin] = "O'rtacha suhbat",
            [AppLanguage.UzbekCyrillic] = "Ўртача суҳбат",
            [AppLanguage.Russian] = "Среднее время разговора",
            [AppLanguage.English] = "Avg Talk Time"
        },
        ["AvgDuration"] = new()
        {
            [AppLanguage.UzbekLatin] = "O'rtacha davomiylik",
            [AppLanguage.UzbekCyrillic] = "Ўртача давомийлик",
            [AppLanguage.Russian] = "Средняя длительность",
            [AppLanguage.English] = "Avg Duration"
        },
        ["TotalIncoming"] = new()
        {
            [AppLanguage.UzbekLatin] = "Jami kiruvchi",
            [AppLanguage.UzbekCyrillic] = "Жами кирувчи",
            [AppLanguage.Russian] = "Всего входящих",
            [AppLanguage.English] = "Total Incoming"
        },
        ["Answered"] = new()
        {
            [AppLanguage.UzbekLatin] = "Javob berildi",
            [AppLanguage.UzbekCyrillic] = "Жавоб берилди",
            [AppLanguage.Russian] = "Отвечено",
            [AppLanguage.English] = "Answered"
        },
        ["Missed"] = new()
        {
            [AppLanguage.UzbekLatin] = "O'tkazib yuborildi",
            [AppLanguage.UzbekCyrillic] = "Ўтказиб юборилди",
            [AppLanguage.Russian] = "Пропущено",
            [AppLanguage.English] = "Missed"
        },
        ["AvgWaitTime"] = new()
        {
            [AppLanguage.UzbekLatin] = "O'rtacha kutish",
            [AppLanguage.UzbekCyrillic] = "Ўртача кутиш",
            [AppLanguage.Russian] = "Среднее ожидание",
            [AppLanguage.English] = "Avg Wait Time"
        },
        ["NoCalls"] = new()
        {
            [AppLanguage.UzbekLatin] = "Qo'ng'iroqlar yo'q",
            [AppLanguage.UzbekCyrillic] = "Қўнғироқлар йўқ",
            [AppLanguage.Russian] = "Нет звонков",
            [AppLanguage.English] = "No Calls"
        },
        ["StatusDistribution"] = new()
        {
            [AppLanguage.UzbekLatin] = "Holat taqsimoti",
            [AppLanguage.UzbekCyrillic] = "Ҳолат тақсимоти",
            [AppLanguage.Russian] = "Распределение статусов",
            [AppLanguage.English] = "Status Distribution"
        },
        ["General"] = new()
        {
            [AppLanguage.UzbekLatin] = "Umumiy",
            [AppLanguage.UzbekCyrillic] = "Умумий",
            [AppLanguage.Russian] = "Общие",
            [AppLanguage.English] = "General"
        },
        ["CallTrend"] = new()
        {
            [AppLanguage.UzbekLatin] = "Qo'ng'iroqlar trendi",
            [AppLanguage.UzbekCyrillic] = "Қўнғироқлар тренди",
            [AppLanguage.Russian] = "Тренд звонков",
            [AppLanguage.English] = "Call Trend"
        },
        ["DataLoading"] = new()
        {
            [AppLanguage.UzbekLatin] = "Ma'lumotlar yuklanmoqda...",
            [AppLanguage.UzbekCyrillic] = "Маълумотлар юкланмоқда...",
            [AppLanguage.Russian] = "Загрузка данных...",
            [AppLanguage.English] = "Loading data..."
        },
        ["NoCallsInPeriod"] = new()
        {
            [AppLanguage.UzbekLatin] = "Bu oraliqda qo'ng'iroqlar topilmadi",
            [AppLanguage.UzbekCyrillic] = "Бу оралиқда қўнғироқлар топилмади",
            [AppLanguage.Russian] = "Звонки в этом диапазоне не найдены",
            [AppLanguage.English] = "No calls found in this period"
        },
        ["Active"] = new()
        {
            [AppLanguage.UzbekLatin] = "● Faol",
            [AppLanguage.UzbekCyrillic] = "● Фаол",
            [AppLanguage.Russian] = "● Активен",
            [AppLanguage.English] = "● Active"
        },
        ["Inactive"] = new()
        {
            [AppLanguage.UzbekLatin] = "○ Nofaol",
            [AppLanguage.UzbekCyrillic] = "○ Нофаол",
            [AppLanguage.Russian] = "○ Неактивен",
            [AppLanguage.English] = "○ Inactive"
        },
    };

    public static string Get(string key)
    {
        if (_translations.TryGetValue(key, out var languageDict))
        {
            if (languageDict.TryGetValue(_currentLanguage, out var translation))
            {
                return translation;
            }
        }
        return key;
    }

    public static string GetLanguageName(AppLanguage language) => language switch
    {
        AppLanguage.UzbekLatin => "O'zbek",
        AppLanguage.UzbekCyrillic => "Ўзбек",
        AppLanguage.Russian => "Русский",
        AppLanguage.English => "English",
        _ => "Unknown"
    };
}
