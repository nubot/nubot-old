using NuBot.Automation;

namespace NuBot.Parts
{
    public class HelloGoodbye : RobotPart
    {
        public override void Attach(IRobot robot)
        {
            robot.OnChannelJoin(async ctx =>
            {
                await ctx.SendAsync($"Hello, {ctx.Source.User.Name}!");
            });

            robot.OnChannelLeave(async ctx =>
            {
                await ctx.SendAsync($"Goodbye, {ctx.Source.User.Name}");
            });
        }
    }
}