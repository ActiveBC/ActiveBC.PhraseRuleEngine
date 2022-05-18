using System.Collections.Generic;

namespace ActiveBC.PhraseRuleEngine.Benchmarking
{
    public static class DataProvider
    {
        public static class MatcherCases
        {
            public static readonly Dictionary<string, string> RuleSets = new Dictionary<string, string>()
            {
                {
                    "ner.time",
                    @"using System;

// root expression
TimeSpan Expression = peg#($BeforeOrAfter:multiplier $TimeSpan:timeSpan)# {
    return timeSpan.Multiply(multiplier);
}

// direction modifier
int BeforeOrAfter = peg#($Before:before|$After:after)# { return (before ?? 0) + (after ?? 0); }
int Before = peg#(за)# { return -1; }
int After = peg#(через)# { return 1; }

// alternatives of possible relative time phrase
TimeSpan TimeSpan = peg#($HoursExact:hoursExact|$HoursAndMinutes_WithWords:hmWithWords|$HoursAndMinutes_WithoutWords:hmNoWords|$HoursOnly_WithWord:h|$MinutesOnly_WithWord:m)# { return (hoursExact ?? hmWithWords ?? hmNoWords ?? h ?? m).Value; }

// different patterns for relative time phrase
TimeSpan HoursExact = peg#($HoursNumber_HalfAnHour:special|$HoursExact_WithHalf:common)# { return (special ?? common).Value; }
TimeSpan HoursExact_WithHalf = peg#($Doubles_Halves_After_0_12:hours $Word_Hour__S_Nom_S_Gen_P_Gen)# { return TimeSpan.FromHours(hours); }
TimeSpan HoursAndMinutes_WithWords = peg#($HoursNumber_WithWord:hours $MinutesNumber_WithWord:minutes)# { return hours.Add(minutes); }
TimeSpan HoursAndMinutes_WithoutWords = peg#($HoursNumber_WithoutExtraWord:hours $MinutesNumber_WithoutWord:minutes)# { return hours.Add(minutes); }
TimeSpan HoursOnly_WithWord = peg#($HoursNumber_WithWord:hours)# { return hours; }
TimeSpan MinutesOnly_WithWord = peg#($MinutesNumber_WithWord:minutes)# { return minutes; }

// minutes helpers
TimeSpan MinutesNumber_WithWord = peg#($MinutesNumber_WithoutWord:number $Word_Minute__S_Nom_S_Gen_P_Gen)# { return number; }
TimeSpan MinutesNumber_WithoutWord = peg#($Integer_0? $Integers_0_9__F_Acc:n_0_9|$Integers_10_19:n_10_19|$Integers_20_59__F_Acc:n_20_59)# { return TimeSpan.FromMinutes((n_0_9 ?? n_10_19 ?? n_20_59).Value); }

// hours helpers
TimeSpan HoursNumber_WithWord = peg#($HoursNumber_WithoutWord:common $Word_Hour__S_Nom_S_Gen_P_Gen|$HoursNumber_SingleHour:special)# { return (common ?? special).Value; }
TimeSpan HoursNumber_WithoutWord = peg#($Integers_0_12__M_Acc:hours)# { return TimeSpan.FromHours(hours); }
TimeSpan HoursNumber_WithoutExtraWord = peg#($HoursNumber_WithoutWord:hours|$HoursNumber_SingleHour:hour)# { return (hours ?? hour).Value; }
TimeSpan HoursNumber_SingleHour = peg#(час)# { return TimeSpan.FromHours(1); }
TimeSpan HoursNumber_HalfAnHour = peg#(полчаса)# { return TimeSpan.FromMinutes(30); }

// numeric helpers
// legend:
// if the word doesn't distinguish significant (in the context of current PEG-grammar) grammatical categories
// (such as declension, grammatical gender or grammatical number),
// the respective rule is not being marked with additional suffix, otherwise it is:
// ""F"" and ""M"" stand for feminine and masculine grammatical genders respectively
// ""S"" and ""P"" stand for singular and plural grammatical numbers respectively
// ""Nom"" and ""Acc"" stand for nominative and accusative cases respectively
int Integers_0_9__F_Acc = peg#($Integer_0:n_0|$Integers_1_9__F_Acc:n_1_9)# { return (n_0 ?? n_1_9).Value; }
int Integers_1_9__F_Acc = peg#($Integer_1__F_Acc:n_1|$Integer_2__F_Acc:n_2|$Integers_3_9:n_3_9)# { return (n_1 ?? n_2 ?? n_3_9).Value; }
int Integers_20_59__F_Acc = peg#($Integers_Tens_20_50:tens $Integers_1_9__F_Acc?:ones)# { return tens + (ones ?? 0); }
int Integers_0_9__M_Acc = peg#($Integer_0:n_0|$Integer_1__M_Acc:n_1|$Integer_2__M_Acc:n_2|$Integers_3_9:n_3_9)# { return (n_0 ?? n_1 ?? n_2 ?? n_3_9).Value; }
int Integers_0_12__M_Acc = peg#($Integers_0_9__M_Acc:n_0_9|$Integer_10:n_10|$Integer_11:n_11|$Integer_12:n_12)# { return (n_0_9 ?? n_10 ?? n_11 ?? n_12).Value; }
int Integers_3_9 = peg#($Integer_3:n_3|$Integer_4:n_4|$Integer_5:n_5|$Integer_6:n_6|$Integer_7:n_7|$Integer_8:n_8|$Integer_9:n_9)# { return (n_3 ?? n_4 ?? n_5 ?? n_6 ?? n_7 ?? n_8 ?? n_9).Value; }
int Integers_10_19 = peg#($Integer_10:n_10|$Integer_11:n_11|$Integer_12:n_12|$Integer_13:n_13|$Integer_14:n_14|$Integer_15:n_15|$Integer_16:n_16|$Integer_17:n_17|$Integer_18:n_18|$Integer_19:n_19)# { return (n_10 ?? n_11 ?? n_12 ?? n_13 ?? n_14 ?? n_15 ?? n_16 ?? n_17 ?? n_18 ?? n_19).Value; }

int Integer_0 = peg#(ноль)# { return 0; }
int Integer_1__M_Acc = peg#(один)# { return 1; }
int Integer_1__F_Acc = peg#(одну)# { return 1; }
int Integer_2__M_Acc = peg#(два)# { return 2; }
int Integer_2__F_Acc = peg#(две)# { return 2; }
int Integer_3 = peg#(три)# { return 3; }
int Integer_4 = peg#(четыре)# { return 4; }
int Integer_5 = peg#(пять)# { return 5; }
int Integer_6 = peg#(шесть)# { return 6; }
int Integer_7 = peg#(семь)# { return 7; }
int Integer_8 = peg#(восемь)# { return 8; }
int Integer_9 = peg#(девять)# { return 9; }
int Integer_10 = peg#(десять)# { return 10; }
int Integer_11 = peg#(одиннадцать)# { return 11; }
int Integer_12 = peg#(двенадцать)# { return 12; }
int Integer_13 = peg#(тринадцать)# { return 13; }
int Integer_14 = peg#(четырнадцать)# { return 14; }
int Integer_15 = peg#(пятнадцать)# { return 15; }
int Integer_16 = peg#(шестнадцать)# { return 16; }
int Integer_17 = peg#(семьнадцать)# { return 17; }
int Integer_18 = peg#(восемьнадцать)# { return 18; }
int Integer_19 = peg#(девятнадцать)# { return 19; }

int Integers_Tens_20_50 = peg#($Integers_Tens_20:n_20|$Integers_Tens_30:n_30|$Integers_Tens_40:n_40|$Integers_Tens_50:n_50)# { return (n_20 ?? n_30 ?? n_40 ?? n_50).Value; }
int Integers_Tens_20 = peg#(двадцать)# { return 20; }
int Integers_Tens_30 = peg#(тридцать)# { return 30; }
int Integers_Tens_40 = peg#(сорок)# { return 40; }
int Integers_Tens_50 = peg#(пятьдесят)# { return 50; }

double Doubles_Halves_After_0_12 = peg#($Doubles_Halves_After_0_Special:n_0|$Doubles_Halves_After_1_Special:n_1|$Doubles_Halves_After_0_12_Common:n_0_12)# { return (n_0 ?? n_1 ?? n_0_12).Value; }
double Doubles_Halves_After_0_Special = peg#(пол)# { return 0.5; }
double Doubles_Halves_After_1_Special = peg#(полтора)# { return 1.5; }
double Doubles_Halves_After_0_12_Common = peg#($Integers_0_12__M_Acc:n с половиной)# { return n + 0.5; }

// lexical helpers
void Word_Minute__S_Nom_S_Gen_P_Gen = peg#([минута минуты минут])# {}
void Word_Hour__S_Nom_S_Gen_P_Gen = peg#([час часа часов])# {}"
                },
                {
                    "ner.doctor",
                    @"
using ActiveBC.PhraseRuleEngine.Benchmarking.Helpers;

string Doctor = peg#($Therapeutic:d_0|$Chirurgeon:d_1|$Oculist:d_2|$Otolaryngologist:d_3|$Urologist:d_4|$WomanGynecologist:d_5|$ManGynecologist:d_6|$Gynecologist:d_7|$Neurologist:d_8|$Traumatologist:d_9|$Dermatologist:d_10|$Gastroenterologist:d_11|$Cardiologist:d_12|$Endocrinologist:d_13|$TeethCleaning:d_14|$DentistTherapist:d_15|$DentistSurgeon:d_16|$DentistOrthodontist:d_17|$Dentist:d_18)# { return Pick.OneOf(d_0, d_1, d_2, d_3, d_4, d_5, d_6, d_7, d_8, d_9, d_10, d_11, d_12, d_13, d_14, d_15, d_16, d_17, d_18); }
string Therapeutic = peg#(терапевт~)# { return ""терапевту""; }
string Chirurgeon = peg#(хирург~)# { return ""хирургу""; }
string Oculist = peg#([окулист~ глазн~ офтальмолог~])# { return ""офтальмологу""; }
string Otolaryngologist = peg#([отоларинголог~ лор~ ухогорл~ горлонос~ горлунос~]|ухо горло нос|ухо горло носу|горл~ нос~)# { return ""отоларингологу""; }
string Urologist = peg#(уролог~)# { return ""урологу""; }
string WomanGynecologist = peg#(женщин~ гинеколог~|гинеколог~ женщин~|[женщине-гинекологу женщина-гинеколог гинеколог-женщина гинекологу-женщине])# { return ""женщине гинекологу""; }
string ManGynecologist = peg#(мужчин~ гинеколог~|гинеколог~ мужчин~|[мужчине-гинекологу мужчина-гинеколог гинеколог-мужчина гинекологу-мужчине])# { return ""мужчине гинекологу""; }
string Gynecologist = peg#([гинеколог~ женск~])# { return ""гинекологу""; }
string Neurologist = peg#(невролог~)# { return ""неврологу""; }
string Traumatologist = peg#(травматолог~)# { return ""травматологу""; }
string Dermatologist = peg#([кожн~ дерматолог~])# { return ""дерматологу""; }
string Gastroenterologist = peg#(гастроэнтеролог~)# { return ""гастроэнтерологу""; }
string Cardiologist = peg#([кардиолог~ сердечн~])# { return ""кардиологу""; }
string Endocrinologist = peg#(эндокринолог~)# { return ""эндокринологу""; }
string TeethCleaning = peg#(на? гигиеническ~? [очистк~ чистк~] зубов?|[почисти~ очисти~] зубы|зубы [почисти~ очисти~])# { return ""специалисту по гигиенической чистке зубов""; }
string DentistTherapist = peg#(стоматолог~ терапевт~|[стоматолог-терапевт стоматолог-терапевту стоматологу-терапевту])# { return ""стоматологу-терапевту""; }
string DentistSurgeon = peg#(стоматолог~ хирург~|[стоматолог-хирург стоматолог-хирургу стоматологу-хирургу])# { return ""стоматологу-хирургу""; }
string DentistOrthodontist = peg#(стоматолог~? ортодонт~|[стоматолог-ортодонт стоматолог-ортодонту стоматологу-ортодонту])# { return ""стоматологу-ортодонту""; }
string Dentist = peg#([стоматолог~ дантист~ зубно~])# { return ""стоматологу""; }"
                },
            };

