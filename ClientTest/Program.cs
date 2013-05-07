using System;
using System.IO;
using System.Collections.Generic;

namespace ClientTest
{
    class Program
    {
        static void Main(string[] args)
        {
            makeChoice();   
            /*WebServeur.ImageTransfertClient client = new WebServeur.ImageTransfertClient();
            MemoryStream imageStream = new MemoryStream(lireFichier(@"d:\mer4.jpg"));
            // Appel de notre web method    
            WebServeur.ImageInfo info = new WebServeur.ImageInfo();
            info.imageID = "mer4";
            info.AlbumID = "mer";
            client.UploadImage(info,imageStream);
            Console.Out.WriteLine("Transfert Terminé");
            Console.ReadLine();
            */
        }

        private static byte[] lireFichier(string chemin)
        {
            byte[] data = null;
            FileInfo fileInfo = new FileInfo(chemin);
            int nbBytes = (int)fileInfo.Length;
            FileStream fileStream = new FileStream(chemin, FileMode.Open,
            FileAccess.Read);
            BinaryReader br = new BinaryReader(fileStream);
            data = br.ReadBytes(nbBytes);
            return data;
        }

        private static String welcome() 
        {
            String choice;
            Console.WriteLine("******Hello,what you want to do?******");
            Console.WriteLine("1.Add user.");
            Console.WriteLine("2.Delete user.");
            Console.WriteLine("3.Delete an album.");
            Console.WriteLine("4.Delete an photo.");
            Console.WriteLine("5.Quit.");
            Console.WriteLine("******Please enter your choice with a number.******");
            choice = Console.ReadLine();
            return choice;
        }

        private static void makeChoice() 
        {
            // Instanciation de la référence de service
            WebServeur.ImageTransfertClient client = new WebServeur.ImageTransfertClient();
            String choice;
            while ((choice = welcome()) != "5")
            {
                switch (choice)
                {
                    case "1":
                        Console.WriteLine("*******Please enter the userName.******");
                        String userName = Console.ReadLine();
                        Console.WriteLine("*******Please enter the password.******");
                        String password = Console.ReadLine();
                        client.addUser(userName, password);
                        Console.WriteLine("******Add sucessfully the user {0} in the BD.*******", userName);
                        break;
                    case "2":
                        String[] users = client.showUsers();
                        Console.WriteLine("******Now we have these users in our BD: ******");
                        for (int i = 0; i < users.Length; i++) 
                        {
                            Console.WriteLine(users[i]);
                        }
                        Console.WriteLine("******Please enter the userName which you want to delete.******");
                        client.deleteUser(Console.ReadLine());
                        Console.WriteLine("******Delete sucessfully.*******");
                        break;
                    case "3":
                        String[] albums = client.showAlbums();
                        Console.WriteLine("******Now we have these albums in our BD: ******");
                        for (int i = 0; i < albums.Length; i++) 
                        {
                            Console.WriteLine(albums[i]);
                        }
                        Console.WriteLine("******Please enter the albumID which you want to delete.******");
                        client.DelectAlbum(Console.ReadLine());
                        Console.WriteLine("******Delete sucessfully.*******");
                        break;
                    case "4":
                        Console.WriteLine("******Please enter the imageID which you want to delete.******");
                        client.DelectImage(Console.ReadLine());
                        Console.WriteLine("******Delete sucessfully.*******");
                        break;
                    default:
                        Console.WriteLine("******We dont have this choice.Sorry!******");
                        break;
                }
            }
            Console.WriteLine("******Thank you for using.Bye bye!******");
            Console.ReadLine();
        }
    }
}