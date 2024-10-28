
using System;

public class TestClass : IEquatable<TestClass>
{
    public int Id;
    public string Name;

    public bool Equals(TestClass other)
    {
        return Id.Equals(other.Id) && Name.Equals(other.Name);
    }

    public TestClass(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
