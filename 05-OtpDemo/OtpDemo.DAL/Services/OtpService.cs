using Microsoft.EntityFrameworkCore;
using OtpDemo.BLL.Entities;
using OtpDemo.BLL.Interfaces.Services;
using OtpDemo.DAL.Data;

namespace OtpDemo.DAL.Services;

public class OtpService : IOtpService
{
    private readonly IdentityAppDbContext _db;
    private readonly ISmsSenderService _smsSender;

    public OtpService(IdentityAppDbContext db, ISmsSenderService smsSender)
    {
        _db = db;
        _smsSender = smsSender;
    }

    public async Task GenerateAndSendOtpAsync(string phoneNumber)
    {
        var code = new Random().Next(100000, 999999).ToString(); // 6-digit code
        var otp = new OtpCode
        {
            Id = Guid.NewGuid(),
            Code = code,
            PhoneNumber = phoneNumber,
            ExpiresAt = DateTime.UtcNow.AddMinutes(3),
            IsUsed = false
        };
        await _db.OtpCodes.AddAsync(otp);
        await _db.SaveChangesAsync();
        await _smsSender.SendSmsAsync(phoneNumber, $"Your OTP code is: {code}");
    }

    public async Task<bool> ValidateOtpAsync(string phoneNumber, string code)
    {
        var otp = await _db.OtpCodes
            .Where(x => x.PhoneNumber == phoneNumber && x.Code == code && !x.IsUsed && x.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(x => x.ExpiresAt)
            .FirstOrDefaultAsync();

        if (otp == null) return false;
        otp.IsUsed = true;
        await _db.SaveChangesAsync();
        return true;
    }
}
