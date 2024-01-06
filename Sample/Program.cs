using Microsoft.Data.SqlClient;
namespace Sample;

internal class Program
{
    static void Main(string[] args)
    {
        int timeouts=10*1000;
        using var con = new SqlConnection("");
        con.Open();
        var sqlTask = Task.Run(() => {

            try
            {
                using var transaction = new SqlCommand("BEGIN TRAN", con);
                transaction.ExecuteNonQuery();


                using var sqlcommand = new SqlCommand("any query", con);
                sqlcommand.ExecuteNonQuery();


                using var commit = new SqlCommand("COMMIT TRAN", con);
                commit.ExecuteNonQuery();

            }
            catch (InvalidOperationException)
            {

            }


        });
        var tasks = new List<Task>();
        tasks.Add(sqlTask);
        tasks.Add(Task.Delay(timeouts) );

        var result=Task.WaitAny(sqlTask);
        if (result == 1)
        {
            Console.WriteLine("timeout");
        }
    }
}
