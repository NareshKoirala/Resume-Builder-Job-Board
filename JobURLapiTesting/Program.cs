using System.Text;
using System.Text.Json;


JobFetch jobFetch = new JobFetch();

JobBoard job1 = new JobBoard();

var data = await jobFetch.AdzunaJobFetch(job1);

foreach (var i in data)
{
    if (i is JobBoardReturn)
    {
        Console.WriteLine(i.id);
        Console.WriteLine(i.title);
        Console.WriteLine(i.company);
        Console.WriteLine(i.location);
        Console.WriteLine(i.description);
        Console.WriteLine(i.salaryRange);
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
    }
}

Console.WriteLine();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();


public class JobBoard
{
}

public class JobBoardReturn 
{
}


public class JobFetch
{
}

