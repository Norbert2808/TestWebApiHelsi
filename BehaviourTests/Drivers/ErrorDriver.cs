using FluentAssertions;

namespace BehaviourTests.Drivers;

public class ErrorDriver
{
    private readonly List<ArgumentException> _argumentExceptions = new ();

    public bool TryExecuteAsync<T>(Func<Task<T>> action, out T result)
    {
        try
        {
            result = action().GetAwaiter().GetResult();
            return true;
        }
        catch (ArgumentException ex)
        {
            _argumentExceptions.Add(ex);

            result = default!;
            return false;
        }
    }
    
    public void AssertArgumentExceptionRaised(string parameterName, string errorMessage)
    {
        _argumentExceptions.Should().NotBeEmpty($"ArgumentException expected with parameter name {parameterName} and message {errorMessage}");

        var exceptionRaised = _argumentExceptions.FirstOrDefault(it => it.Message == errorMessage);
        exceptionRaised.Should().NotBeNull();
        exceptionRaised.ParamName.Should().Be(parameterName);
        _argumentExceptions.Remove(exceptionRaised);
    }

    public void AssertNoUnexpectedExceptionsRaised()
    {
        if (_argumentExceptions.Count > 0)
        {
            var unexpectedArgumentException = _argumentExceptions.First();
            _argumentExceptions.Should()
                .BeEmpty($"ArgumentException found with with parameter name {unexpectedArgumentException.ParamName} and message {unexpectedArgumentException.Message}");
        }
    }
}