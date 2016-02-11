using NuBot.Automation;

namespace NuBot.Parts
{
    public class IllBeBack : RobotPart
    {
        public override void Attach(IRobot robot)
        {
            robot.OnConnected(async ctx =>
            {
                await ctx.BroadcastAsync("I'm back.");
            });

            robot.OnDisconnected(async ctx =>
            {
                await ctx.BroadcastAsync("I'll be back.");
            });
        }
    }
}