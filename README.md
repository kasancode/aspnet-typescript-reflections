# aspnet-typescript-reflections
This library generate Typescript definitions and methods to asscess ASP.net MVC Controller.


## Usage

Attach ReturnObjectTypeAttribute at Controller Method

```C#
public class HomeController : Controller
{
    [ReturnObjectType(typeof(List<SampleModel>))]
    public ActionResult Add(string name, string note)
    {
        return Json(new List<SampleModel>());
    }
}
```

Create T4 (.tt) to generaete Typescript definitions and methods.

```
<#
var ag = new AjaxMethodGenerator(typeof(SampleWebApplication.Controllers.HomeController));
#>
<#=ag.Generate()#>
```
