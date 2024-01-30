using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Threading;
using System.Data.Common;
using System.Data.SQLite;

namespace LibaryProje
{
    internal class Program
    {
        

        
        static void Main(string[] args)
        {
            Library library = new Library();
            library.mainMenu();//library class'ını tanımladık. Classı içinden mainMenu() fonskiyonnuu çağırdık. 
        }
        
    }
    
    
    class Library
    {
        SQLiteConnection connt = new SQLiteConnection("Data Source=.\\LibraryDB.db;Versiyon=3");
        SQLiteCommand comd;
        SQLiteDataReader reader;
        
        
        public  void mainMenu()//Bu fonksiyon kütüphane içinde hangi menüye nasıl gidili olduğunu gosteriri.
        {
            int s;
            Console.WriteLine("*****************************\n" +
                "Velo Games Project Library\n" +
                "*****************************");
            Console.WriteLine("1-) Add a new book\n" +
                "2-) Display all books\n" +
                "3-) Display all borrowed books\n" +
                "4-) Display expired books\n" +
                "5-) Search a book\n" +
                "6-) Borrow a book\n" +
                "7-) Return the borrowed book\n" +
                "8-) Exit the program.\n");

            if (int.TryParse(Console.ReadLine(), out s))// ifi içindeki ifadeye bir çok yerde görebilirsiniz bunu anlamı girilen değerin sayıl olup olmadığını kontrol ederiz. Program konsolda olduğu için her türlü olasılığı düşünmek zorundayız.
            {
                selectionControl(s);
            }
            else {
                Console.Clear();
                Console.WriteLine("\x1b[3J");//Console.Clear() foksiyonu sadece konsol boyutu kadar temizleme işlevi yapar ama "Console.WriteLine("\x1b[3J");" bu foksiyonla beraber kullanıldığı zaman consolda olan ver bir değişkeni temizleme işlevini yerine getirir.
                Console.WriteLine("You have entered incorrectly.Please try again...\n");
                mainMenu();
            }

 
        }//okey
        
