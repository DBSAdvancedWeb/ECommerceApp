using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.Controllers;


public class ExampleController : Controller
{
    public ExampleController() {}

    public string Index(string day, int num)
    {
        return HtmlEncoder.Default.Encode($"{day} is the day {num} of the week!");
    }

    public string Other()
    {
        return "This is the Other method!";
    }

}