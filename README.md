# NuBot

A robotic butler for your organization.

## Getting started

Create a new Console Application and install the `NuBot` package.

```
Install-Package NuBot
```

In your `Main` function, run NuBot,

```csharp
public static void Main(string[] args)
{
    var gitterToken = "<YOUR GITTER AUTHENTICATION TOKEN>";

    new RobotFactory()
        .AddPart<DoIt>()
        .AddPart<Echo>()
        .AddPart<ShipIt>()
        .UseAdapter(new GitterAdapter(gitterToken))
        .RunAsync(CancellationToken.None)
        .GetAwaiter()
        .GetResult();
}
```

## Writing Robot Parts

It is up to the robot owner to add Robot Parts to the NuBot, making the robot
do things just right.

Here is a simple `RobotPart` which echoes any message sent to the robot,

```csharp
public sealed class Echo : RobotPart
{
    public override void Attach(IRobot robot)
    {
        robot.Listen("echo (?<Message>.*)", async ctx =>
        {
            await ctx.SendAsync("{0}", ctx.Parameters["Message"]);
        });
    }
}
```
