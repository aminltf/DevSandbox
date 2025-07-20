using System.ComponentModel.DataAnnotations;

namespace DevSandbox.Shared.Kernel.Extensions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        var type = enumValue.GetType();
        var name = Enum.GetName(type, enumValue);
        if (name == null)
            return "نامشخص";

        var field = type.GetField(name);
        var displayAttribute = field?.GetCustomAttributes(typeof(DisplayAttribute), false)
                                    .FirstOrDefault() as DisplayAttribute;
        return displayAttribute?.Name ?? name;
    }
}
