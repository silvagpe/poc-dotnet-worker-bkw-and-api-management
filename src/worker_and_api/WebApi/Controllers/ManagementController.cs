namespace WorkerAndApiPoc.WebApi.Controllers;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WorkerAndApiPoc.Worker;

[ApiController]
[Route("/api/management")]
public class ManagementController : Controller
{
    private readonly WorkerManager _workerManager;
    public ManagementController(WorkerManager workerManager)
    {
        _workerManager = workerManager;
    }

    [HttpGet]
    public ActionResult GetServices()
    {
        return Ok(_workerManager.GetWorkers());
    }

    [Route("start-service/{name}")]
    [HttpGet]
    public ActionResult StartService(string name)
    {
        return Ok(_workerManager.StartWorker(name));
    }

    [Route("services")]
    [HttpGet]
    public ActionResult StopService(string name)
    {
        return Ok(_workerManager.StopWorker(name));
    }
}