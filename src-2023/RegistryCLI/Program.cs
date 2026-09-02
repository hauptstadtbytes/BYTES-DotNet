using BYTES.NET.Windows.Registry;
using System.Reflection.Metadata.Ecma335;

public class RegistryCLI
{
    public static void Main()
    {
        Console.WriteLine("---------------- 1. Create RegistryNode ----------------");

        RegistryNode rg = new RegistryNode("HKEY_LOCAL_MACHINE\\SOFTWARE");
        Console.WriteLine("RegistryNode created");

        Console.WriteLine("---------------- 2. Create Keys ----------------");

        try
        {
            rg.AddKey("Test1");             // create subkey under root
            rg.AddKey("Test2", "Test1");    // create subkey under root/Test1
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine("Keys created");

        Console.WriteLine("Press [ENTER] to continue.");
        Console.ReadLine();

        Console.WriteLine("---------------- 3. Set values of keys ----------------");

        try
        {
            rg.SetKeyValue("Test1", "Teststring", "Test1"); 
            rg.SetKeyValue("Test2", true, "Test1");     
            rg.SetKeyValue("Test3", 123, "Test1\\Test2");  
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine("Changed values of keys.");

        Console.WriteLine("Press [ENTER] to continue.");
        Console.ReadLine();


        Console.WriteLine("---------------- 4. Get values of subkeys ----------------");

        try
        {
            RegistryNode test1Node = new RegistryNode(rg.Root.OpenSubKey("Test1"));
            Dictionary<string, object> test1Values = test1Node.Values;
            Console.WriteLine("Test1 values:");
            foreach (KeyValuePair<string, object> kv in test1Values)
            {
                Console.WriteLine($"  {kv.Key} = {kv.Value}");
            }

            RegistryNode test2Node = new RegistryNode(rg.Root.OpenSubKey("Test1\\Test2"));
            Dictionary<string, object> test2Values = test2Node.Values;
            Console.WriteLine("Test1/Test2 values:");
            foreach (KeyValuePair<string, object> kv in test2Values)
            {
                Console.WriteLine($"  {kv.Key} = {kv.Value}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine("Press [ENTER] to continue.");
        Console.ReadLine();

        Console.WriteLine("---------------- 5. Get children of root ----------------");

        try
        {
            RegistryNode[] children = rg.Children;

            Console.WriteLine($"Children ({children.Length}):");
            foreach (RegistryNode child in children)
            {
                Console.WriteLine($"  {child.Path}");
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine("Press [ENTER] to continue.");
        Console.ReadLine();

        Console.WriteLine("---------------- 6. Filter key by value ----------------");

        try
        {
            Dictionary<string, string> filter = new Dictionary<string, string> { { "Test1", "test" } };
            RegistryNode.EnumerationOptions[] options = new[]
            {
                RegistryNode.EnumerationOptions.IgnoreCase,
                RegistryNode.EnumerationOptions.ContainsSearch
            };

            RegistryNode[] matches = rg.SearchForChildren(filter, options);

            Console.WriteLine($"Matches ({matches.Length}):");
            foreach (RegistryNode match in matches)
            {
                Console.WriteLine($"  {match.Path}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine("Press [ENTER] to continue.");
        Console.ReadLine();

        Console.WriteLine("---------------- 7. Get value of Test1\\Test2 ----------------");

        try
        {
            Console.WriteLine(rg.GetKeyValue("Test2", "Test1"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine("Press [ENTER] to continue.");
        Console.ReadLine();

        Console.WriteLine("---------------- 8. Delete keys ----------------");

        try
        {
            rg.DeleteKey("Test2", "Test1");  // delete key root/Test1/Test2
            rg.DeleteKey("Test1");           // delete key root/Test1
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine("Deleted keys Test1 and Test2");

        Console.WriteLine("Press [ENTER] to continue.");
        Console.ReadLine();

        Console.WriteLine("---------------- 9. Delete nonexistent key to force error ----------------");

        try
        {
            rg.DeleteKey("Test1");  // delete key root/Test1 to force error
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Console.WriteLine("Test CLI finished.");
        }

        Console.WriteLine("---------------- CLI FINISHED ----------------");

    }
}


