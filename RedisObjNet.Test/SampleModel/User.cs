using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisObjNet.Test.SampleModel;

internal class User
{
    [RedisKeyAttribute]
    public string Name { get; set; }
    [RedisValueAttribute]
    public int Age { get; set; }
    [RedisValueAttribute]
    public string Email { get; set; }
    [RedisValueAttribute]
    public string Password { get; set; }
}
