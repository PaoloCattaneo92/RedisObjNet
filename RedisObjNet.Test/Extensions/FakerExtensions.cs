using RedisObjNet.Test.SampleModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisObjNet.Test;

internal static class FakerExtensions
{
    private static readonly Random random = new();

    internal static string Mac()
    {
        var macBytes = new byte[6];
        random.NextBytes(macBytes);
        macBytes[0] = (byte)(macBytes[0] & 0xFE); // clear the multicast bit
        macBytes[0] = (byte)(macBytes[0] | 0x02); // set the local assignment bit
        var macAddress = string.Concat(macBytes.Select(b => b.ToString("X2")));
        return string.Join(":", Enumerable.Range(0, 6).Select(i => macAddress.Substring(i * 2, 2)));
    }

    internal static User Fake(this User user)
    {
        user.Name = Faker.NameFaker.Name();
        user.Age = Faker.NumberFaker.Number(18, 99);
        user.Email = Faker.InternetFaker.Email();
        user.Password = "passsword" + DateTime.Now.Ticks;
        return user;
    }

    internal static Device Fake(this Device device)
    {
        device.Mac = FakerExtensions.Mac();
        device.Name = "DeviceOf" + Faker.NameFaker.FirstName();
        device.Battery = Faker.NumberFaker.Number(5, 100);
        return device;
    }
}
