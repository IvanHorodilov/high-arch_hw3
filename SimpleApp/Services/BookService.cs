using BooksApi.Models;
using MongoDB.Driver;
using Nest;
using System.Collections.Generic;
using System.Linq;

namespace BooksApi.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;
        private readonly ElasticClient _elasticClient;

        public BookService(IBookstoreDatabaseSettings settings, ElasticClient elasticClient)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _books = database.GetCollection<Book>(settings.BooksCollectionName);
            _elasticClient = elasticClient;
        }

        public List<Book> Get() 
        {
            var books = _books.Find(book => true).ToList();
            var result =_elasticClient.Search<Book>(i => i.From(0).Size(10).MatchAll());
            return books;
        }

        public Book Get(string id) =>
            _books.Find<Book>(book => book.Id == id).FirstOrDefault();

        public Book Create(Book book)
        {
            _books.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookIn) =>
            _books.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Book bookIn) =>
            _books.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) => 
            _books.DeleteOne(book => book.Id == id);
    }
}