using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RegexExample
{
    class RegexFunctions
    {
        public bool EmailRegex(string email, string type)
        {
            string pattern;
            
            if (type == "lenient")
            {
               pattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

            }
            if (type == "strict")
            {
                pattern = @"^(([^<>()[\]\\.,;:\s@\""]+"
                   + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                   + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                   + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                   + @"[a-zA-Z]{2,}))$";

            }
            else
            {
                // This will catch 80% of the stuff, but isn't the most robust
                pattern = @"[a-zA-Z0-9_\-]+@([a-zA-Z0-9_\-]+\.)+(com|org|edu|nz|au)";
            }
            Regex check = new Regex(pattern);

            bool isMatch = check.IsMatch(email);
            return isMatch;


        }

    }
}
