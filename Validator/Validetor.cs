using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Project_module_4.Validator
{
    public static class Validetor
    {
        public static class Validator
        {
            public static bool IsValid(string input)
            {
                if (!Regex.IsMatch(input, @"^\d{4}$"))
                {
                    return false;
                }
                return true;
            }

            public static bool IsValid(string input, int size)
            {
                if (!Regex.IsMatch(input, @"^\d{" + size + "}$"))
                {
                    return false;
                }
                return true;
            }


            public static bool IsValidName(string name)
            {
                if (name.Length == 0)
                    return false;

                for (int i = 0; i < name.Length; i++)
                {
                    if (!(Char.IsLetter(name[i])) && !(Char.IsWhiteSpace(name[i])))
                    {
                        return false;
                    }

                }

                return true;
            }
        }
    }
}
