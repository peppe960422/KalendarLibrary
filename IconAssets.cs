using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalendarLibrary
{
    static class IconAssets
    {

        public const string DownArrow = "R0lGODlhGQAZAPMAADOA/zOq/zOAzACAzABVzACA/wBV/zNVzDOqzGaA/zNV/wAAAAAAAAAAAAAAAAAAACH/C05FVFNDQVBFMi4wAwEBAAAh+QQBAAALACwAAAAAGQAZAAAIhQAXCBxIsKDBgwgTKlzIsKHDhxAjLgAQQACAAQMIZMxIoGPHhAAKXAQwcoABjBotKgRgccDFAgcGCND4UWGAAC4HHChgciMBBAsRJBBQgKhMnQU0Hmg4kgAAjT4dhjQ6QAFHiESNpiQAkSVMmSclLsipUewCAQO+mhU4YK3bt3Djyp0rNiAAOw==";
        public const string UpArrow = "R0lGODlhGQAZAPMAADOAzABVzACAzACA/zOA/wBV/2aq/zOq/2aA/zOqzDNV/wAAAAAAAAAAAAAAAAAAACH/C05FVFNDQVBFMi4wAwEBAAAh+QQBAAALACwAAAAAGQAZAAAIggAXCBxIsKDBgwgTKlzIsKHDhxAjIgQgkWAAAQEGUKyIUWMAjgEACBgggMBGhwEuCiggkoAAAw9TErjoUiPMhhdzimSpsSGAlCVXtix5AMHCmRiTjgxqIMHCnAIUlAypcQCBAQoB/Eyq0qVXrQmBZvzpkieAAwQIVFzLtq3bt3DjBgQAOw==";
        public const string Floppy = "R0lGODlhGQAZAPcAACkANQAATBoGUBoXWC8ARyUASzsASjUbUzIcUzEdVDAeVTccVCohUioiVS0iVS8oXjAgVTYkVjUlVjojVD8hVzwjVTgkVTMmXDkkWS0lYzEqYDIpYDUrYjMqZzEtZjMqajQqaDo0bzw1fz02f0U7ZUo7ZUs/Z0w/Z00xck44dk85eFEydlE3dlE1elU3eFE3fFM7e1Y5elY8elI5flY7fVM+flo8fUlAaGBYeOwSC+kWIuYXK+QYNOEZPL4eQ647cK89c6o9d609dac+e6U+fqk9ebE8cd8aQ90bStocUdYeX9gdWNMfZdEgbNEgcsNGYN9Tbd1UcNtUdNdVfNlUeOdTYeJTZeFTaUA5gEM8gVI7gVc+gFI+hFo+gElChFRAgldAg1FChlVChVREh1pBg1hChVpDhVlEhVBGi1VFildHjFpGilpGiFVKjltIi1tKjllJj1tMj1JKkFdLkFZMklVNk1ZPlVlLkVtNkVlNk1pPlllOlVtOlVRPmEtRmk1RmU9QmUxRmlZQl1xQllZRmlZSmVVTnVdVnlpSm1pRmVxSmFxUmllTnFlWn1tVn11WnXlDgXxKkG9mh3Bphn1qhnZriXNtjnlsin1tildXoVVVoFtXoVdYoVZYpFpZol1ZollapFpbp1xapVhcp15cpV1dqV5hrl9irWBirKI+gaE+hLo3gqBAh4V9l4R9m4Z/mYx5mdZVgNRVhNJWiNBWjMFaltBXkIiBm46FqZqKppaLsaKZtJiZw5aay5WZy7SnwLSkxLumxLeuxrepyLmrybmty7uxyb61ysC3zsO5zsW8z8a318S70se+0ca91Mq/1sq72s262su928y/3crA0s3F18vD2c7F283B39DA4dPD49XG5dbH5dbH5trF4dnF49rF4t3H4tvH5dbL4dXK4NfM4tbM4dfK59fI59fO5NjO49jJ59jK59nP5NjJ6NnL6NnN6dvM6dzP69vP7NvR5tzS59/V6d/W6uHY6+Pa7+LZ7eLZ7uTb7+Tb8OXc8AAAACH/C05FVFNDQVBFMi4wAwEBAAAh+QQBAAD/ACwAAAAAGQAZAAAI/wD/CRxIsKDBgwgTHuRlahSHCgtKWOu3b1++dLlg/ar2wZdBTqc6bZhw4MQ1f/r0+UtmAAAlaiBMeSTIqZRIkias4bt3Tx8yCgQwNesQ6lQvgplKadpgAYGJZ/bq1bOHDEOBS8o8gAqFiiAjUYY2REhgwhm9du3qGbsgoJKyDps8lSJYaBMhDRIU3GCmrly5dsIGBJCUrEOjTaQICtrU54EECCSQlSNHbtwzV5Z2HcvAyNEnxYwSeWDgoMGkV61Sv3p16xaOEIgWfR5IJ1EiOV5yYxHBW8QILCNEZKlj+xHBOXr49AnkJ9Cf54EAPQdEXdAePcYHtslTB9IPI0CECP8pUmTIECKpVLGKVOfOIoJq7qDxkUPHDh49jiBJskQJkyZOrELHHYoQlAYcYTxRhRVXQBGFFFRMEYsss9BiSy1vqDEIQWOswUUw4XgDTojeiAPOieJ8I040b6TBB0FirKFFMfPAEw888MgDT43v4ChPNm6UgQdBX5QxQzHxvHPOO+ycw06T8LDzTjzTsAFGHATVAMYLSJ7j5TnufLkOO+5QacYWb2S5xQvEsIPOm3DGic470pBBgxsEwUDDCsS4sw033QDKjaCBsgNNFzGwQZAKMawwzDrcbCPppJJGuo40NrhwBkG4qIACMNpkI+qopGajzTIysKCLQqy26qpBAQEAOw==";
        public const string ArrowLeft = "R0lGODlhGQAZAPMAADOA/zOqzDOq/wCA/2aA/zOAzACAzABVzDNVzDNV/wBV/wAAAAAAAAAAAAAAAAAAACH/C05FVFNDQVBFMi4wAwEBAAAh+QQBAAALACwAAAAAGQAZAAAIjAAXCBxIsKDBgwgTKlzIsKFDhgAAPCwYQACAAQImEiQgoACAAhoXRCxgAEDJiR8HGBhg0uTEAgM8koxowCHJmAMODEBgwMABhyURqDQ5gGVNhit79jxAkufPhgiYHkjQU4EBq0CvKuVJ8inQAgd0hvWpceyBpV4nnjUbUiCCAFLbEgSQVq7du3jzagwIADs=";
        public const string ArrowRight = "R0lGODlhGQAZAPMAAABVzDOAzDOA/wCAzACA/zNV/wBV/2aq/zOq/zOqzGaA/wAAAAAAAAAAAAAAAAAAACH/C05FVFNDQVBFMi4wAwEBAAAh+QQBAAALACwAAAAAGQAZAAAIhgAXCBxIsKDBgwgTKlzIsKFDgQACPEQYEYAAiRMHDgCwEQCAjBA3ivSYkQDHkx4HTAwwoMAAAS9HOkyJckAAjg5hwjTA0sAAkxgXDhDJ8iKBAAQcGjA6wOfQiA5ZEniJVICAiQQuwjxqc+VPqgcOZERg9QACkAKNJlCAdsHFtnDjyp1L12BAADs=";
        public const string CreateNew = "iVBORw0KGgoAAAANSUhEUgAAADAAAAAwCAYAAABXAvmHAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAACxIAAAsSAdLdfvwAAAJ9SURBVGhD7ZZPaxNBGIeroN/Bg1e/QC9eRLLTznixKdJ4rH8KfoR6zEWxVTCeFC/uNv8KPejBgyApgnhp8eRBBY0fQPBQzFZ7GpkEFZ/NZndnMsHDPvBA2mz293vh3WTm5kpK/PFg/0jPUuY7wwDfMt8ZBviW+c4wwLfMd4YBvmW+MwzwLfOdYYBvme8MA3zLfGcY4FvmO8MA46n1d0P5d1F533KAcTDAt8x3hgG+Zb4zDDByBbgaeeV9ywHGwQDfMt8ZBviW+c4wwMgV4GrklfctBxgHA3zLfGcY4FvmO8MAI1eAq5Hmmfp7vbbzVd/f+5m4538/wLlGX2+8ORxe39iL9Xqvp1ef3tbV7etaNqtDzWsRyu5CpC7Xdmon2cUKlnf1Zu+Vvti9okWkJhvKzwtP1CX2KQwL2NrY/6FvPH+ULJplqO7V6/Xj7JUbFrHVqvwf5SZ75YZFbDRrkyw1kvD93wahXGa3XLBMUc0DuzRh5wnf/zuA+mL1YLNQUc23DcvYDDAa4kKN/TJhoaKuPruVKGI9QKTa7JcJCxV1qXttYuEs/h1CfmS/VCrtwVHQibWrIlqe3gCh+s6eqVTa8TeWsXGaAwShPGDPVCqdQZ9lbBRb01uhIFIf2DOVSjt+wTI2iub0HmIRyhZ7phK04zssY6NovUwWsRwgiBZX2DOV8634LMvYeaBFdDVRpugAQaT6xX7ItD5W6QxeJwsVV7R2E4WKDiC2VJUVM5Hbh6eDTvyWhWwUzYfJUnkN1Qa7zRxzJA4ieTdRLlO56XScnjbmVClC9SlZFJprbNZmFsw/nj9hDmbmbGO+280vrNG8Hv1vccVcw8+VlHjiFwr8pthnv0vsAAAAAElFTkSuQmCC";
        public const string Lupe = "R0lGODlhGQAZAPcAAJWTkXx4cnNvZ397dpybmXd1cmFeWWFteWJ+mWKDpGJ8lWFqcmFdWIKBf2FdV2FxgWObzGOv7GGz9WC1+GGz9GOt6mOXw2FrdGpnYmSu6mC2+me3+Hy/+ITC+Hm++GS3+GG2+mSp4mJzhGxoY2JxgWK2+YLC96vW+q3X+q3W+qfU+nm+92K3+mOo4GFobo6Ni2FeWmOczGG3+rDZ+o3G+Gm39mG09my59pbL+W259mK3+WKQuGFcVWSv7GO2+Gy49nW891+09mO19mS19mK09mO2+WFlaK2rqmJ+mmS19WS192S292Sx72JxgpGPi3RvaGS3+WJ1i4mGgWJ8lmS09GSw7WFwf5WSj2St6WS2+GOm3GFjY2OVwWS4+mKJrWVgWWSp4WS3+mOh1WFkZZuZmGSo32Oh1GJrdWFeW21pZWFnbmKPtmKJrGFjZWJfXGFhYX18fGFxgmJ2i2Jwf2FiY39/f1daXFRfZa2sqlNfZDZGTlBcZE5bYjZHT1BcY05aYTdHT3J7gHB6fwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACH/C05FVFNDQVBFMi4wAwEBAAAh+QQBAACDACwAAAAAGQAZAAAI/wAHCRxIsKDBgwgTKhwIIICAAQQWHixg4ACCBAoWMGggcaCDBxAiSJhAoYKFCxg6OlCQQcMGDh08fAARQsSIhQVIZChh4gSKFChUrGDRwsULhTBiyFgxg0YNGzdwoMihYwePhAAO9PDxA0gQIUOI2FhRo0gLI0cQBkCSRIkQIkPixrUhZAmTJk4QPkkARa5fv0miSEE4YAoVJX/9Lqli5QpCAguwZEksF4qWLQoZcOlCOe4HL18UNrgAJgzlMGLGkFmIQUQZKFCWDFkC5YOZM2g6plGzpgWTJFW0sGnj5g2cji94GIkjZw6dL6vr2LnTcRAeJ1IcE8yjZ0/1hHz6+BP5jjD8ePIG/wAKhN6goPbwyQcEADs=";
        public const string ObjImg = "R0lGODlhMAAwAPYAAIPMxWfAuFS4r4TMxVa5sE22rIq+7XHEvG7Duz6k9Bx71V+9tE62rFe5sV+9tWO/tle6sGC9tI3I+CGW8xl20lKf5G/Du1C3rWC+tWS/t3DEvCeY8hl30mCz9SuG2la6sYW871++tDyi9Bt61E+u3U2b42W/t0eytiKW8Bl20y6f3ymF2j6rxH657GfAt0u1ryaZ6xt402e6sp6ooteQjtyOi6ekn2+7szWl0kua5G+tpLGZlOyHhPSFg++HhcCXknmooUWxuCKX8ZWkndeOituMiZufmVyyqC2e4SeD2vetq/OHhfKFg8SQlWeQziSW8Xu27PSRj/OKidt7hJxceBt41O9TUO5TUPF3dfWPjfOFg/OEgsNze6ZLYMQpK8YoKEaX4vBiX/KHhfGJiOCAfrVmY7k5ORl30/B2deKIhPGFg8h3c7BSUMQqKiaD2e5jYOODgMBnZbw4N3Sy6i6I3CWB2SWC2Vmw9Vqx9Vev9SiE2vJ8etNcXPKAftJZWQAAACH/C05FVFNDQVBFMi4wAwEBAAAh+QQBAAB/ACwAAAAAMAAwAAAI/wD/CBxIsKDBgwgTKlzIsKHDhxAjSpxIsaLFixgFAgggQECAARkfEihAsiRJAgoNZDxgsmVJBAcTKMC4oACDBg4ePHAAgUHLCAUlTKBQwaKFAhcwZFjK1MGFlhoIbhjKwaJNpUyzOvBpcmCHCUMpeKA4skHWsxk+tET5B2xYCiAmkgyBVqvLPyLcUtg7QiIAkg/qMn3gkoTbtxRKRDQBWPBSwiZPoDi8d2+KiALmOs7goKWKw4gprICYuQCEzWpLsgAdmkKLhy5IMnAguHPJFzBYV64c4+GAkhdkzKBRw8YNzk9L4mDdmkKOhyR17ODRo3oPHz+AcC0QRAjz3ZUvO/8cQsS6eetFjBwpgIR5cwpJGCpZcr6+dSZNnDz5Dr4ylIRRSGHfgNZNQQV//VFQRUJWXIFFFloQaN0WXHThxRcIJggGQlZ0aEUYYowxIBllmPHFiRjqluBeZ3DoYYdXoJGGGtVtsQYbbaCIYoYJunHQi0C+AUcccuhoJI8JzmEQkEwa6SSS/dGxJJMvOnmkiivWodKUVHZopY5QUmDHlj926eWXJ96BZWVjLmTmmWgKpKZelWnZ0JtWoHkiQXiAtZcbZDKEp55fGJSHHhINqmdIe7ypJx8hCdRHl1/6EWlBkwLppKWXHpQpnF9w2mlCmZ4o6qgL7QEpqqy26uqrsMYGKuusBQUEADs=";
        public const string Calendar = "iVBORw0KGgoAAAANSUhEUgAAADAAAAAwCAYAAABXAvmHAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAAB3RJTUUH4gQTASAcoDNcQgAAAkRJREFUaEPVVIGRhCAMtCd6opTrwZ7s6X4QY8hCTPDknsvMTmQNmN2oy9KIELf3LwD73iPdiOvcCGE9UTW/bu+pkZqOcdEFoFoNVFtmvB6CQwCJcAkg3pOHwyuArlt5FBYFIXA2BYwENtZu0MJAAdjYvQYtXAkofk8zgwQsyw6OdY1vjG1bkdrDx29nzjyvfTydxTnG8C0Bsimbl/drPucvCdAc1fi6WY0fKACd05y2+Lz+0gQ0R+/y3OztCaSNHqwhqsDaJ2EKwEib6tj2RluOanw+x+IfmABGLSA/LDfKa4vnc2Tu5T8UYDvd5nVHe/kPBPic1njN0V7+pgB0VHda8ixec7SXNwWkjR7gn6cE1j4JU4AM/Etw7uPzw1uO9vIdAvDdtd5pjc/5Kd4pwHJUe6exnu+36/t5p4AU6Kg2AY2X+WP+WDsFoNPaBJDXnXuKdwpIgY7K16JyaBQP2RSQNs4MUwDHP03gWJ987wSO6jPng8b9VS75hjinAN5UOTSKh/tavVPAJBOgdSHSKSAfsu9RnHich1zVWx/xvzav8SJve9OpT1VA2jgzTAEcToeK8Qoe7lf1lzx9d+y8ewIysmp8qMofLlW8q56aLHho/nIC9PpwwEPxcOSt+mIt6pFXmk+ZmlcFcOTD5BgbPDjqq5eOYpMaX7pfCUgX0n18KOeqyYuHPsmXzZOYSsDMIAHltRDwC1AFYFDBjDAFJBLHNxNKc6vmScDMKCfRFNCKcuMscAt4vV7VGGeAq/kjqHg2nPEHOXrY5+YxxpkAAAAASUVORK5CYII=";
    }

    public static class ImageBase64Converter
    {
        public static string ImageFileToBase64(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                throw new ArgumentNullException(nameof(imagePath));

            if (!File.Exists(imagePath))
                throw new FileNotFoundException($"File non trovato: {imagePath}");

            using (var image = Image.FromFile(imagePath))
            {

                string extension = Path.GetExtension(imagePath).ToLower();
                ImageFormat format = GetImageFormatFromExtension(extension);

                return ImageToBase64(image, format);
            }
        }


        public static string ImageToBase64(Image image, ImageFormat format = null)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            format ??= ImageFormat.Jpeg;

            using (var ms = new MemoryStream())
            {

                image.Save(ms, format);


                byte[] imageBytes = ms.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);

                return base64String;
            }
        }
        private static string CleanBase64String(string base64String)
        {
            if (base64String.Contains("base64,"))
            {
                // Rimuove il prefisso "data:image/format;base64,"
                int base64Index = base64String.IndexOf("base64,", StringComparison.Ordinal) + 7;
                return base64String[base64Index..];
            }

            return base64String;
        }

        private static ImageFormat GetImageFormatFromExtension(string extension)
        {
            return extension switch
            {
                ".jpg" or ".jpeg" => ImageFormat.Jpeg,
                ".png" => ImageFormat.Png,
                ".bmp" => ImageFormat.Bmp,
                ".gif" => ImageFormat.Gif,
                ".tiff" or ".tif" => ImageFormat.Tiff,
                ".ico" => ImageFormat.Icon,
                _ => ImageFormat.Jpeg // Default
            };
        }

        public static Image Base64ToImage(string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
                throw new ArgumentNullException(nameof(base64String));

            string cleanBase64 = CleanBase64String(base64String);


            byte[] imageBytes = Convert.FromBase64String(cleanBase64);


            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                ms.Write(imageBytes, 0, imageBytes.Length);


                Image image = Image.FromStream(ms, true);
                return new Bitmap(image);
            }
        }

    }
}