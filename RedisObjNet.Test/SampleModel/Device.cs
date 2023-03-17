using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisObjNet.Test.SampleModel;

internal class Device
{
    [RedisKeyAttribute]
    public string Mac { get; set; }
    [RedisValueAttribute]
    public string Name { get; set; }
    [RedisValueAttribute]
    public int Battery { get; set; }
}
