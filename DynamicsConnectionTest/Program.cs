using Microsoft.PowerPlatform.Dataverse.Client;
using System;
using System.Threading.Tasks;


namespace HelloDynamics365
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Azure app registration details
            const string clientId = "60d13472-6aff-4b26-a224-bc8055ad0097";
            const string clientSecret = "wIE8Q~R_8YgLmZPxPR1~z7WS2gaZAbgzPT--Sdy7";
            const string tenantId = "041f15fa-0c78-4cdc-897d-b522556663d7";
            const string dynamicsUrl = "https://org76451050.crm.dynamics.com";

            
            string connectionString = $"AuthType=ClientSecret;" +
                                       $"Url={dynamicsUrl};" +
                                       $"ClientId={clientId};" +
                                       $"ClientSecret={clientSecret};" +
                                       $"LoginPrompt=Never;" +
                                       $"Authority=https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

            try
            {
                // Connect to Dynamics
                using var serviceClient = new ServiceClient(connectionString);

                if (serviceClient.IsReady)
                {
                    Console.WriteLine("Connected to Dynamics 365!");

                    // Query data from an entity (e.g., Accounts)
                    string fetchAccounts = @"
                        <fetch top='5'>
                            <entity name='account'>
                                <attribute name='name' />
                                <attribute name='accountid' />
                            </entity>
                        </fetch>";

                    string fetchEventos = @"
                        <fetch>
                            <entity name='crd90_evento'>
                            </entity>
                        </fetch>";

                    var accountEntities = serviceClient.RetrieveMultiple(new Microsoft.Xrm.Sdk.Query.FetchExpression(fetchAccounts));

                    Console.WriteLine("Top 5 Accounts:");
                    foreach (var entity in accountEntities.Entities)
                    {
                        Console.WriteLine($"- {entity["name"]}");
                    }
                    var eventoEntities = serviceClient.RetrieveMultiple(new Microsoft.Xrm.Sdk.Query.FetchExpression(fetchEventos));
                    Console.WriteLine("Eventos:");
                    foreach (var entity in eventoEntities.Entities)
                    {
                        Console.WriteLine($"- {entity["crd90_newcolumn"]}");
                    }
                    
                }
                else
                {
                    Console.WriteLine("Failed to connect to Dynamics 365.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
