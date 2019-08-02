using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace XUnitTestProject1
{
    [Route("gateway")]
    public class MockGatewayController : ControllerBase
    {
        [HttpPost]
        public ActionResult<string> Logon([FromBody]LogonRequest request)
        {
            if (request.From == "China")
            {
                var behavior = MockGatewayData.MockBehavior;
                return behavior.LogonResult();
            }

            return string.Empty;
        }
    }

    public class LogonRequest
    {
        public string From { get; set; }
    }

    public interface IGatewayMockBehavior
    {
        ActionResult<string> LogonResult();
    }

    public class MockGatewayData
    {
        public static IGatewayMockBehavior MockBehavior { get; set; }
    }
}