namespace JORDAN_2T.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IHomeRepository Home { get; }
        IApplicationUserRepository ApplicationUsers { get; }

        IMovieRepository Movies { get; }
        IPhotoRepository MoviePhotos { get; }
        ICategoryRepository Categorys {get;}
        ISubCategoryRepository SubCategorys {get;}
        IShoppingCartRepository ShoppingCarts { get; }
        IOrderDetailRepository OrderDetails { get; }
        IOrderRepository Orders { get; }
        void Save();
    }
}
