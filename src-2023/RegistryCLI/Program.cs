using BYTES.NET.Windows.Registry;
using System.Reflection.Metadata.Ecma335;

public class RegistryCLI
{
    /// <summary>
    /// CLI showcasing how to use the RegistryNode class
    /// 
    /// Important note:
    /// To properly use Registry keys, an app.manifest file is needed with the requestedExecutionLevel
    /// set to "requireAdministrator". Otherwise we cannot access any key under HKEY.
    /// See the CLI's app.manifest for reference.
    /// </summary>
    public static void Main()
    {
        Console.WriteLine("---------------- 1. Create RegistryNode ----------------");
        RegistryNode rg;

        try
        {
            rg = new RegistryNode("HKEY_LOCAL_MACHINE\\SOFTWARE");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Cannot access the key. Error: \n {e}");
            return;
        }

        Console.WriteLine("RegistryNode created");
        Console.WriteLine("---------------- 2. Create Keys ----------------");

        CreateKeys(rg);
        Continue();

        Console.WriteLine("---------------- 3. Set values of keys ----------------");

        SetValues(rg);
        Continue();

        Console.WriteLine("---------------- 4. Get values of subkeys ----------------");

        GetValues(rg);
        Continue();

        Console.WriteLine("---------------- 5. Get children of root ----------------");

        GetChildren(rg);
        Continue();

        Console.WriteLine("---------------- 6. Filter key by value ----------------");

        FilterKeys(rg);
        Continue();

        Console.WriteLine("---------------- 7. Delete keys ----------------");

        DeleteKeys(rg);

        Console.WriteLine("---------------- CLI FINISHED ----------------");
    }

    private static void CreateKeys(RegistryNode rg) {
        try
        {
            rg.AddKey("Test1");
            rg.AddKey("Test2");
            rg.AddNode("Test3");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine("Keys created\n");
    }

    private static void SetValues(RegistryNode rg)
    {
        try
        {
            rg.SetKeyValue("Test1", "Key1", "Teststring");
            rg.SetKeyValue("Test2", "Key2", true);
            rg.SetKeyValue("Test3", "Key3", 123);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine("Changed values of keys.\n");
    }

    private static void GetValues(RegistryNode rg)
    {
        try
        {
            Console.WriteLine(rg.GetKeyValue("Test1"));
            Console.WriteLine(rg.GetKeyValue("Test2"));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static void GetChildren(RegistryNode rg)
    {
        try
        {
            RegistryNode[] children = rg.Children;

            Console.WriteLine($"Children ({children.Length}):");
            foreach (RegistryNode child in children)
            {
                Console.WriteLine($"{child.Path}");
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static void FilterKeys(RegistryNode rg)
    {
        try
        {
            Dictionary<string, string> filter = new Dictionary<string, string> { { "Key1", "Teststring" } };

            RegistryNode[] matches = rg.SearchForChildren(filter);

            Console.WriteLine("Search for key with value \"Teststring\"\n");
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
    }

    private static void DeleteKeys(RegistryNode rg)
    {
        try
        {
            rg.DeleteKey("Test3");
            rg.DeleteKey("Test2");
            rg.DeleteKey("Test1");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Console.WriteLine("Deleted keys.");
    }

    private static void Continue()
    {
        Console.WriteLine("Press [ENTER] to continue.\n");
        Console.ReadLine();
    }
}


