using RedisObjNet;
using StackExchange.Redis;

var db = ConnectionMultiplexer.Connect("localhost").GetDatabase();
var redis = new RedisMapper(db);

var paolo = new User
{
    Name = "Paolo",
    Age = 31,
    Email = "paolo.cattaneo@company.com",
    Password = "secret"
};

var clara = new User
{
    Name = "Clara",
    Age = 23,
    Email = "clara.ferro@company.com",
    Password = "asdasda"
};

redis.Set(paolo);
redis.Set(clara);

paolo.Age = 35;
redis.Set(paolo, nameof(User.Age));

var paoloAge = redis.GetInt<User>("Paolo", "Age");
Console.WriteLine($"Paolo age is {paoloAge}");
Console.WriteLine($"Clara mail is {redis.GetString<User>("Clara", nameof(User.Email))}");

class User
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

class Device
{
    [RedisKeyAttribute]
    public string Mac { get; set; }
    [RedisValueAttribute]
    public string Name { get; set; }
    [RedisValueAttribute]
    public int Battery { get; set; }
}
