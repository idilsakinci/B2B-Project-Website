using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.ProductImageRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.ProductImageRepository.Validation;
using Business.Repositories.ProductImageRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.ProductImageRepository;
using Business.Abstract;
using Entities.Dtos;
using Core.Utilities.Business;
using Core.Aspects.Transaction;

namespace Business.Repositories.ProductImageRepository
{
    public class ProductImageManager : IProductImageService
    {
        private readonly IProductImageDal _productImageDal;
        private readonly IFileService _fileService;

        public ProductImageManager(IProductImageDal productImageDal, IFileService fileService)
        {
            _productImageDal = productImageDal;
            _fileService = fileService;
        }

        [SecuredAspect()]
        [ValidationAspect(typeof(ProductImageValidator))]
        [RemoveCacheAspect("IProductImageService.Get")]

        public async Task<IResult> Add(ProductImageAddDto productImageAddDto)
        {
            foreach(var image in productImageAddDto.Images)
            {
                IResult result = BusinessRules.Run(
                    CheckIfImageExtesionsAllow(image.FileName),
                    CheckIfImageSizeIsLessThanOneMb(image.Length)
                );

                if (result == null)
                {
                    string fileName = _fileService.FileSaveToServer(image, "C:/idil/Projects/B2B/B2BFrontend/src/assets/img/");

                    ProductImage productImage = new ProductImage()
                    {
                        Id = 0,
                        ImageUrl = fileName,
                        ProductId = productImageAddDto.ProductId,
                        IsMainImage = false
                    };

                    await _productImageDal.Add(productImage);
                }

            }

            return new SuccessResult(ProductImageMessages.Added);
        }

        [SecuredAspect()]
        [ValidationAspect(typeof(ProductImageValidator))]
        [RemoveCacheAspect("IProductImageService.Get")]

        public async Task<IResult> Update(ProductImageUpdateDto productImageUpdateDto)
        {
            IResult result = BusinessRules.Run(
                CheckIfImageExtesionsAllow(productImageUpdateDto.Image.FileName),
                CheckIfImageSizeIsLessThanOneMb(productImageUpdateDto.Image.Length)
            );

            if (result != null)
            {
                return result;
            }

            string path = @"./Content/img/" + productImageUpdateDto.ImageUrl;

           _fileService.FileDeleteToServer(path);
           
            string fileName = _fileService.FileSaveToServer(productImageUpdateDto.Image, "./Content/img/");

            ProductImage productImage = new ProductImage()
            {
                Id = productImageUpdateDto.Id,
                ImageUrl = fileName,
                ProductId = productImageUpdateDto.ProductId,
                IsMainImage = productImageUpdateDto.IsMainImage
            };

            await _productImageDal.Update(productImage);
            return new SuccessResult(ProductImageMessages.Updated);
        }

        [SecuredAspect()]
        [RemoveCacheAspect("IProductImageService.Get")]
        public async Task<IResult> Delete(ProductImage productImage)
        {
            string path = @"C:/idil/Projects/B2B/B2BFrontend/src/assets/img/" + productImage.ImageUrl;

            _fileService.FileDeleteToServer(path);

            await _productImageDal.Delete(productImage);
            return new SuccessResult(ProductImageMessages.Deleted);
        }

        [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<ProductImage>>> GetList()
        {
            return new SuccessDataResult<List<ProductImage>>(await _productImageDal.GetAll());
        }

        [SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<ProductImage>>> GetListByProductId(int productId)
        {
            return new SuccessDataResult<List<ProductImage>>(await _productImageDal.GetAll(p => p.ProductId == productId));
        }

        [SecuredAspect()]
        public async Task<IDataResult<ProductImage>> GetById(int id)
        {
            return new SuccessDataResult<ProductImage>(await _productImageDal.Get(p => p.Id == id));
        }
        private IResult CheckIfImageSizeIsLessThanOneMb(long imgSize)
        {
            decimal imgMbSize = Convert.ToDecimal(imgSize * 0.000001);
            if (imgMbSize > 5)
            {
                return new ErrorResult("The size of the image you upload must be no more than 1 MB!");
            }
            return new SuccessResult();
        }
        private IResult CheckIfImageExtesionsAllow(string fileName)
        {
            var ext = fileName.Substring(fileName.LastIndexOf('.'));
            var extension = ext.ToLower();
            List<string> AllowFileExtensions = new List<string> { ".jpg", ".jpeg", ".gif", ".png" };
            if (!AllowFileExtensions.Contains(extension))
            {
                return new ErrorResult("The image you add must be one of the types .jpg, .jpeg, .gif, .png!");
            }
            return new SuccessResult();
        }

        [SecuredAspect()]
        [RemoveCacheAspect("IProductImageService.Get")]
        [RemoveCacheAspect("IProductService.Get")]
        public async Task<IResult> SetMainImage(int id)
        {
            var productImage = await _productImageDal.Get(p => p.Id == id);
            var productImages = await _productImageDal.GetAll(p => p.ProductId == productImage.ProductId);
            foreach(var item in productImages)
            {
                item.IsMainImage = false;
                await _productImageDal.Update(item);
            }

            productImage.IsMainImage = true;
            await _productImageDal.Update(productImage);

            return new SuccessResult(ProductImageMessages.MainImageIsUpdated);
        }
    }
}
