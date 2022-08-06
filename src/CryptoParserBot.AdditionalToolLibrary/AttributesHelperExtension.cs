using System.ComponentModel;

namespace CryptoParserBot.AdditionalToolLibrary;

public static class AttributesHelperExtension
{
    public static string ToDescription(this Enum value)
    {
        var da = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

        return da.Length > 0 ? da[0].Description : value.ToString();
    }
    
    public static string[] GetStringValues<T>(T obj)
        where T : class
    {
        var type = obj.GetType();
        var props = type.GetProperties();
        var result = new string[props.Length];

        for (var i = 0; i < props.Length; i++)
        {
            var value = props[i].GetValue(obj, null)?.ToString();
            if(value is not null)
                result[i] = value;
        }
       
        return result;
    }
}
    
