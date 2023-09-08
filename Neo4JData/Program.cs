using Neo4j.Driver;

namespace Neo4JData
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                //docker run  --name testneo4j  -p7474:7474 -p7687:7687 -d  -v $HOME/neo4j/data:/data -v $HOME/neo4j/logs:/logs -v $HOME/neo4j/import:/var/lib/neo4j/import -v $HOME/neo4j/plugins:/plugins --env NEO4J_AUTH=neo4j/password neo4j:latest
                var driver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "password"));
                var session = driver.AsyncSession();

                using (var reader = new StreamReader("freemints-nodes.CSV"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        var values = line.Split(';');

                        var personA = values[0];
                        var payment = values[1];
                        var personB = values[2];

                        var cypherQuery = "MERGE (a:Person {address: $personA}) " +
                                          "MERGE (b:Person {address: $personB}) " +
                                          "MERGE (a)-[:PAYMENT {amount: $payment}]->(b)";

                        var parameters = new { personA, payment, personB };

                        await session.RunAsync(cypherQuery, parameters);
                    }
                }

                await session.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

    }
}