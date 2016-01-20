using NuBot.Automation;

namespace NuBot.Parts
{
    public sealed class DoIt : RobotPart
    {
        public override void Attach(IRobot robot)
        {
            robot.Hear("do\\s*it", async ctx =>
            {
                await ctx.SendAsync("http://i.imgur.com/pKove8A.gif");
            });
        }
    }
}
