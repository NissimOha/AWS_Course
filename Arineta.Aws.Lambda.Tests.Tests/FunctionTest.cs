using Xunit;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.SNSEvents;
using Arineta.Aws.Lambda.App;

namespace Arineta.Aws.Lambda.Tests;

public class FunctionTest
{
    [Fact]
    public async Task TestSQSEventLambdaFunction()
    {
        var topic = "AddTopic";
        var message = "User";
        var subject = "AddSubject";

        var expected = $"Processed record: Topic: {topic}, Subcect: {subject}, Message: {message}";
        var actual = string.Empty;

        var snsEvent = new SNSEvent
        {
            Records = new List<SNSEvent.SNSRecord>
            {
                new SNSEvent.SNSRecord
                {
                    Sns = new SNSEvent.SNSMessage()
                    {
                        TopicArn = topic,
                        Message = message,
                        Subject = subject
                    }
                }
            }
        };

        var logger = new TestLambdaLogger();
        var context = new TestLambdaContext
        {
            Logger = logger
        };

        var function = new Function();
        await function.FunctionHandler(snsEvent, context);

        actual = logger.Buffer.ToString();

        Assert.Contains(expected, actual);
    }
}