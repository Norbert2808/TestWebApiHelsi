using BehaviourTests.Drivers;

namespace BehaviourTests.Steps;

[Binding]
public class ErrorSteps
{
    private readonly ErrorDriver _errorDriver;

    public ErrorSteps(ErrorDriver errorDriver)
    {
        _errorDriver = errorDriver;
    }
    
    [Then(@"a ArgumentException should be thrown with parameter name '(.*)' and message ""(.*)""")]
    public void ThenAArgumentExceptionShouldBeThrownWithParameterNameAndMessage(string parameterName, string errorMessage)
    {
        _errorDriver.AssertArgumentExceptionRaised(parameterName, errorMessage);
    }

    [Then(@"exception shouldn't be thrown")]
    public void ThenExceptionShouldnTBeThrown()
    {
        _errorDriver.AssertNoUnexpectedExceptionsRaised();
    }
}