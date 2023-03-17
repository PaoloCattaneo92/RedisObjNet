using RedisObjNet.Test.SampleModel;

namespace RedisObjNet.Test;

public class TestUser
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestSave()
    {
        // Arrange
        var user = new User().Fake();

        // Act
        Repository.Redis.Set(user);

        // Assert
        var age = Repository.Redis.GetInt<User>(user.Name, nameof(User.Age));
        Assert.That(age, Is.Not.Null);
        Assert.That(age, Is.EqualTo(user.Age));
    }
}