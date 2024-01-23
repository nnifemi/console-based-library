using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project___Console_Based_Management_Library_System
{
    class Program
    {
        static List<Book> libraryCatalog = new List<Book>();
        static Dictionary<string, List<Book>> booksByGenre = new Dictionary<string, List<Book>>();
        static List<string> availableGenres = new List<string> { "Fiction", "Non-Fiction", "Mystery", "Science Fiction", "Romance", "Fantasy" };

        // Main method to run the library management system
        static void Main(string[] args)
        {
            while (true)
            {
                DisplayMenu(); // Display the main menu

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddBook(); // Call method to add a new book
                        break;
                    case "2":
                        ViewBooks(); // Call method to view books
                        break;
                    case "3":
                        SearchBooks(); // Call method to search for books
                        break;
                    case "4":
                        BorrowBook(); // Call method to borrow a book
                        break;
                    case "5":
                        ReturnBook(); // Call method to return a borrowed book
                        break;
                    case "6":
                        Environment.Exit(0); // Exit the program
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear(); // Clear the console for a cleaner interface
            }
        }

        // Display the main menu options
        static void DisplayMenu()
        {
            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. View Books");
            Console.WriteLine("3. Search Books");
            Console.WriteLine("4. Borrow Book");
            Console.WriteLine("5. Return Book");
            Console.WriteLine("6. Exit");
        }

        // Add a new book to the library
        static void AddBook()
        {
            Console.Write("Enter Title: ");
            string title = Console.ReadLine();
            Console.Write("Enter Author: ");
            string author = Console.ReadLine();

            int year;
            while (true)
            {
                Console.Write("Enter Publication Year: ");

                try
                {
                    year = int.Parse(Console.ReadLine());
                    break; // Break the loop if parsing is successful
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input format. Please enter a valid integer for the publication year.");
                }
            }

            DisplayGenres();
            Console.Write("Enter Genre: ");
            string genre = Console.ReadLine();

            if (!availableGenres.Contains(genre))
            {
                Console.WriteLine("Invalid genre. Please select from the provided list.");
                return;
            }

            // Create a new book object and add it to the library catalog
            Book newBook = new Book
            {
                Id = Guid.NewGuid().ToString(),
                Title = title,
                Author = author,
                Year = year,
                Genre = genre,
                Status = BookStatus.Available
            };

            libraryCatalog.Add(newBook);

            // Update the booksByGenre dictionary
            if (booksByGenre.ContainsKey(genre))
            {
                booksByGenre[genre].Add(newBook);
            }
            else
            {
                booksByGenre[genre] = new List<Book> { newBook };
            }

            Console.WriteLine("Book added successfully.");
        }

        // View books based on user choice
        static void ViewBooks()
        {
            Console.WriteLine("1. All Books");
            Console.WriteLine("2. By Genre");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayBooks(libraryCatalog); // Display all books in the catalog
                    break;
                case "2":
                    DisplayGenres();
                    Console.Write("Enter Genre: ");
                    string genre = Console.ReadLine();

                    // Display books by the specified genre
                    if (!availableGenres.Contains(genre))
                    {
                        Console.WriteLine("Invalid genre. Please select from the provided list.");
                        break;
                    }

                    if (booksByGenre.ContainsKey(genre))
                    {
                        DisplayBooks(booksByGenre[genre]);
                    }
                    else
                    {
                        Console.WriteLine("No books found in the specified genre.");
                    }
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        // Display a list of genres for the user to choose from
        static void DisplayGenres()
        {
            Console.WriteLine("Available Genres:");
            foreach (var genre in availableGenres)
            {
                Console.WriteLine($"- {genre}");
            }
        }

        // Display a list of books
        static void DisplayBooks(List<Book> books)
        {
            foreach (var book in books)
            {
                Console.WriteLine($"ID: {book.Id}, Title: {book.Title}, Author: {book.Author}, Year: {book.Year}, Genre: {book.Genre}, Status: {book.Status}");
            }
        }

        // Search for books based on user choice
        static void SearchBooks()
        {
            Console.WriteLine("1. By Title");
            Console.WriteLine("2. By Author");
            Console.WriteLine("3. By ID");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter Title: ");
                    string title = Console.ReadLine();
                    SearchBooksByTitle(title); // Search books by title
                    break;
                case "2":
                    Console.Write("Enter Author: ");
                    string author = Console.ReadLine();
                    SearchBooksByAuthor(author); // Search books by author
                    break;
                case "3":
                    Console.Write("Enter ID: ");
                    string id = Console.ReadLine();
                    SearchBooksById(id); // Search books by ID
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        // Search for books by title
        static void SearchBooksByTitle(string title)
        {
            List<Book> results = libraryCatalog.Where(b => b.Title.ToLower().Contains(title.ToLower())).ToList();
            DisplayBooks(results);
        }

        // Search for books by author
        static void SearchBooksByAuthor(string author)
        {
            List<Book> results = libraryCatalog.Where(b => b.Author.ToLower().Contains(author.ToLower())).ToList();
            DisplayBooks(results);
        }

        // Search for books by ID
        static void SearchBooksById(string id)
        {
            Book result = libraryCatalog.FirstOrDefault(b => b.Id == id);
            if (result != null)
            {
                DisplayBooks(new List<Book> { result });
            }
            else
            {
                Console.WriteLine("Book not found with the specified ID.");
            }
        }

        // Borrow a book by changing its status to Borrowed
        static void BorrowBook()
        {
            Console.Write("Enter Book ID to borrow: ");
            string id = Console.ReadLine();
            Book book = libraryCatalog.FirstOrDefault(b => b.Id == id);

            if (book != null && book.Status == BookStatus.Available)
            {
                book.Status = BookStatus.Borrowed;
                Console.WriteLine("Book borrowed successfully.");
            }
            else
            {
                Console.WriteLine("Book not available for borrowing.");
            }
        }

        // Return a borrowed book by changing its status to Available
        static void ReturnBook()
        {
            Console.Write("Enter Book ID to return: ");
            string id = Console.ReadLine();
            Book book = libraryCatalog.FirstOrDefault(b => b.Id == id);

            if (book != null && book.Status == BookStatus.Borrowed)
            {
                book.Status = BookStatus.Available;
                Console.WriteLine("Book returned successfully.");
            }
            else
            {
                Console.WriteLine("Invalid book or book not borrowed.");
            }
        }

        // Book class to represent a book with properties
        class Book
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string Author { get; set; }
            public int Year { get; set; }
            public string Genre { get; set; }
            public BookStatus Status { get; set; }
        }

        // Enumeration for book status (Available or Borrowed)
        enum BookStatus
        {
            Available,
            Borrowed
        }
    }
}