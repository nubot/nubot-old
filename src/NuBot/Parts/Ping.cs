using NuBot.Automation;

namespace NuBot.Parts
{
    public sealed class Ping : RobotPart
    {
        public override void Attach(IRobot robot)
        {
            robot.Listen("ping", async ctx =>
            {
                await ctx.SendAsync("PONG");
            });
        }
    }
}
