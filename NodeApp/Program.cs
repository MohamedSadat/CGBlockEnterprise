using CGBlockBussines;
using CGBlockDA;
using CGBlockInfra.Models;
namespace NodeApp
{
    internal class Program
    {
       static CAppModel app;
        static void Main(string[] args)
        {
            app=new CAppModel();
            GlobalConfig.StartupApp(app);
            BLNode xnode = new BLNode(app);
        
            Console.WriteLine("Hello, World!");
            Console.WriteLine($"Node {app.Node.NodeName}");

            Console.WriteLine($"Address {app.Node.Address}");
            Console.WriteLine($"PublicKey {app.Node.PublicKey}");

            Console.WriteLine($"Peers {app.Node.Peers.Count}");
    
            xnode.GenerateGenisusBlock();
            Console.Read();
        }
    }
}