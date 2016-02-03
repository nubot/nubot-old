using NuBot.Automation;
using NuBot.Automation.Contexts;

namespace NuBot.Parts
{
    public class EchoWebHook : RobotPart
    {
        private class EchoRequest
        {
            public string Channel { get; set; }

            public string Message { get; set; }
        }

        public override void Attach(IRobot robot)
        {
            robot.WebHook("POST", "/echo", async ctx =>
            {
                var request = ctx.GetContent<EchoRequest>();
                var channel = ctx.GetChannel(request.Channel);

                if (channel == null)
                {
                    return;
                }

                await channel.SendAsync(request.Message);
            });
        }
    }
}