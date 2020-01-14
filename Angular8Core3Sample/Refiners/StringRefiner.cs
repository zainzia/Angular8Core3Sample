using System;
using System.Collections.Generic;
using System.Linq;

namespace Angular8Core3Sample.Refiners
{
    public static class StringRefiner
    {

        private static List<char> SpecialChars { get; set; }
        private static List<char> SQLCharsToRemove { get; set; }


        static StringRefiner()
        {
            SpecialChars = @"`~!@#$%^&*|\()_+-=[]{};':"",./<>?".ToCharArray().ToList();
            SQLCharsToRemove= @"()=/"";<>.".ToCharArray().ToList();
        }


        public static string RefineForSQL(string stringForSQL, out bool charsRemoved)
        {
            if (string.IsNullOrEmpty(stringForSQL))
            {
                charsRemoved = false;
                return stringForSQL;
            }

            var charArray = stringForSQL.ToCharArray();
            var charsToRemove = SQLCharsToRemove.Where(x => charArray.Contains(x)).ToList();

            foreach (var character in charsToRemove)
            {
                stringForSQL = stringForSQL.Replace(character.ToString(), "");
            }

            if(stringForSQL.Contains("'"))
            {
                stringForSQL = stringForSQL.Replace("'", "''");
            }

            charsRemoved = charsToRemove.Count() > 0;
            return stringForSQL;
        }


        //public static string RemoveSpecialChars(string stringWithSpecialChars)
        //{

        //}


        //public static string EscapeSpecialChars(string stringWithSpecialChars)
        //{

        //}

    }
}
