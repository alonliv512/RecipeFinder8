using System.Net.Mail;

namespace Model
{
    // מחלקה שבודקת אם הנתונים של משתמש תקינים.
    // נמצאת בשכבת Model, ולכן גם התצוגה וגם ה-API יכולים להשתמש בה.
    public static class UserValidator
    {
        // הכללים במקום אחד: כדי לשנות כלל, משנים רק כאן
        public const int UsernameMinLength = 2;
        public const int PasswordMinLength = 8;

        // בדיקת שם משתמש. מחזירה הודעת שגיאה, או מחרוזת ריקה אם הכול תקין
        public static string ValidateUsername(string? username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return "יש להקליד שם משתמש.";
            }

            string value = username.Trim();
            if (value.Length < UsernameMinLength)
            {
                return $"שם המשתמש צריך להכיל לפחות {UsernameMinLength} תווים.";
            }

            return "";
        }

        // בדיקת אימייל. מחזירה הודעת שגיאה, או מחרוזת ריקה אם הכול תקין
        public static string ValidateEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return "יש להקליד כתובת אימייל.";
            }

            string value = email.Trim();

            try
            {
                var addr = new MailAddress(value);
                if (addr.Address != value)
                {
                    return "כתובת האימייל לא תקינה.";
                }
            }
            catch
            {
                return "כתובת האימייל לא תקינה.";
            }

            // MailAddress מקבל גם "user@localhost", לכן נדרוש גם נקודה אחרי ה-@
            int atIndex = value.IndexOf('@');
            if (!value.Substring(atIndex + 1).Contains('.'))
            {
                return "כתובת האימייל לא תקינה.";
            }

            return "";
        }

        // בדיקת סיסמה. מחזירה הודעת שגיאה, או מחרוזת ריקה אם הכול תקין
        public static string ValidatePassword(string? password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return "יש להקליד סיסמה.";
            }

            if (password.Length < PasswordMinLength)
            {
                return $"הסיסמה צריכה להכיל לפחות {PasswordMinLength} תווים.";
            }

            bool hasLetter = false;
            bool hasDigit = false;
            foreach (char c in password)
            {
                if (char.IsLetter(c))
                {
                    hasLetter = true;
                }
                else if (char.IsDigit(c))
                {
                    hasDigit = true;
                }
            }

            if (!hasLetter || !hasDigit)
            {
                return "הסיסמה צריכה לכלול גם אותיות וגם ספרות.";
            }

            return "";
        }

        // בודק משתמש שלם ומחזיר רשימה של כל השגיאות שנמצאו (רשימה ריקה = הכול תקין)
        public static List<string> Validate(User? user)
        {
            List<string> errors = new List<string>();

            if (user == null)
            {
                errors.Add("לא התקבלו נתוני משתמש.");
                return errors;
            }

            string usernameError = ValidateUsername(user.Username);
            if (usernameError != "")
            {
                errors.Add(usernameError);
            }

            string emailError = ValidateEmail(user.Email);
            if (emailError != "")
            {
                errors.Add(emailError);
            }

            string passwordError = ValidatePassword(user.PasswordHash);
            if (passwordError != "")
            {
                errors.Add(passwordError);
            }

            return errors;
        }
    }
}