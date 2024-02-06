namespace BTCSpyConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            BTCSpyConsoleApp app = new();
            app.ExploreBestPrice();

            Console.WriteLine("Goodbye, World!");
        }
    }
}
