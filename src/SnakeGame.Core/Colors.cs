using Microsoft.Xna.Framework;

namespace SnakeGame.Core;

/*
https://coolors.co/palette/8ecae6-219ebc-023047-ffb703-fb8500
Color palette values
#8ECAE6
#219EBC
#023047
#FFB703
#FB8500
*/

public static class Colors
{
    public static Color DefaultBackgroundColor = Color.FromNonPremultiplied(0x02, 0x30, 0x47, 255);
    public static Color DefaultTextColor = Color.White;
    public static Color DefaultTextShadowColor = Color.Black;
    public static Color ScoreTimeColor = Color.FromNonPremultiplied(0xFF, 0xB7, 0x03, 255);
    public static Color ScoreMultiplicatorColor = Color.FromNonPremultiplied(0xFF, 0xB7, 0x03, 255);
    public static Color PlayerCloseTintColor = new Color(255, 255, 255, 245);
}