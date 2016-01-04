using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker
{
    //Klasa opisująca protokół pomiędzy aplikacjami
    //Nagłówki wysyłane są na początku każdej wiadomości
    static class Protocol
    {
        public static string LOGIN = "#LogIn"; //Wysyłane przez klienta do banku podczas żądania zalogowania
        public static string REGISTRATION = "#Registration"; //Wysyłane przez klienta do banku podczas żądanie rejestracji w systemie
        public static string LOGINVERIFY = "#LogInVerify"; //Wysyłane przez bank do klienta - wynik weryfikacji logowania
        public static string REGISTRATIONVERIFY = "#RegistrationVerify";// Wysyłane przez bank do klienta - wynik rejestracji
    }
}
