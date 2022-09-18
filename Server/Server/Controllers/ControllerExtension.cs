using Microsoft.AspNetCore.Mvc;
using Server.Data;

namespace Server.Controllers;

public static class ControllerExtension
{
    public static IActionResult StatusCode(this Controller controller, ServerResponse response)
    {
        return controller.StatusCode(response.StatusCode, response.Description);
    }
}