using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Models.Catalog;
using LibraryData;
using Microsoft.AspNetCore.Mvc;
 
namespace Library.Controllers
{
    public class CatalogController : Controller
    {
        private ILibraryAsset _assets;
        public CatalogController( ILibraryAsset _libraryAsset)
        {
            _assets = _libraryAsset;
        }

        public IActionResult Index()                
        {
            var allAssets = _assets.GetAll();
            var ListingResult = allAssets
                .Select(
                asset =>
                    new AssetIndexListingModel {
                        id = asset.Id,
                        ImageUrl = asset.ImageUrl,
                        Title = asset.Title,
                        AuthorOrDirector = _assets.GetAuthorOrDirector(asset.Id),
                        Type = _assets.GetType(asset.Id),
                        DeweyCallNumber = _assets.GetDeweyIndex(asset.Id),
                        NumberOfCopies = asset.NumberOfCopies
                    }
                    ).ToList();

            var model = new AssetIndexModel()
            {
                Assets = ListingResult
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var asset = _assets.GetById(id);
            var returned = new AssetDetailModel
            {
                AssetId = asset.Id,
                ImageUrl = asset.ImageUrl,
                AuthorOrDirector = _assets.GetAuthorOrDirector(id),                
                DeweyCallNumber = _assets.GetDeweyIndex(id),
                Title = asset.Title,
                Type = _assets.GetType(id),
                Status = asset.Status.Name,
                ISBN = _assets.GetIsbn(id),
                Cost = asset.Cost,
                CurrentLocation = _assets.GetCurrentLocation(id).Name,
                Year = asset.Year,                
            };
            return View(returned);
        }
    }
}
