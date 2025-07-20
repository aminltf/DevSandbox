namespace DevSandbox.Shared.Kernel.Utilities;

public static class ValidationHelpers
{
    public static bool IsValidIranianNationalCode(string nationalCode)
    {
        if (string.IsNullOrWhiteSpace(nationalCode) || nationalCode.Length != 10 || nationalCode.All(c => c == '0'))
            return false;

        var check = Convert.ToInt32(nationalCode[9].ToString());
        var sum = Enumerable.Range(0, 9)
            .Select(i => Convert.ToInt32(nationalCode[i].ToString()) * (10 - i))
            .Sum() % 11;

        return (sum < 2 && check == sum) || (sum >= 2 && check == (11 - sum));
    }

    public static bool IsValidIranianMobile(string mobileNumber)
    {
        if (string.IsNullOrWhiteSpace(mobileNumber))
            return false;

        if (!System.Text.RegularExpressions.Regex.IsMatch(mobileNumber, @"^09\d{9}$"))
            return false;

        string[] validPrefixes =
        {
            "090", "091", "092", "093", "099"
        };

        return validPrefixes.Any(prefix => mobileNumber.StartsWith(prefix));
    }
}
