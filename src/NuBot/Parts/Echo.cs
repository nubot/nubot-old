using NuBot.Automation;

namespace NuBot.Parts
{
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
}
