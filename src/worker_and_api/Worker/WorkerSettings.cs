namespace WorkerAndApiPoc.Worker;

public class WorkerSettings
{
    public string Name { get; set; }    
    public string ConnectionString { get; set; }    

    public static IList<WorkerSettings> Load(IConfiguration configuration) => 
        configuration.GetSection("Workers").Get<IList<WorkerSettings>>() ?? new List<WorkerSettings>();
}