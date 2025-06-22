using System;
using System.Linq;

namespace YanAPI.Utils; 
public static class ObjectExtensions {
    public static string ConvertToANSI(this object data, int r, int g, int b) {
        if (data == null)
            return "null";
        try {
            return $"{(char)27}[38;2;{r};{g};{b}m{data}{(char)27}[0m";
        } catch (Exception ex) {
            return $"Error converting to ANSI: {ex.Message}";
        }
    }

    public static string ConvertToANSI(this object data, string hex) {
        if (data == null)
            return "null";

        if (string.IsNullOrWhiteSpace(hex))
            throw new ArgumentException("Invalid hex value.");

        if (hex.StartsWith("#"))
            hex = hex.Substring(1);

        if (hex.Length == 3)
            hex = string.Concat(hex.Select(c => new string(c, 2)));
        else if (hex.Length != 6)
            throw new ArgumentException("Hex value must be 3 or 6 characters long.");

        try {
            return $"{(char)27}[38;2;{Convert.ToInt32(hex.Substring(0, 2), 16)};{Convert.ToInt32(hex.Substring(2, 2), 16)};{Convert.ToInt32(hex.Substring(4, 2), 16)}m{data}{(char)27}[0m";
        } catch (Exception ex) {
            return $"Error converting to ANSI: {ex.Message}";
        }
    }
}
