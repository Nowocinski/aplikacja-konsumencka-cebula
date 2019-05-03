using System.Text;

namespace WebApplication.Infrastructure.Extensions
{
    public static class RemoveDiacriticsExtensions
    {
        public static string RemoveDiacritics(this string text)
        {
            StringBuilder newText = new StringBuilder(text.Length);
            foreach (var t in text)
            {
                switch(t)
                {
                    case 'ą': newText.Append("a"); break;
                    case 'ć': newText.Append("c"); break;
                    case 'ę': newText.Append("e"); break;
                    case 'ł': newText.Append("l"); break;
                    case 'ń': newText.Append("n"); break;
                    case 'ó': newText.Append("o"); break;
                    case 'ś': newText.Append("s"); break;
                    case 'ź': newText.Append("z"); break;
                    case 'ż': newText.Append("z"); break;

                    case 'Ą': newText.Append("A"); break;
                    case 'Ć': newText.Append("C"); break;
                    case 'Ę': newText.Append("E"); break;
                    case 'Ł': newText.Append("L"); break;
                    case 'Ń': newText.Append("N"); break;
                    case 'Ó': newText.Append("O"); break;
                    case 'Ś': newText.Append("S"); break;
                    case 'Ź': newText.Append("Z"); break;
                    case 'Ż': newText.Append("Z"); break;

                    default: newText.Append(t); break;
                }
            }

            return newText.ToString();
        }
    }
}
