namespace WebApplication.Infrastructure.Extensions
{
    public static class RemoveDiacriticsExtensions
    {
        public static string RemoveDiacritics(this string text)
        {
            string newText = "";

            foreach(var t in text)
            {
                switch(t)
                {
                    case 'ą': newText += "a"; break;
                    case 'ć': newText += "c"; break;
                    case 'ę': newText += "e"; break;
                    case 'ł': newText += "l"; break;
                    case 'ń': newText += "n"; break;
                    case 'ó': newText += "o"; break;
                    case 'ś': newText += "s"; break;
                    case 'ź': newText += "z"; break;
                    case 'ż': newText += "z"; break;

                    case 'Ą': newText += "A"; break;
                    case 'Ć': newText += "C"; break;
                    case 'Ę': newText += "E"; break;
                    case 'Ł': newText += "L"; break;
                    case 'Ń': newText += "N"; break;
                    case 'Ó': newText += "O"; break;
                    case 'Ś': newText += "S"; break;
                    case 'Ź': newText += "Z"; break;
                    case 'Ż': newText += "Z"; break;

                    default: newText += t; break;
                }
            }

            return newText;
        }
    }
}
