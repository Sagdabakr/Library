using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryServices
{
    public class LibraryAssetService : ILibraryAsset
    {
        private LibraryContext _context;
        public LibraryAssetService(LibraryContext context)
        {
            _context = context;
        }
        public void Add(LibraryAsset NewAsset)
        {
            _context.Add(NewAsset);
            _context.SaveChanges();
        }

        public IEnumerable<LibraryAsset> GetAll()
        {
            var x = _context.LibraryAssets.Include(Asset => Asset.Location)
                .Include(Asset => Asset.Status);
            return x;
        }

        public string GetAuthorOrDirector(int id)
        {
            var isBook = _context.LibraryAssets.OfType<Book>().Where(asset => asset.Id == id).Any();
            return
                isBook ? _context.Books.FirstOrDefault(book => book.Id == id).Author :
                _context.Videos.FirstOrDefault(video => video.Id == id).Director ?? "UnKnown";
        }

        public LibraryAsset GetById(int id)
        {             
            return _context.LibraryAssets
                .Include(asset => asset.Status)
                .Include(asset => asset.Location)
                .FirstOrDefault(asset => asset.Id == id);
        }

        public LibraryBranch GetCurrentLocation(int id)
        {
            return GetById(id).Location;
        }

        public string GetDeweyIndex(int id)
        {
            if (_context.Books.Any(Book => Book.Id == id))
            {
                return _context.Books.FirstOrDefault(Book => Book.Id == id).DeweyIndex;
            }
            else
                return "";
        }

        public string GetIsbn(int id)
        {
            if (_context.Books.Any(book => book.Id == id))
            {
                return _context.Books.FirstOrDefault(Book => Book.Id == id).ISBN;
            }
            else
                return "";
        }

        public string GetTitle(int id)
        {
            return GetById(id).Title;
        }

        public string GetType(int id)
        {
            var isBook = _context.LibraryAssets.OfType<Book>().Where(asset => asset.Id == id).Any();           
            return isBook ? "Book" : "Video"  ?? "UnKnown" ;
        }
    }
}
