using System.Collections.Generic;

namespace TrelloClone.Core.Constants
{
    public class UserDefaultAvatarColorSchemes
    {
        public static List<string> colorSchemes { get; private set; } = new List<string> { 
            ColorSchemes.LightBlue,
            ColorSchemes.LightBrown,
            ColorSchemes.LightGray,
            ColorSchemes.LightGreen,
            ColorSchemes.LightOrange,
            ColorSchemes.LightPurple
        };
    }
}