            public static readonly Dictionary<string, string[]> PhrasesByRule = new Dictionary<string, string[]>()
            {
                {
                    "ner.time.Expression",
                    new[]
                    {
                        "за полчаса",
                        "через пол часа",
                        "за полтора часа",
                        "через один с половиной час",
                        "за два с половиной часа",
                        "через пять с половиной часов",
                        "за девять с половиной часов",
                        "через двенадцать с половиной часов",
                        "за час ноль две минуты",
                        "через один час ноль девять минут",
                        "за два часа пятнадцать минут",
                        "через три часа двадцать минут",
                        "за пять часов сорок одну минута",
                        "через двенадцать часов пятьдесят девять минут",
                        "за час ноль пять",
                        "через два ноль две",
                        "за пять пятнадцать",
                        "через семь тридцать пять",
                        "за час",
                        "через два часа",
                        "за пять часов",
                        "через двенадцать часов",
                        "через две минуты",
                        "за пять минут",
                        "через тридцать минут",
                        "за пятьдесят пять минут",
                    }
                },
                {
                    "ner.doctor.Doctor",
                    new[]
                    {
                        "терапевт",
                        "терапевту",
                        "хирург",
                        "хирургу",
                        "окулист",
                        "окулисту",
                        "глазной",
                        "глазному",
                        "офтальмолог",
                        "офтальмологу",
                        "отоларинголог",
                        "отоларингологу",
                        "лор",
                        "лору",
                        "ухогорлонос",
                        "ухогорлоносу",
                        "горлонос",
                        "горлоносу",
                        "горлунос",
                        "горлуносу",
                        "ухо горло нос",
                        "ухо горло носу",
                        "горлу носу",
                        "горлу нос",
                        "горло носу",
                        "горло нос",
                        "уролог",
                        "урологу",
                        "гинеколог",
                        "гинекологу",
                        "женский",
                        "женскому",
                        "женщина гинеколог",
                        "женщине гинекологу",
                        "гинеколог женщина",
                        "гинекологу женщине",
                        "женщина-гинеколог",
                        "женщине-гинекологу",
                        "гинеколог-женщина",
                        "гинекологу-женщине",
                        "мужчина гинеколог",
                        "мужчине гинекологу",
                        "гинеколог мужчина",
                        "гинекологу мужчине",
                        "мужчина-гинеколог",
                        "мужчине-гинекологу",
                        "гинеколог-мужчина",
                        "гинекологу-мужчине",
                        "невролог",
                        "неврологу",
                        "травматолог",
                        "травматологу",
                        "кожный",
                        "кожному",
                        "кожник",
                        "кожнику",
                        "гастроэнтеролог",
                        "гастроэнтерологу",
                        "кардиолог",
                        "кардиологу",
                        "эндокринолог",
                        "эндокринологу",
                        "стоматолог",
                        "стоматологу",
                        "дантист",
                        "дантисту",
                        "зубной",
                        "зубному",
                        "на гигиеническую чистку зубов",
                        "на гигиеническую очистку зубов",
                        "гигиеническую чистку зубов",
                        "гигиеническую очистку зубов",
                        "на чистку зубов",
                        "на очистку зубов",
                        "чистку зубов",
                        "очистку зубов",
                        "на гигиеническую чистку",
                        "на гигиеническую очистку",
                        "гигиеническая чистка зубов",
                        "гигиеническая очистка зубов",
                        "чистка зубов",
                        "очистка зубов",
                        "гигиеническая чистка",
                        "гигиеническая очистка",
                        "почистить зубы",
                        "очистить зубы",
                        "почистили зубы",
                        "очистили зубы",
                        "зубы почистить",
                        "зубы очистить",
                        "зубы почистили",
                        "зубы очистили",
                        "стоматолог терапевт",
                        "стоматолог терапевту",
                        "стоматологу терапевту",
                        "стоматолог-терапевт",
                        "стоматолог-терапевту",
                        "стоматологу-терапевту",
                        "стоматолог хирург",
                        "стоматолог хирургу",
                        "стоматологу хирургу",
                        "стоматолог-хирург",
                        "стоматолог-хирургу",
                        "стоматологу-хирургу",
                        "ортодонт",
                        "стоматолог ортодонт",
                        "ортодонту",
                        "стоматолог ортодонту",
                        "стоматологу ортодонту",
                        "стоматолог-ортодонт",
                        "стоматолог-ортодонту",
                        "стоматологу-ортодонту",
                    }
                },
            };
        }
    }
}