        public void selectionControl(int selection)// Fonksiyon mainMenu fonksiyonudaki menulerden hagisini gitmek isteniyorsa o fonksiyonu çağırmak iççin kullanıldı.
        {
            
            int s = selection;
            switch (s)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    addNewBook();
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    displayAllBooks();
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    displayAllBorrewedBooks();
                    break;
                case 4:
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    displayExpiredBooks();
                    break;
                case 5:
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    searchABook();
                    break;
                case 6:
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    borrewABook();
                    break;
                case 7:
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    returnBorrowedBook();
                    break;
                case 8:
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    Console.WriteLine("Wait...");
                    exitProgram();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    Console.WriteLine("You have entered incorrectly. Please try again...\n\n");
                    mainMenu();
                    break;
            }

        }//okey
        public void addNewBook() //Fonksiyon Veri tabanıa veri eklemek için kullanırız.
        {
            try
            {
                Console.WriteLine("Book Name:");
                string BookName = Console.ReadLine();

                Console.WriteLine("\nAuthor:");
                string Author = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(BookName) && !string.IsNullOrWhiteSpace(Author))//Veri tabanında not null olmasına rağmen null değer olarak kaydediyor. İf null değer girmeyi engelliyor.
                {

                    Console.WriteLine("\nISBN:");
                long ISBN = Convert.ToInt64(Console.ReadLine());

                Console.WriteLine("\nCopies:");
                int Copies = Convert.ToInt32(Console.ReadLine());

                int Borrewed = 0;
                Random random = new Random();

                int BorrewedID = random.Next(1000, 100000);

                DateTime? BorrewedTime = null;
                DateTime? TimeToReturn = null;

                connt.Open();
                
                    

                    string queryString = "INSERT INTO book (BookName, Author, ISBN, Copies, Borrewed,BorrewedID,BorrewedTime,TimeToReturn) VALUES (@BookName, @Author, @ISBN, @Copies, @Borrewed,@BorrewedID,@BorrewedTime,@TimeToReturn)";
                    SQLiteCommand comd = new SQLiteCommand(queryString, connt);

                    comd.Parameters.AddWithValue("@BookName", BookName);
                    comd.Parameters.AddWithValue("@Author", Author);
                    comd.Parameters.AddWithValue("@ISBN", ISBN);
                    comd.Parameters.AddWithValue("@Copies", Copies);
                    comd.Parameters.AddWithValue("@Borrewed", Borrewed);
                    comd.Parameters.AddWithValue("@BorrewedID", BorrewedID);
                    comd.Parameters.AddWithValue("@BorrewedTime", BorrewedTime.HasValue ? (object)BorrewedTime.Value : DBNull.Value);
                    comd.Parameters.AddWithValue("@TimeToReturn", TimeToReturn.HasValue ? (object)TimeToReturn.Value : DBNull.Value);



                    int rowsAffected = comd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("\nThe book was recorded.");
                    }
                    else
                    {
                        Console.WriteLine("\nThe book could not be saved.");
                    }
                }
                else
                {
                    Console.WriteLine("\nAuthor or Book Name cannot be left blank.");
                }
                
                        
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nAn Error Occurred During the Process.\nCheck the values ​​you entered..." + ex.Message);
            }
            finally
            {
                connt.Close();
                Console.WriteLine("\n7-)New add books");
                Console.WriteLine("8-)return a main menu");

                int a;
                if (int.TryParse(Console.ReadLine(), out a))
                {
                    switch (a)
                    {
                        case 7: 
                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                            addNewBook();
                            break;
                        case 8:
                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                            mainMenu();
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                            Console.WriteLine("\nYou made a mistake. You return to the main menu.");
                            mainMenu();
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    Console.WriteLine("\nYou did not enter a valid number. You return to the main menu.");
                    mainMenu();
                }
            }

            
            
        }

        public void displayAllBooks()//veri tabanıdaki bütün kitapları gostermemize yarayan fonksiyondur.
        {
            
                connt.Open();

                comd = new SQLiteCommand("select * from book where Copies>0", connt);
                reader = comd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($" Book Name: {reader["BookName"]}\n" +
                        $" Author: {reader["Author"]}\n" +
                        $" ISBN: {reader["ISBN"]}\n" +
                        $" Copies: {reader["Copies"]}\n" +
                        $" Borrowed: {reader["Borrewed"]}\n" +
                        "----------------------------------------\n");
                }
            reader.Close();
            connt.Close();

            Console.WriteLine("\n8-)return a main menu");

            int a;
            if (int.TryParse(Console.ReadLine(), out a))
            {
                switch (a)
                {
                    case 8:
                        Console.Clear();
                        Console.WriteLine("\x1b[3J");
                        mainMenu();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("\x1b[3J");
                        Console.WriteLine("\nYou made a mistake. You return to the main menu.");
                        mainMenu();
                        break;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                Console.WriteLine("\nYou did not enter a valid number. You return to the main menu.");
                mainMenu();
            }





        }//okey
        
        public  void displayAllBorrewedBooks()//Veri tabanıdaki Ödünç alımış bütün kitapları gösteren foksiyondur.
        {
            
            connt.Open();
            int i = 0;
            comd = new SQLiteCommand("select * from book where Borrewed>0 and Copies>0 and BorrewedTime < date('now', '+14 days')", connt);
            reader = comd.ExecuteReader();
            
                while (reader.Read())
                {
                    Console.WriteLine($" Book Name: {reader["BookName"]}\n" +
                        $" Author: {reader["Author"]}\n" +
                        $" ISBN: {reader["ISBN"]}\n" +
                        $" Copies: {reader["Copies"]}\n" +
                        $" Borrewed: {reader["Borrewed"]}\n" +
                        $" BorrewedTime: {reader["BorrewedTime"]}\n" +
                        $" BorrewedID: {reader["BorrewedID"]}\n" +
                        $" TimeToReturn: {reader["TimeToReturn"]}\n" +
                        $"\n------------------------------------\n");
                i++;
                }
                if(i==0)
                Console.WriteLine("\nNo borrowed books");
                comd.Cancel();
            connt.Close();
            reader.Close();
            
            Console.WriteLine("\n8-)return a main menu");

            int a;
            if (int.TryParse(Console.ReadLine(), out a))
            {
                switch (a)
                {
                    case 8:
                        Console.Clear();
                        Console.WriteLine("\x1b[3J");
                        mainMenu();
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("\x1b[3J");
                        Console.WriteLine("\nYou made a mistake. You return to the main menu.");
                        mainMenu();
                        break;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                Console.WriteLine("\nYou did not enter a valid number. You return to the main menu.");
                mainMenu();
            }

        }//okey
        public void displayExpiredBooks()//Ödünç alınmış kitapların süresi geçmiş olan kitapları göruntulemeyi sağlayan fponksiyondur. 
        {
            
            try
            {
                int i = 0;
                connt.Open();
                comd = new SQLiteCommand("select * from book where Borrewed>0 and  BorrewedTime < date('now', '-14 days') ", connt);
                reader = comd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($" Book Name: {reader["BookName"]}\n" +
                        $" Author: {reader["Author"]}\n" +
                        $" ISBN: {reader["ISBN"]}\n" +
                        $" Copies: {reader["Copies"]}\n" +
                        $" BorrewedTime: {reader["BorrewedTime"]}\n" +
                        $" BorrewedID: {reader["BorrewedID"]}\n" +
                        $" TimeToReturn: {reader["TimeToReturn"]}" +
                        $": \nExpired\n" +
                        $"----------------------------------------");
                    i++;
                }
                if(i==0)
                    Console.WriteLine("\nThere are no expired books");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("\nAn unexpected error occurred" + ex.Message);
            }
            finally
            {
               
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                }

                connt.Close();

                Console.WriteLine("\n8-)return a main menu");

                int a;
                if (int.TryParse(Console.ReadLine(), out a))
                {
                    switch (a)
                    {
                        case 8:
                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                            mainMenu();
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                            Console.WriteLine("\nYou made a mistake. You return to the main menu.");
                            mainMenu();
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    Console.WriteLine("\nYou did not enter a valid number. You return to the main menu.");
                    mainMenu();
                }
            }

            
        }
        public void searchABook()//veri tabanında kitap veya yazar aramamızı sağlayan foksiyondur.
        {
            try
            {
                Console.WriteLine("1-) Book name\n" +
                "2-)Author Name");
                int i;
                if (int.TryParse(Console.ReadLine(), out i))
                {
                    switch (i)
                    {

                        case 1:
                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                            connt.Open();
                            Console.Write("Enter the book name you are looking for:\n");
                            string searchBookName = Console.ReadLine();
                            comd = new SQLiteCommand("SELECT * FROM book WHERE lower(BookName) LIKE '%' || lower(?) || '%'", connt);
                            comd.Parameters.AddWithValue("@SearchBookName", searchBookName);
                            reader = comd.ExecuteReader();
                            int j = 0;
                            while (reader.Read())
                            {
                                Console.WriteLine($" Book Name: {reader["BookName"]}\n" +
                                                  $" Author: {reader["Author"]}\n" +
                                                  $" ISBN: {reader["ISBN"]}\n" +
                                                  $" Copies: {reader["Copies"]}\n" +
                                                  $" Borrowed: {reader["Borrewed"]}\n" +
                                                  "----------------------------------------");
                                j++;
                            }if(j==0)
                                Console.WriteLine("\nThe book you are looking for could not be found...");
                            reader.Close(); 
                            connt.Close();

                            break;
                        case 2:
                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                            connt.Open();
                            Console.Write("\nEnter the author name you are looking for:");
                            string authorBookName = Console.ReadLine();
                            comd = new SQLiteCommand("SELECT * FROM book WHERE lower(Author) LIKE '%' || lower(?) || '%'", connt);
                            comd.Parameters.AddWithValue("@AuthorBookName", authorBookName);
                            reader = comd.ExecuteReader();
                            int k = 0;
                            while (reader.Read())
                            {
                                Console.WriteLine($" Book Name: {reader["BookName"]}\n" +
                                                  $" Author: {reader["Author"]}\n" +
                                                  $" ISBN: {reader["ISBN"]}\n" +
                                                  $" Copies: {reader["Copies"]}\n" +
                                                  $" Borrowed: {reader["Borrewed"]}\n" +
                                                  "----------------------------------------");
                                k++;
                            }
                            if (k == 0)
                                Console.WriteLine("\nThe author you are looking for could not be found...");
                            reader.Close(); 
                            connt.Close();
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                            Console.WriteLine("\nThe entered value is invalid, please try again...");
                            searchABook();
                            break;
                            
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    Console.WriteLine("\nYou have made a mistake....You are returning to the Main Menu..");
                    mainMenu();

                }
            }
            catch(Exception ex)
            {
                Console.Clear();
                Console.WriteLine("\x1b[3J");
                Console.WriteLine("\nYou made a mistake. Returning to the main menu." + ex.Message);
                mainMenu();
            }
            finally {
                Console.WriteLine("\n7-)New Search  book");
                Console.WriteLine("8-)return a main menu");

                int a;
                if (int.TryParse(Console.ReadLine(), out a))
                {
                    switch (a)
                    {
                        case 7:
                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                            searchABook();
                            break;
                        case 8:
                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                            mainMenu();
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                            Console.WriteLine("\nYou made a mistake. You return to the main menu.");
                            mainMenu();
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    Console.WriteLine("\nYou did not enter a valid number. You return to the main menu.");
                    mainMenu();
                }
            }
               
        }//okey
        public void borrewABook()// veri tabanından kitap ödünç alma işlevini sağlayan foksiyondur.
        {
           
            try
            {
                DateTime TimeDatea = DateTime.Now;
                string TimeDate = TimeDatea.ToString("yyyy-MM-dd");
                DateTime Timetoretur = TimeDatea.AddDays(14);
                string Timetoreturn = Timetoretur.ToString("yyyy-MM-dd");
                connt.Open();
                
                comd = new SQLiteCommand("select * from book where Copies>0", connt);
                reader = comd.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine($" Book Name: {reader["BookName"]}\n" +
                        $" Author: {reader["Author"]}\n" +
                        $" ISBN: {reader["ISBN"]}\n" +
                        $" Copies: {reader["Copies"]}\n" +
                        $" Borrewed: {reader["Borrewed"]}\n");

                }
                reader.Close();
                Console.WriteLine("----------------------------\nEnter the ISBN of the book you want to borrow");
                long check = Convert.ToInt64(Console.ReadLine());
                comd = new SQLiteCommand("update book set Copies=(Copies-1), Borrewed=(Borrewed+1) ,borrewedTime =@TimeDate,TimeToReturn=@Timetoreturn where  Copies>0 and ISBN=@Check", connt);
                comd.Parameters.AddWithValue("@TimeDate", TimeDate);
                comd.Parameters.AddWithValue("@Timetoreturn", Timetoreturn);
                comd.Parameters.AddWithValue("@Check", check);

                int rowsAffected = comd.ExecuteNonQuery();

                if (rowsAffected > 0)
                    Console.WriteLine("\nThe book was borrowed..");
                else
                    Console.WriteLine("\n\nThe book could not be borrowed. \nIncorrect ISBN number\n");

            }
            catch (Exception)
            {
                
                Console.WriteLine("\n\nThe entered character is invalid...\n" + "İnput string is not in the correct format");
            }
            finally
            {
                connt.Close();
                Console.WriteLine("\n7-)New Borrow a book");
                Console.WriteLine("8-)return a main menu");

                int a;
                if (int.TryParse(Console.ReadLine(), out a))
                {
                    switch (a)
                    {
                        case 7:
                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                            borrewABook();
                            break;
                        case 8:
                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                            mainMenu();
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                            Console.WriteLine("\nYou made a mistake. You return to the main menu.");
                            mainMenu();
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    Console.WriteLine("\nYou did not enter a valid number. You return to the main menu.");
                    mainMenu();
                }
            } 
        }
        public void returnBorrowedBook()// ödünç alınan kitabuı geri bırakmak için kullaılan fonksiyondur.
        {
            try
            {
                connt.Open();
                DateTime TimeDatea = DateTime.Now;
                string TimeDate = TimeDatea.ToString("yyyy-MM-dd");
                DateTime? BorrewedTime = null;
                DateTime? TimeToReturn = null;

                comd = new SQLiteCommand("select * from book where borrewed > 0 ", connt);
                
                reader = comd.ExecuteReader();
                int i = 0;
                while (reader.Read())
                {
                    Console.WriteLine($" Book Name: {reader["BookName"]}\n" +
                        $" Author: {reader["Author"]}\n" +
                        $" ISBN: {reader["ISBN"]}\n" +
                        $" Copies: {reader["Copies"]}\n" +
                        $" Borrowed: {reader["Borrewed"]}\n" +
                        $" BorrewedTime: {reader["BorrewedTime"]}\n" +
                        $" BorrewedID: {reader["BorrewedID"]}\n" +
                        $" TimeToReturn: {reader["TimeToReturn"]}\n" +
                        "----------------------------------------");
                    i++;
                }
                reader.Close();
                if (i==0)
                    Console.WriteLine("\nNo borrowed books.");
                else
                {
                    Console.WriteLine("\nEnter the BorrwedID of the borrowed book");
                    int borrewedID;
                    if (int.TryParse(Console.ReadLine(), out borrewedID))
                    {

                        string queryString = "SELECT Borrewed FROM book WHERE BorrewedID = @BorrewedID GROUP BY Borrewed";
                        comd = new SQLiteCommand(queryString, connt);
                        comd.Parameters.AddWithValue("@BorrewedID", borrewedID);
                        int j = 0;
                        reader = comd.ExecuteReader();
                        while (reader.Read())
                        {
                            j = Convert.ToInt32(reader["Borrewed"]);
                            
                        }
                        if (j == 1)
                        {
                            comd = new SQLiteCommand("update book set Copies=(Copies+1), Borrewed=(Borrewed-1),BorrewedTime=@BorrewedTime,TimeToReturn=@TimeToReturn where BorrewedID=@BorrewedID" /*and borrewed=1*/, connt);
                            comd.Parameters.AddWithValue("@BorrewedID", borrewedID);
                            comd.Parameters.AddWithValue("@BorrewedTime", BorrewedTime.HasValue ? (object)BorrewedTime.Value : DBNull.Value);
                            comd.Parameters.AddWithValue("@TimeToReturn", TimeToReturn.HasValue ? (object)TimeToReturn.Value : DBNull.Value);
                        }
                        else
                        {
                            comd = new SQLiteCommand("update book set Copies=(Copies+1), Borrewed=(Borrewed-1) where BorrewedID=@BorrewedID and  borrewed>0 and borrewed<>1", connt);
                            comd.Parameters.AddWithValue("@BorrewedID", borrewedID);
                        }

                        int rowsAffected = comd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                            Console.WriteLine("\nThe book was left back..");
                        else
                            Console.WriteLine("\nThe book was not returned, the BorrewedID is incorrect..");
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\x1b[3J");
                        Console.WriteLine("\nThe entered character is invalid...");
                    }
                }

            }
            catch (Exception ex)
            {       
                Console.WriteLine("\nAn unexpected error occurred..." + ex.Message);
            }
            finally {
                connt.Close();
                Console.WriteLine("\nn7-)Return to borrow book");
                Console.WriteLine("8-)return a main menu");

                int a;
                if (int.TryParse(Console.ReadLine(), out a))
                {
                    switch (a)
                    {
                        case 7:
                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                            returnBorrowedBook();
                            break;
                        case 8:
                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                            mainMenu();
                            break;
                        default:

                            Console.Clear();
                            Console.WriteLine("\x1b[3J");
                            Console.WriteLine("\nYou made a mistake. We return to the main menu.");
                            mainMenu();
                            break;
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\x1b[3J");
                    Console.WriteLine("\nYou did not enter a valid number. You return to the main menu.");
                    mainMenu();
                }

            }

        }

        public void exitProgram()//Console programından cıkış yapmak için kullanırız.
        {
            Environment.Exit(0);
        }
        
    }
}

