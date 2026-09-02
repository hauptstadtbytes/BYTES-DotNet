using BYTES.NET.Windows.Registry;
using System.Reflection.Metadata.Ecma335;

public class RegistryCLI
{
    public static void Main()
    {
        RegistryNode rg = new RegistryNode("HKEY_LOCAL_MACHINE\\SOFTWARE");
        Console.WriteLine("RegistryNode created");

        try
        {
            rg.AddKey("Test1");             // create subkey under root
            rg.AddKey("Test2", "Test1");    // create subkey under root/Test1
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine("Created keys Test1 and Test2");

        Console.WriteLine("Press [ENTER] to continue.");
        Console.ReadLine();

        try
        {
            rg.SetKeyValue("Test1", "Teststring", "Test1"); // create key root/Test1/Test1 and set value
            rg.SetKeyValue("Test2", true, "Test1");         // create key root/Test1/Test2 and set value
            rg.SetKeyValue("Test3", 123, "Test1\\Test2");   // create key root/Test1/Test2 and set value

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine("Changed values of keys.");

        Console.WriteLine("Press [ENTER] to continue.");
        Console.ReadLine();

        try
        {
            rg.DeleteKey("Test2", "Test1");  // delete key root/Test1/Test2
            rg.DeleteKey("Test1");           // delete key root/Test1
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine("Deleted keys Test1 and Test2");

        Console.WriteLine("Press [ENTER] to continue.");
        Console.ReadLine();

        try
        {
            rg.DeleteKey("Test1");  // delete key root/Test1 to force error
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine("Test CLI finished.");
        }
    }
}


