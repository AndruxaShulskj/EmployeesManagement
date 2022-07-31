namespace EmployeesManagement.Business.Tests;

public class TestBase<T> where T : class
{
    protected AutoMocker AutoMocker { get; }

    public TestBase()
    {
        AutoMocker = new AutoMocker();
    }

    protected T CreateTestSubject()
    {
        return AutoMocker.CreateInstance<T>();
    }
}