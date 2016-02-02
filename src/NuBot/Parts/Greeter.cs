using NuBot.Automation;

namespace NuBot.Parts
{
    public class Greeter : RobotPart
    {
        public override void Attach(IRobot robot)
        {
            robot.OnJoin(async ctx =>
            {
                await ctx.SendAsync($"Hello, {ctx.Message.UserName}!");
            });
        }
    }
}